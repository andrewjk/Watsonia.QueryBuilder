using System;
using System.Collections.Generic;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// A user-defined function in the database.
	/// </summary>
	public sealed class UserDefinedFunction : StatementPart
	{
		/// <summary>
		/// Gets the type of the statement part.
		/// </summary>
		/// <value>
		/// The type of the part.
		/// </value>
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.UserDefinedFunction;
			}
		}

		/// <summary>
		/// Gets the name of the function.
		/// </summary>
		/// <value>
		/// The name of the function.
		/// </value>
		public string Name { get; internal set; }

		/// <summary>
		/// Gets the alias to use for the function.
		/// </summary>
		/// <value>
		/// The alias.
		/// </value>
		public string Alias { get; internal set; }

		/// <summary>
		/// Gets the paths of related items and collections to include when loading data from this function.
		/// </summary>
		/// <value>
		/// The include paths.
		/// </value>
		public List<Parameter> Parameters { get; } = new List<Parameter>();

		/// <summary>
		/// Initializes a new instance of the <see cref="UserDefinedFunction" /> class.
		/// </summary>
		internal UserDefinedFunction()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UserDefinedFunction" /> class.
		/// </summary>
		/// <param name="name">The name of the function.</param>
		/// <param name="alias">The alias to use for the function.</param>
		public UserDefinedFunction(string name, string alias = null)
		{
			this.Name = name;
			this.Alias = alias;
		}

		/// <summary>
		/// Returns a <see cref="string" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="string" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return this.Name + (!string.IsNullOrEmpty(this.Alias) ? " As " + this.Alias : "");
		}
	}
}
