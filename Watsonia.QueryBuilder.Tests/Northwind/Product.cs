using System;
using System.Linq;

namespace Watsonia.QueryBuilder.Tests.Northwind
{
	public class Product
	{
		public virtual int ProductID { get; set; }

		public virtual string ProductName { get; set; }

		public virtual decimal UnitPrice { get; set; }
	}
}
