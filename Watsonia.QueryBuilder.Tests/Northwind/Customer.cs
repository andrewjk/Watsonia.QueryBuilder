using System;
using System.Linq;
using System.Collections.Generic;

namespace Watsonia.QueryBuilder.Tests.Northwind
{
	public class Customer
	{
		public virtual string City { get; set; }

		public virtual string CompanyName { get; set; }

		public virtual string ContactName { get; set; }

		public virtual string Country { get; set; }

		public virtual string CustomerID { get; set; }

		public List<Order> Orders { get; set; }

		public virtual string Phone { get; set; }
	}
}
