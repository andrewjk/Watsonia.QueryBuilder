using System;
using System.Collections.Generic;
using System.Text;

namespace Watsonia.QueryBuilder.Tests.Northwind
{
	public class NorthwindMapper : DatabaseMapper
	{
		public override string GetTableName(Type type)
		{
			return type.Name + "s";
		}
	}
}
