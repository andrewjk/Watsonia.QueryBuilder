using System;
using System.Collections.Generic;
using System.Text;

namespace Watsonia.QueryBuilder.Tests.Entities
{
	public class Customer
	{
		public string Name { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public string BusinessNumber { get; set; }
		public int LicenseCount { get; set; }

		public Address BillingAddress { get; set; }
		public Address ShippingAddress { get; set; }
	}
}
