using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Watsonia.QueryBuilder.Tests.Northwind
{
	public class NorthwindMapper : DatabaseMapper
	{
		public NorthwindMapper()
		{
			this.EntityNamespace = this.GetType().Namespace;
		}

		public override string GetTableName(Type type)
		{
			return type.Name + "s";
		}

		public override string GetPrimaryKeyColumnName(Type type)
		{
			return type.Name + "ID";
		}

		public override string GetForeignKeyColumnName(PropertyInfo property)
		{
			if (property.Name == "City")
			{
				return property.Name;
			}
			return base.GetForeignKeyColumnName(property);
		}
	}
}
