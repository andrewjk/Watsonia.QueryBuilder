using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// Maps .NET objects to database objects.
	/// </summary>
	public class DatabaseMapper
	{
		/// <summary>
		/// Gets the namespace in which entity classes are located.
		/// </summary>
		/// <value>
		/// The entity namespace.
		/// </value>
		public string EntityNamespace { get; set; } = "$";

		/// <summary>
		/// Gets the name of the schema for the supplied type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public virtual string GetSchemaName(Type type)
		{
			return string.Empty;
		}

		/// <summary>
		/// Gets the name of the table for the supplied type.
		/// </summary>
		/// <remarks>
		/// For a Book item, this would return "Book" by default but might be overridden to return "Books" or something different.
		/// </remarks>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public virtual string GetTableName(Type type)
		{
			return type.Name;
		}

		/// <summary>
		/// Gets the name of the procedure for the supplied type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public virtual string GetProcedureName(Type type)
		{
			return type.Name.Replace("Procedure", "");
		}

		/// <summary>
		/// Gets the name of the function for the supplied type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public virtual string GetFunctionName(Type type)
		{
			return type.Name.Replace("Function", "");
		}

		/// <summary>
		/// Gets the name of the column for the supplied property.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns></returns>
		public virtual string GetColumnName(PropertyInfo property)
		{
			return property.Name;
		}

		/// <summary>
		/// Gets the name of the primary key column.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public virtual string GetPrimaryKeyColumnName(Type type)
		{
			return "ID";
		}

		/// <summary>
		/// Determines whether the supplied property contains a related entity item.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns>
		///   <c>true</c> if the supplied property contains a related entity item; otherwise, <c>false</c>.
		/// </returns>
		public virtual bool IsRelatedItem(PropertyInfo property)
		{
			return ShouldMapType(property.PropertyType);
		}

		/// <summary>
		/// Gets the name of the foreign key column for the supplied property.
		/// </summary>
		/// <remarks>
		/// For a Book.Author property, this would return "AuthorID" by default.
		/// </remarks>
		/// <param name="property">The property.</param>
		/// <returns></returns>
		public virtual string GetForeignKeyColumnName(PropertyInfo property)
		{
			return property.Name + "ID";
		}

		/// <summary>
		/// Determines whether the supplied type is a stored procedure.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		///   <c>true</c> if the supplied type is a stored procedure; otherwise, <c>false</c>.
		/// </returns>
		public virtual bool IsProcedure(Type type)
		{
			return type.Name.EndsWith("Procedure");
		}

		/// <summary>
		/// Determines whether the supplied type is a user-defined function.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		///   <c>true</c> if the supplied type is a user-defined function; otherwise, <c>false</c>.
		/// </returns>
		public virtual bool IsFunction(Type type)
		{
			return type.Name.EndsWith("Function");
		}

		/// <summary>
		/// Determines whether the class with the supplied type should be mapped to the database.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public virtual bool ShouldMapType(Type type)
		{
			return (type.Namespace == this.EntityNamespace);
		}
	}
}
