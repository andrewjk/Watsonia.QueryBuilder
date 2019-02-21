using System;
using System.Collections.Generic;
using System.Linq;

namespace Watsonia.QueryBuilder.Tests.Northwind
{
	public class Order
	{
		public virtual Customer Customer { get; set; }

		public virtual string CustomerID { get; set; }

		public virtual DateTime OrderDate { get; set; }

		public virtual int OrderID { get; set; }

		public List<OrderDetail> Details { get; set; }
	}
}
