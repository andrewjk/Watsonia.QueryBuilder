//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data.Common;
//using System.IO;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Xml.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Watsonia.Data.Tests.Northwind;

//// TODO: Implement all double commented (////) tests

//namespace Watsonia.Data.Tests.Queries
//{
//	[TestClass]
//	public class QueryLanguageTests
//	{
//		private static NorthwindDatabase db = new NorthwindDatabase();
//		private static Dictionary<string, string> sqlServerBaselines = new Dictionary<string, string>();
//		private static Dictionary<string, string> sqliteBaselines = new Dictionary<string, string>();

//		[ClassInitialize]
//		public static void Initialize(TestContext context)
//		{
//			string sqlServerFileName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Queries\QueryLanguageBaselinesSqlServer.xml";
//			if (!string.IsNullOrEmpty(sqlServerFileName) && File.Exists(sqlServerFileName))
//			{
//				XDocument doc = XDocument.Load(sqlServerFileName);
//				sqlServerBaselines = doc.Root.Elements("baseline").ToDictionary(e => (string)e.Attribute("key"), e => e.Value);
//			}

//			string sqliteFileName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Queries\QueryLanguageBaselinesSQLite.xml";
//			if (!string.IsNullOrEmpty(sqliteFileName) && File.Exists(sqliteFileName))
//			{
//				XDocument doc = XDocument.Load(sqliteFileName);
//				sqliteBaselines = doc.Root.Elements("baseline").ToDictionary(e => (string)e.Attribute("key"), e => e.Value);
//			}
//		}

//		[TestMethod]
//		public void TestWhere()
//		{
//			TestQuery(
//				"TestWhere",
//				db.Customers.Where(c => c.City == "London"));
//		}

//		[TestMethod]
//		public void TestCompareEntityEqual()
//		{
//			Customer alfki = new Customer { CustomerID = "ALFKI" };
//			TestQuery(
//				"TestCompareEntityEqual",
//				db.Customers.Where(c => c == alfki));
//		}

//		[TestMethod]
//		public void TestCompareEntityNotEqual()
//		{
//			Customer alfki = new Customer { CustomerID = "ALFKI" };
//			TestQuery(
//				"TestCompareEntityNotEqual",
//				db.Customers.Where(c => c != alfki));
//		}

//		[TestMethod]
//		public void TestCompareConstructedEqual()
//		{
//			TestQuery(
//				"TestCompareConstructedEqual",
//				db.Customers.Where(c => new { x = c.City } == new { x = "London" }));
//		}

//		////[TestMethod]
//		////public void TestCompareConstructedMultiValueEqual()
//		////{
//		////	TestQuery(
//		////		"TestCompareConstructedMultiValueEqual",
//		////		db.Customers.Where(c => new { x = c.City, y = c.Country } == new { x = "London", y = "UK" }));
//		////}

//		////[TestMethod]
//		////public void TestCompareConstructedMultiValueNotEqual()
//		////{
//		////	TestQuery(
//		////		"TestCompareConstructedMultiValueNotEqual",
//		////		db.Customers.Where(c => new { x = c.City, y = c.Country } != new { x = "London", y = "UK" }));
//		////}

//		[TestMethod]
//		public void TestCompareConstructed()
//		{
//			TestQuery(
//				"TestCompareConstructed",
//				db.Customers.Where(c => new { x = c.City } == new { x = "London" }));
//		}

//		[TestMethod]
//		public void TestSelectScalar()
//		{
//			TestQuery(
//				"TestSelectScalar",
//				db.Customers.Select(c => c.City));
//		}

//		[TestMethod]
//		public void TestSelectAnonymousOne()
//		{
//			TestQuery(
//				"TestSelectAnonymousOne",
//				db.Customers.Select(c => new { c.City }));
//		}

//		[TestMethod]
//		public void TestSelectAnonymousTwo()
//		{
//			TestQuery(
//				"TestSelectAnonymousTwo",
//				db.Customers.Select(c => new { c.City, c.Phone }));
//		}

//		[TestMethod]
//		public void TestSelectAnonymousThree()
//		{
//			TestQuery(
//				"TestSelectAnonymousThree",
//				db.Customers.Select(c => new { c.City, c.Phone, c.Country }));
//		}

//		[TestMethod]
//		public void TestSelectCustomerTable()
//		{
//			TestQuery(
//				"TestSelectCustomerTable",
//				db.Customers);
//		}

//		[TestMethod]
//		public void TestSelectCustomerIdentity()
//		{
//			TestQuery(
//				"TestSelectCustomerIdentity",
//				db.Customers.Select(c => c));
//		}

//		////[TestMethod]
//		////public void TestSelectAnonymousWithObject()
//		////{
//		////	TestQuery(
//		////		"TestSelectAnonymousWithObject",
//		////		db.Customers.Select(c => new { c.City, c }));
//		////}

//		////[TestMethod]
//		////public void TestSelectAnonymousNested()
//		////{
//		////	TestQuery(
//		////		"TestSelectAnonymousNested",
//		////		db.Customers.Select(c => new { c.City, Country = new { c.Country } }));
//		////}

//		////[TestMethod]
//		////public void TestSelectAnonymousEmpty()
//		////{
//		////	TestQuery(
//		////		"TestSelectAnonymousEmpty",
//		////		db.Customers.Select(c => new { }));
//		////}

//		////[TestMethod]
//		////public void TestSelectAnonymousLiteral()
//		////{
//		////	TestQuery(
//		////		"TestSelectAnonymousLiteral",
//		////		db.Customers.Select(c => new { X = 10 }));
//		////}

//		[TestMethod]
//		public void TestSelectConstantInt()
//		{
//			TestQuery(
//				"TestSelectConstantInt",
//				db.Customers.Select(c => 0));
//		}

//		[TestMethod]
//		public void TestSelectConstantNullString()
//		{
//			TestQuery(
//				"TestSelectConstantNullString",
//				db.Customers.Select(c => (string)null));
//		}

//		[TestMethod]
//		public void TestSelectLocal()
//		{
//			int x = 10;
//			TestQuery(
//				"TestSelectLocal",
//				db.Customers.Select(c => x));
//		}

//		////[TestMethod]
//		////public void TestSelectNestedCollection()
//		////{
//		////	TestQuery(
//		////		"TestSelectNestedCollection",
//		////		from c in db.Customers
//		////		where c.City == "London"
//		////		select db.Orders.Where(o => o.CustomerID == c.CustomerID && o.OrderDate.Year == 1997).Select(o => o.OrderID));
//		////}

//		////[TestMethod]
//		////public void TestSelectNestedCollectionInAnonymousType()
//		////{
//		////	TestQuery(
//		////		"TestSelectNestedCollectionInAnonymousType",
//		////		from c in db.Customers
//		////		where c.CustomerID == "ALFKI"
//		////		select new { Foos = db.Orders.Where(o => o.CustomerID == c.CustomerID && o.OrderDate.Year == 1997).Select(o => o.OrderID) });
//		////}

//		////[TestMethod]
//		////public void TestJoinCustomerOrders()
//		////{
//		////	TestQuery(
//		////		"TestJoinCustomerOrders",
//		////		from c in db.Customers
//		////		join o in db.Orders on c.CustomerID equals o.CustomerID
//		////		select new { c.ContactName, o.OrderID });
//		////}

//		////[TestMethod]
//		////public void TestJoinMultiKey()
//		////{
//		////	TestQuery(
//		////		"TestJoinMultiKey",
//		////		from c in db.Customers
//		////		join o in db.Orders on new { a = c.CustomerID, b = c.CustomerID } equals new { a = o.CustomerID, b = o.CustomerID }
//		////		select new { c, o });
//		////}

//		////[TestMethod]
//		////public void TestJoinIntoCustomersOrders()
//		////{
//		////	TestQuery(
//		////		"TestJoinIntoCustomersOrders",
//		////		from c in db.Customers
//		////		join o in db.Orders on c.CustomerID equals o.CustomerID into ords
//		////		select new { cust = c, ords = ords.ToList() });
//		////}

//		////[TestMethod]
//		////public void TestJoinIntoCustomersOrdersCount()
//		////{
//		////	TestQuery(
//		////		"TestJoinIntoCustomersOrdersCount",
//		////		from c in db.Customers
//		////		join o in db.Orders on c.CustomerID equals o.CustomerID into ords
//		////		select new { cust = c, ords = ords.Count() });
//		////}

//		////[TestMethod]
//		////public void TestJoinIntoDefaultIfEmpty()
//		////{
//		////	TestQuery(
//		////		"TestJoinIntoDefaultIfEmpty",
//		////		from c in db.Customers
//		////		join o in db.Orders on c.CustomerID equals o.CustomerID into ords
//		////		from o in ords.DefaultIfEmpty()
//		////		select new { c, o });
//		////}

//		////[TestMethod]
//		////public void TestSelectManyCustomerOrders()
//		////{
//		////	TestQuery(
//		////		"TestSelectManyCustomerOrders",
//		////		from c in db.Customers
//		////		from o in db.Orders
//		////		where c.CustomerID == o.CustomerID
//		////		select new { c.ContactName, o.OrderID }
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestMultipleJoinsWithJoinConditionsInWhere()
//		////{
//		////	TestQuery(
//		////		"TestMultipleJoinsWithJoinConditionsInWhere",
//		////		from c in db.Customers
//		////		from o in db.Orders
//		////		from d in db.OrderDetails
//		////		where o.CustomerID == c.CustomerID && o.OrderID == d.OrderID
//		////		where c.CustomerID == "ALFKI"
//		////		select d.ProductID
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestMultipleJoinsWithMissingJoinCondition()
//		////{
//		////	TestQuery(
//		////		"TestMultipleJoinsWithMissingJoinCondition",
//		////		from c in db.Customers
//		////		from o in db.Orders
//		////		from d in db.OrderDetails
//		////		where o.CustomerID == c.CustomerID /*&& o.OrderID == d.OrderID*/
//		////		where c.CustomerID == "ALFKI"
//		////		select d.ProductID
//		////		);
//		////}

//		[TestMethod]
//		public void TestOrderBy()
//		{
//			TestQuery(
//				"TestOrderBy",
//				db.Customers.OrderBy(c => c.CustomerID)
//				);
//		}

//		[TestMethod]
//		public void TestOrderBySelect()
//		{
//			TestQuery(
//				"TestOrderBySelect",
//				db.Customers.OrderBy(c => c.CustomerID).Select(c => c.ContactName)
//				);
//		}

//		[TestMethod]
//		public void TestOrderByOrderBy()
//		{
//			TestQuery(
//				"TestOrderByOrderBy",
//				db.Customers.OrderBy(c => c.CustomerID).OrderBy(c => c.Country).Select(c => c.City)
//				);
//		}

//		[TestMethod]
//		public void TestOrderByThenBy()
//		{
//			TestQuery(
//				"TestOrderByThenBy",
//				db.Customers.OrderBy(c => c.CustomerID).ThenBy(c => c.Country).Select(c => c.City)
//				);
//		}

//		[TestMethod]
//		public void TestOrderByDescending()
//		{
//			TestQuery(
//				"TestOrderByDescending",
//				db.Customers.OrderByDescending(c => c.CustomerID).Select(c => c.City)
//				);
//		}

//		[TestMethod]
//		public void TestOrderByDescendingThenBy()
//		{
//			TestQuery(
//				"TestOrderByDescendingThenBy",
//				db.Customers.OrderByDescending(c => c.CustomerID).ThenBy(c => c.Country).Select(c => c.City)
//				);
//		}

//		[TestMethod]
//		public void TestOrderByDescendingThenByDescending()
//		{
//			TestQuery(
//				"TestOrderByDescendingThenByDescending",
//				db.Customers.OrderByDescending(c => c.CustomerID).ThenByDescending(c => c.Country).Select(c => c.City)
//				);
//		}

//		////[TestMethod]
//		////public void TestOrderByJoin()
//		////{
//		////	TestQuery(
//		////		"TestOrderByJoin",
//		////		from c in db.Customers.OrderBy(c => c.CustomerID)
//		////		join o in db.Orders.OrderBy(o => o.OrderID) on c.CustomerID equals o.CustomerID
//		////		select new { CustomerID = c.CustomerID, o.OrderID }
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestOrderBySelectMany()
//		////{
//		////	TestQuery(
//		////		"TestOrderBySelectMany",
//		////		from c in db.Customers.OrderBy(c => c.CustomerID)
//		////		from o in db.Orders.OrderBy(o => o.OrderID)
//		////		where c.CustomerID == o.CustomerID
//		////		select new { c.ContactName, o.OrderID }
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestGroupBy()
//		////{
//		////	TestQuery(
//		////		"TestGroupBy",
//		////		db.Customers.GroupBy(c => c.City)
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestGroupBySelectMany()
//		////{
//		////	TestQuery(
//		////		"TestGroupBySelectMany",
//		////		db.Customers.GroupBy(c => c.City).SelectMany(g => g)
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestGroupBySum()
//		////{
//		////	TestQuery(
//		////		"TestGroupBySum",
//		////		db.Orders.GroupBy(o => o.CustomerID).Select(g => g.Sum(o => o.OrderID))
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestGroupByCount()
//		////{
//		////	TestQuery(
//		////		"TestGroupByCount",
//		////		db.Orders.GroupBy(o => o.CustomerID).Select(g => g.Count())
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestGroupByLongCount()
//		////{
//		////	TestQuery(
//		////		"TestGroupByLongCount",
//		////		db.Orders.GroupBy(o => o.CustomerID).Select(g => g.LongCount()));
//		////}

//		////[TestMethod]
//		////public void TestGroupBySumMinMaxAvg()
//		////{
//		////	TestQuery(
//		////		"TestGroupBySumMinMaxAvg",
//		////		db.Orders.GroupBy(o => o.CustomerID).Select(g =>
//		////			new
//		////			{
//		////				Sum = g.Sum(o => o.OrderID),
//		////				Min = g.Min(o => o.OrderID),
//		////				Max = g.Max(o => o.OrderID),
//		////				Avg = g.Average(o => o.OrderID)
//		////			})
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestGroupByWithResultSelector()
//		////{
//		////	TestQuery(
//		////		"TestGroupByWithResultSelector",
//		////		db.Orders.GroupBy(o => o.CustomerID, (k, g) =>
//		////			new
//		////			{
//		////				Sum = g.Sum(o => o.OrderID),
//		////				Min = g.Min(o => o.OrderID),
//		////				Max = g.Max(o => o.OrderID),
//		////				Avg = g.Average(o => o.OrderID)
//		////			})
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestGroupByWithElementSelectorSum()
//		////{
//		////	TestQuery(
//		////		"TestGroupByWithElementSelectorSum",
//		////		db.Orders.GroupBy(o => o.CustomerID, o => o.OrderID).Select(g => g.Sum())
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestGroupByWithElementSelector()
//		////{
//		////	TestQuery(
//		////		"TestGroupByWithElementSelector",
//		////		db.Orders.GroupBy(o => o.CustomerID, o => o.OrderID)
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestGroupByWithElementSelectorSumMax()
//		////{
//		////	TestQuery(
//		////		"TestGroupByWithElementSelectorSumMax",
//		////		db.Orders.GroupBy(o => o.CustomerID, o => o.OrderID).Select(g => new { Sum = g.Sum(), Max = g.Max() })
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestGroupByWithAnonymousElement()
//		////{
//		////	TestQuery(
//		////		"TestGroupByWithAnonymousElement",
//		////		db.Orders.GroupBy(o => o.CustomerID, o => new { o.OrderID }).Select(g => g.Sum(x => x.OrderID))
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestGroupByWithTwoPartKey()
//		////{
//		////	TestQuery(
//		////		"TestGroupByWithTwoPartKey",
//		////		db.Orders.GroupBy(o => new { CustomerID = o.CustomerID, o.OrderDate }).Select(g => g.Sum(o => o.OrderID))
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestOrderByGroupBy()
//		////{
//		////	TestQuery(
//		////		"TestOrderByGroupBy",
//		////		db.Orders.OrderBy(o => o.OrderID).GroupBy(o => o.CustomerID).Select(g => g.Sum(o => o.OrderID))
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestOrderByGroupBySelectMany()
//		////{
//		////	TestQuery(
//		////		"TestOrderByGroupBySelectMany",
//		////		db.Orders.OrderBy(o => o.OrderID).GroupBy(o => o.CustomerID).SelectMany(g => g)
//		////		);
//		////}

//		[TestMethod]
//		public void TestSumWithNoArg()
//		{
//			TestQuery(
//				"TestSumWithNoArg",
//				() => db.Orders.Select(o => o.OrderID).Sum()
//				);
//		}

//		[TestMethod]
//		public void TestSumWithArg()
//		{
//			TestQuery(
//				"TestSumWithArg",
//				() => db.Orders.Sum(o => o.OrderID)
//				);
//		}

//		[TestMethod]
//		public void TestCountWithNoPredicate()
//		{
//			TestQuery(
//				"TestCountWithNoPredicate",
//				() => db.Orders.Count()
//				);
//		}

//		[TestMethod]
//		public void TestCountWithPredicate()
//		{
//			TestQuery(
//				"TestCountWithPredicate",
//				() => db.Orders.Count(o => o.CustomerID == "ALFKI")
//				);
//		}

//		[TestMethod]
//		public void TestDistinct()
//		{
//			TestQuery(
//				"TestDistinct",
//				db.Customers.Distinct()
//				);
//		}

//		[TestMethod]
//		public void TestDistinctScalar()
//		{
//			TestQuery(
//				"TestDistinctScalar",
//				db.Customers.Select(c => c.City).Distinct()
//				);
//		}

//		[TestMethod]
//		public void TestOrderByDistinct()
//		{
//			TestQuery(
//				"TestOrderByDistinct",
//				db.Customers.OrderBy(c => c.CustomerID).Select(c => c.City).Distinct()
//				);
//		}

//		////[TestMethod]
//		////public void TestDistinctOrderBy()
//		////{
//		////	TestQuery(
//		////		"TestDistinctOrderBy",
//		////		db.Customers.Select(c => c.City).Distinct().OrderBy(c => c)
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestDistinctGroupBy()
//		////{
//		////	TestQuery(
//		////		"TestDistinctGroupBy",
//		////		db.Orders.Distinct().GroupBy(o => o.CustomerID)
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestGroupByDistinct()
//		////{
//		////	TestQuery(
//		////		"TestGroupByDistinct",
//		////		db.Orders.GroupBy(o => o.CustomerID).Distinct()
//		////		);

//		////}

//		[TestMethod]
//		public void TestDistinctCount()
//		{
//			TestQuery(
//				"TestDistinctCount",
//				() => db.Customers.Distinct().Count()
//				);
//		}

//		////[TestMethod]
//		////public void TestSelectDistinctCount()
//		////{
//		////	TestQuery(
//		////		"TestSelectDistinctCount",
//		////		() => db.Customers.Select(c => c.City).Distinct().Count()
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestSelectSelectDistinctCount()
//		////{
//		////	TestQuery(
//		////		"TestSelectSelectDistinctCount",
//		////		() => db.Customers.Select(c => c.City).Select(c => c).Distinct().Count()
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestDistinctCountPredicate()
//		////{
//		////	TestQuery(
//		////		"TestDistinctCountPredicate",
//		////		() => db.Customers.Distinct().Count(c => c.CustomerID == "ALFKI")
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestDistinctSumWithArg()
//		////{
//		////	TestQuery(
//		////		"TestDistinctSumWithArg",
//		////		() => db.Orders.Distinct().Sum(o => o.OrderID)
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestSelectDistinctSum()
//		////{
//		////	TestQuery(
//		////		"TestSelectDistinctSum",
//		////		() => db.Orders.Select(o => o.OrderID).Distinct().Sum()
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestTake()
//		////{
//		////	TestQuery(
//		////		"TestTake",
//		////		db.Orders.Take(5)
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestTakeDistinct()
//		////{
//		////	TestQuery(
//		////		"TestTakeDistinct",
//		////		db.Orders.Take(5).Distinct()
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestDistinctTake()
//		////{
//		////	TestQuery(
//		////		"TestDistinctTake",
//		////		db.Orders.Distinct().Take(5)
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestDistinctTakeCount()
//		////{
//		////	TestQuery(
//		////		"TestDistinctTakeCount",
//		////		() => db.Orders.Distinct().Take(5).Count()
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestTakeDistinctCount()
//		////{
//		////	TestQuery(
//		////		"TestTakeDistinctCount",
//		////		() => db.Orders.Take(5).Distinct().Count()
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestSkip()
//		////{
//		////	TestQuery(
//		////		"TestSkip",
//		////		db.Customers.OrderBy(c => c.ContactName).Skip(5)
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestTakeSkip()
//		////{
//		////	TestQuery(
//		////		"TestTakeSkip",
//		////		db.Customers.OrderBy(c => c.ContactName).Take(10).Skip(5)
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestDistinctSkip()
//		////{
//		////	TestQuery(
//		////		"TestDistinctSkip",
//		////		db.Customers.Distinct().OrderBy(c => c.ContactName).Skip(5)
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestSkipTake()
//		////{
//		////	TestQuery(
//		////		"TestSkipTake",
//		////		db.Customers.OrderBy(c => c.ContactName).Skip(5).Take(10)
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestDistinctSkipTake()
//		////{
//		////	TestQuery(
//		////		"TestDistinctSkipTake",
//		////		db.Customers.Distinct().OrderBy(c => c.ContactName).Skip(5).Take(10)
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestSkipDistinct()
//		////{
//		////	TestQuery(
//		////		"TestSkipDistinct",
//		////		db.Customers.OrderBy(c => c.ContactName).Skip(5).Distinct()
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestSkipTakeDistinct()
//		////{
//		////	TestQuery(
//		////		"TestSkipTakeDistinct",
//		////		db.Customers.OrderBy(c => c.ContactName).Skip(5).Take(10).Distinct()
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestTakeSkipDistinct()
//		////{
//		////	TestQuery(
//		////		"TestTakeSkipDistinct",
//		////		db.Customers.OrderBy(c => c.ContactName).Take(10).Skip(5).Distinct()
//		////		);
//		////}

//		[TestMethod]
//		public void TestFirst()
//		{
//			TestQuery(
//				"TestFirst",
//				() => db.Customers.OrderBy(c => c.ContactName).First()
//				);
//		}

//		[TestMethod]
//		public void TestFirstPredicate()
//		{
//			TestQuery(
//				"TestFirstPredicate",
//				() => db.Customers.OrderBy(c => c.ContactName).First(c => c.City == "London")
//				);
//		}

//		[TestMethod]
//		public void TestWhereFirst()
//		{
//			TestQuery(
//				"TestWhereFirst",
//				() => db.Customers.OrderBy(c => c.ContactName).Where(c => c.City == "London").First()
//				);
//		}

//		[TestMethod]
//		public void TestFirstOrDefault()
//		{
//			TestQuery(
//				"TestFirstOrDefault",
//				() => db.Customers.OrderBy(c => c.ContactName).FirstOrDefault()
//				);
//		}

//		[TestMethod]
//		public void TestFirstOrDefaultPredicate()
//		{
//			TestQuery(
//				"TestFirstOrDefaultPredicate",
//				() => db.Customers.OrderBy(c => c.ContactName).FirstOrDefault(c => c.City == "London")
//				);
//		}

//		[TestMethod]
//		public void TestWhereFirstOrDefault()
//		{
//			TestQuery(
//				"TestWhereFirstOrDefault",
//				() => db.Customers.OrderBy(c => c.ContactName).Where(c => c.City == "London").FirstOrDefault()
//				);
//		}

//		[TestMethod]
//		public void TestReverse()
//		{
//			TestQuery(
//				"TestReverse",
//				db.Customers.OrderBy(c => c.ContactName).Reverse()
//				);
//		}

//		[TestMethod]
//		public void TestReverseReverse()
//		{
//			TestQuery(
//				"TestReverseReverse",
//				db.Customers.OrderBy(c => c.ContactName).Reverse().Reverse()
//				);
//		}

//		////[TestMethod]
//		////public void TestReverseWhereReverse()
//		////{
//		////	TestQuery(
//		////		"TestReverseWhereReverse",
//		////		db.Customers.OrderBy(c => c.ContactName).Reverse().Where(c => c.City == "London").Reverse()
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestReverseTakeReverse()
//		////{
//		////	TestQuery(
//		////		"TestReverseTakeReverse",
//		////		db.Customers.OrderBy(c => c.ContactName).Reverse().Take(5).Reverse()
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestReverseWhereTakeReverse()
//		////{
//		////	TestQuery(
//		////		"TestReverseWhereTakeReverse",
//		////		db.Customers.OrderBy(c => c.ContactName).Reverse().Where(c => c.City == "London").Take(5).Reverse()
//		////		);
//		////}

//		[TestMethod]
//		public void TestLast()
//		{
//			TestQuery(
//				"TestLast",
//				() => db.Customers.OrderBy(c => c.ContactName).Last()
//				);
//		}

//		[TestMethod]
//		public void TestLastPredicate()
//		{
//			TestQuery(
//				"TestLastPredicate",
//				() => db.Customers.OrderBy(c => c.ContactName).Last(c => c.City == "London")
//				);
//		}

//		[TestMethod]
//		public void TestWhereLast()
//		{
//			TestQuery(
//				"TestWhereLast",
//				() => db.Customers.OrderBy(c => c.ContactName).Where(c => c.City == "London").Last()
//				);
//		}

//		[TestMethod]
//		public void TestLastOrDefault()
//		{
//			TestQuery(
//				"TestLastOrDefault",
//				() => db.Customers.OrderBy(c => c.ContactName).LastOrDefault()
//				);
//		}

//		[TestMethod]
//		public void TestLastOrDefaultPredicate()
//		{
//			TestQuery(
//				"TestLastOrDefaultPredicate",
//				() => db.Customers.OrderBy(c => c.ContactName).LastOrDefault(c => c.City == "London")
//				);
//		}

//		[TestMethod]
//		public void TestWhereLastOrDefault()
//		{
//			TestQuery(
//				"TestWhereLastOrDefault",
//				() => db.Customers.OrderBy(c => c.ContactName).Where(c => c.City == "London").LastOrDefault()
//				);
//		}

//		[TestMethod]
//		public void TestSingle()
//		{
//			TestQuery(
//				"TestSingle",
//				() => db.Customers.Single());
//		}

//		[TestMethod]
//		public void TestSinglePredicate()
//		{
//			TestQuery(
//				"TestSinglePredicate",
//				() => db.Customers.Single(c => c.CustomerID == "ALFKI")
//				);
//		}

//		[TestMethod]
//		public void TestWhereSingle()
//		{
//			TestQuery(
//				"TestWhereSingle",
//				() => db.Customers.Where(c => c.CustomerID == "ALFKI").Single()
//				);
//		}

//		////[TestMethod]
//		////public void TestSingleOrDefault()
//		////{
//		////	TestQuery(
//		////		"TestSingleOrDefault",
//		////		() => db.Customers.SingleOrDefault());
//		////}

//		[TestMethod]
//		public void TestSingleOrDefaultPredicate()
//		{
//			TestQuery(
//				"TestSingleOrDefaultPredicate",
//				() => db.Customers.SingleOrDefault(c => c.CustomerID == "ALFKI")
//				);
//		}

//		[TestMethod]
//		public void TestWhereSingleOrDefault()
//		{
//			TestQuery(
//				"TestWhereSingleOrDefault",
//				() => db.Customers.Where(c => c.CustomerID == "ALFKI").SingleOrDefault()
//				);
//		}

//		////[TestMethod]
//		////public void TestAnyWithSubquery()
//		////{
//		////	TestQuery(
//		////		"TestAnyWithSubquery",
//		////		db.Customers.Where(c => db.Orders.Where(o => o.CustomerID == c.CustomerID).Any(o => o.OrderDate.Year == 1997))
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestAnyWithSubqueryNoPredicate()
//		////{
//		////	TestQuery(
//		////		"TestAnyWithSubqueryNoPredicate",
//		////		db.Customers.Where(c => db.Orders.Where(o => o.CustomerID == c.CustomerID).Any())
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestAnyWithLocalCollection()
//		////{
//		////	string[] ids = new[] { "ABCDE", "ALFKI" };
//		////	TestQuery(
//		////		"TestAnyWithLocalCollection",
//		////		db.Customers.Where(c => ids.Any(id => c.CustomerID == id))
//		////		);
//		////}

//		[TestMethod]
//		public void TestAnyTopLevel()
//		{
//			TestQuery(
//				"TestAnyTopLevel",
//				() => db.Customers.Any()
//				);
//		}

//		////[TestMethod]
//		////public void TestAllWithSubquery()
//		////{
//		////	TestQuery(
//		////		"TestAllWithSubquery",
//		////		db.Customers.Where(c => db.Orders.Where(o => o.CustomerID == c.CustomerID).All(o => o.OrderDate.Year == 1997))
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestAllWithLocalCollection()
//		////{
//		////	string[] patterns = new[] { "a", "e" };

//		////	TestQuery(
//		////		"TestAllWithLocalCollection",
//		////		db.Customers.Where(c => patterns.All(p => c.ContactName.Contains(p)))
//		////		);
//		////}

//		[TestMethod]
//		public void TestAllTopLevel()
//		{
//			TestQuery(
//				"TestAllTopLevel",
//				() => db.Customers.All(c => c.ContactName.StartsWith("a"))
//				);
//		}

//		[TestMethod]
//		public void TestContainsWithSubquery()
//		{
//			TestQuery(
//				"TestContainsWithSubquery",
//				db.Customers.Where(c => db.Orders.Select(o => o.CustomerID).Contains(c.CustomerID))
//				);
//		}

//		[TestMethod]
//		public void TestContainsWithLocalCollection()
//		{
//			string[] ids = new[] { "ABCDE", "ALFKI" };
//			TestQuery(
//				"TestContainsWithLocalCollection",
//				db.Customers.Where(c => ids.Contains(c.CustomerID))
//				);
//		}

//		[TestMethod]
//		public void TestContainsTopLevel()
//		{
//			TestQuery(
//				"TestContainsTopLevel",
//				() => db.Customers.Select(c => c.CustomerID).Contains("ALFKI")
//				);
//		}

//		////[TestMethod]
//		////public void TestCoalesce()
//		////{
//		////	TestQuery(
//		////		"TestCoalesce",
//		////		db.Customers.Where(c => (c.City ?? "Seattle") == "Seattle"));
//		////}

//		////[TestMethod]
//		////public void TestCoalesce2()
//		////{
//		////	TestQuery(
//		////		"TestCoalesce2",
//		////		db.Customers.Where(c => (c.City ?? c.Country ?? "Seattle") == "Seattle"));
//		////}

//		[TestMethod]
//		public void TestStringLength()
//		{
//			TestQuery(
//				"TestStringLength",
//				db.Customers.Where(c => c.City.Length == 7));
//		}

//		[TestMethod]
//		public void TestStringStartsWithLiteral()
//		{
//			TestQuery(
//				"TestStringStartsWithLiteral",
//				db.Customers.Where(c => c.ContactName.StartsWith("M")));
//		}

//		[TestMethod]
//		public void TestStringStartsWithColumn()
//		{
//			TestQuery(
//				"TestStringStartsWithColumn",
//				db.Customers.Where(c => c.ContactName.StartsWith(c.ContactName)));
//		}

//		[TestMethod]
//		public void TestStringEndsWithLiteral()
//		{
//			TestQuery(
//				"TestStringEndsWithLiteral",
//				db.Customers.Where(c => c.ContactName.EndsWith("s")));
//		}

//		[TestMethod]
//		public void TestStringEndsWithColumn()
//		{
//			TestQuery(
//				"TestStringEndsWithColumn",
//				db.Customers.Where(c => c.ContactName.EndsWith(c.ContactName)));
//		}

//		[TestMethod]
//		public void TestStringContainsLiteral()
//		{
//			TestQuery(
//				"TestStringContainsLiteral",
//				db.Customers.Where(c => c.ContactName.Contains("and")));
//		}

//		[TestMethod]
//		public void TestStringContainsColumn()
//		{
//			TestQuery(
//				"TestStringContainsColumn",
//				db.Customers.Where(c => c.ContactName.Contains(c.ContactName)));
//		}

//		[TestMethod]
//		public void TestStringConcatImplicit2Args()
//		{
//			TestQuery(
//				"TestStringConcatImplicit2Args",
//				db.Customers.Where(c => c.ContactName + "X" == "X"));
//		}

//		[TestMethod]
//		public void TestStringConcatExplicit2Args()
//		{
//			TestQuery(
//				"TestStringConcatExplicit2Args",
//				db.Customers.Where(c => string.Concat(c.ContactName, "X") == "X"));
//		}

//		[TestMethod]
//		public void TestStringConcatExplicit3Args()
//		{
//			TestQuery(
//				"TestStringConcatExplicit3Args",
//				db.Customers.Where(c => string.Concat(c.ContactName, "X", c.Country) == "X"));
//		}

//		[TestMethod]
//		public void TestStringConcatExplicitNArgs()
//		{
//			TestQuery(
//				"TestStringConcatExplicitNArgs",
//				db.Customers.Where(c => string.Concat(new string[] { c.ContactName, "X", c.Country }) == "X"));
//		}

//		[TestMethod]
//		public void TestStringIsNullOrEmpty()
//		{
//			TestQuery(
//				"TestStringIsNullOrEmpty",
//				db.Customers.Where(c => string.IsNullOrEmpty(c.City)));
//		}

//		[TestMethod]
//		public void TestStringToUpper()
//		{
//			TestQuery(
//				"TestStringToUpper",
//				db.Customers.Where(c => c.City.ToUpper() == "SEATTLE"));
//		}

//		[TestMethod]
//		public void TestStringToLower()
//		{
//			TestQuery(
//				"TestStringToLower",
//				db.Customers.Where(c => c.City.ToLower() == "seattle"));
//		}

//		[TestMethod]
//		public void TestStringSubstring()
//		{
//			TestQuery(
//				"TestStringSubstring",
//				db.Customers.Where(c => c.City.Substring(0, 4) == "Seat"));
//		}

//		[TestMethod]
//		public void TestStringSubstringNoLength()
//		{
//			TestQuery(
//				"TestStringSubstringNoLength",
//				db.Customers.Where(c => c.City.Substring(4) == "tle"));
//		}

//		[TestMethod]
//		public void TestStringIndexOf()
//		{
//			TestQuery(
//				"TestStringIndexOf",
//				db.Customers.Where(c => c.City.IndexOf("tt") == 4));
//		}

//		[TestMethod]
//		public void TestStringIndexOfChar()
//		{
//			TestQuery(
//				"TestStringIndexOfChar",
//				db.Customers.Where(c => c.City.IndexOf('t') == 4));
//		}

//		[TestMethod]
//		public void TestStringReplace()
//		{
//			TestQuery(
//				"TestStringReplace",
//				db.Customers.Where(c => c.City.Replace("ea", "ae") == "Saettle"));
//		}

//		[TestMethod]
//		public void TestStringReplaceChars()
//		{
//			TestQuery(
//				"TestStringReplaceChars",
//				db.Customers.Where(c => c.City.Replace("e", "y") == "Syattly"));
//		}

//		[TestMethod]
//		public void TestStringTrim()
//		{
//			TestQuery(
//				"TestStringTrim",
//				db.Customers.Where(c => c.City.Trim() == "Seattle"));
//		}

//		[TestMethod]
//		public void TestStringToString()
//		{
//			TestQuery(
//				"TestStringToString",
//				db.Customers.Where(c => c.City.ToString() == "Seattle"));
//		}

//		[TestMethod]
//		public void TestStringRemove()
//		{
//			TestQuery(
//				"TestStringRemove",
//				db.Customers.Where(c => c.City.Remove(1, 2) == "Sttle"));
//		}

//		[TestMethod]
//		public void TestStringRemoveNoCount()
//		{
//			TestQuery(
//				"TestStringRemoveNoCount",
//				db.Customers.Where(c => c.City.Remove(4) == "Seat"));
//		}
		
//		[TestMethod]
//		public void TestDateTimeConstructYmd()
//		{
//			TestQuery(
//				"TestDateTimeConstructYmd",
//				db.Orders.Where(o => o.OrderDate == new DateTime(o.OrderDate.Year, 1, 1)));
//		}

//		[TestMethod]
//		public void TestDateTimeConstructYmdhms()
//		{
//			TestQuery(
//				"TestDateTimeConstructYmdhms",
//				db.Orders.Where(o => o.OrderDate == new DateTime(o.OrderDate.Year, 1, 1, 10, 25, 55)));
//		}

//		[TestMethod]
//		public void TestDateTimeDay()
//		{
//			TestQuery(
//				"TestDateTimeDay",
//				db.Orders.Where(o => o.OrderDate.Day == 5));
//		}

//		[TestMethod]
//		public void TestDateTimeMonth()
//		{
//			TestQuery(
//				"TestDateTimeMonth",
//				db.Orders.Where(o => o.OrderDate.Month == 12));
//		}

//		[TestMethod]
//		public void TestDateTimeYear()
//		{
//			TestQuery(
//				"TestDateTimeYear",
//				db.Orders.Where(o => o.OrderDate.Year == 1997));
//		}

//		[TestMethod]
//		public void TestDateTimeHour()
//		{
//			TestQuery(
//				"TestDateTimeHour",
//				db.Orders.Where(o => o.OrderDate.Hour == 6));
//		}

//		[TestMethod]
//		public void TestDateTimeMinute()
//		{
//			TestQuery(
//				"TestDateTimeMinute",
//				db.Orders.Where(o => o.OrderDate.Minute == 32));
//		}

//		[TestMethod]
//		public void TestDateTimeSecond()
//		{
//			TestQuery(
//				"TestDateTimeSecond",
//				db.Orders.Where(o => o.OrderDate.Second == 47));
//		}

//		[TestMethod]
//		public void TestDateTimeMillisecond()
//		{
//			TestQuery(
//				"TestDateTimeMillisecond",
//				db.Orders.Where(o => o.OrderDate.Millisecond == 200));
//		}

//		[TestMethod]
//		public void TestDateTimeDayOfWeek()
//		{
//			TestQuery(
//				"TestDateTimeDayOfWeek",
//				db.Orders.Where(o => o.OrderDate.DayOfWeek == DayOfWeek.Friday));
//		}

//		[TestMethod]
//		public void TestDateTimeDayOfYear()
//		{
//			TestQuery(
//				"TestDateTimeDayOfYear",
//				db.Orders.Where(o => o.OrderDate.DayOfYear == 360));
//		}

//		[TestMethod]
//		public void TestMathAbs()
//		{
//			TestQuery(
//				"TestMathAbs",
//				db.Orders.Where(o => Math.Abs(o.OrderID) == 10));
//		}

//		[TestMethod]
//		public void TestMathAcos()
//		{
//			// Math functions are not supported in SQLite
//			if (db.Configuration.DataAccessProvider.GetType().Name.Contains("SQLite"))
//			{
//				return;
//			}

//			TestQuery(
//				"TestMathAcos",
//				db.Orders.Where(o => Math.Acos(o.OrderID) == 0));
//		}

//		[TestMethod]
//		public void TestMathAsin()
//		{
//			// Math functions are not supported in SQLite
//			if (db.Configuration.DataAccessProvider.GetType().Name.Contains("SQLite"))
//			{
//				return;
//			}

//			TestQuery(
//				"TestMathAsin",
//				db.Orders.Where(o => Math.Asin(o.OrderID) == 0));
//		}

//		[TestMethod]
//		public void TestMathAtan()
//		{
//			// Math functions are not supported in SQLite
//			if (db.Configuration.DataAccessProvider.GetType().Name.Contains("SQLite"))
//			{
//				return;
//			}

//			TestQuery(
//				"TestMathAtan",
//				db.Orders.Where(o => Math.Atan(o.OrderID) == 0));
//		}

//		[TestMethod]
//		public void TestMathAtan2()
//		{
//			// Math functions are not supported in SQLite
//			if (db.Configuration.DataAccessProvider.GetType().Name.Contains("SQLite"))
//			{
//				return;
//			}

//			TestQuery(
//				"TestMathAtan2",
//				db.Orders.Where(o => Math.Atan2(o.OrderID, 3) == 0));
//		}

//		[TestMethod]
//		public void TestMathCos()
//		{
//			// Math functions are not supported in SQLite
//			if (db.Configuration.DataAccessProvider.GetType().Name.Contains("SQLite"))
//			{
//				return;
//			}

//			TestQuery(
//				"TestMathCos",
//				db.Orders.Where(o => Math.Cos(o.OrderID) == 0));
//		}

//		[TestMethod]
//		public void TestMathSin()
//		{
//			// Math functions are not supported in SQLite
//			if (db.Configuration.DataAccessProvider.GetType().Name.Contains("SQLite"))
//			{
//				return;
//			}

//			TestQuery(
//				"TestMathSin",
//				db.Orders.Where(o => Math.Sin(o.OrderID) == 0));
//		}

//		[TestMethod]
//		public void TestMathTan()
//		{
//			// Math functions are not supported in SQLite
//			if (db.Configuration.DataAccessProvider.GetType().Name.Contains("SQLite"))
//			{
//				return;
//			}

//			TestQuery(
//				"TestMathTan",
//				db.Orders.Where(o => Math.Tan(o.OrderID) == 0));
//		}

//		[TestMethod]
//		public void TestMathExp()
//		{
//			// Math functions are not supported in SQLite
//			if (db.Configuration.DataAccessProvider.GetType().Name.Contains("SQLite"))
//			{
//				return;
//			}

//			TestQuery(
//				"TestMathExp",
//				db.Orders.Where(o => Math.Exp(o.OrderID) == 0));
//		}

//		[TestMethod]
//		public void TestMathLog()
//		{
//			// Math functions are not supported in SQLite
//			if (db.Configuration.DataAccessProvider.GetType().Name.Contains("SQLite"))
//			{
//				return;
//			}

//			TestQuery(
//				"TestMathLog",
//				db.Orders.Where(o => Math.Log(o.OrderID) == 0));
//		}

//		[TestMethod]
//		public void TestMathLog10()
//		{
//			// Math functions are not supported in SQLite
//			if (db.Configuration.DataAccessProvider.GetType().Name.Contains("SQLite"))
//			{
//				return;
//			}

//			TestQuery(
//				"TestMathLog10",
//				db.Orders.Where(o => Math.Log10(o.OrderID) == 0));
//		}

//		[TestMethod]
//		public void TestMathSqrt()
//		{
//			// Math functions are not supported in SQLite
//			if (db.Configuration.DataAccessProvider.GetType().Name.Contains("SQLite"))
//			{
//				return;
//			}

//			TestQuery(
//				"TestMathSqrt",
//				db.Orders.Where(o => Math.Sqrt(o.OrderID) == 0));
//		}

//		[TestMethod]
//		public void TestMathCeiling()
//		{
//			TestQuery(
//				"TestMathCeiling",
//				db.Orders.Where(o => Math.Ceiling((double)o.OrderID) == 0));
//		}

//		[TestMethod]
//		public void TestMathFloor()
//		{
//			TestQuery(
//				"TestMathFloor",
//				db.Orders.Where(o => Math.Floor((double)o.OrderID) == 0));
//		}

//		[TestMethod]
//		public void TestMathPow()
//		{
//			// Math functions are not supported in SQLite
//			if (db.Configuration.DataAccessProvider.GetType().Name.Contains("SQLite"))
//			{
//				return;
//			}

//			TestQuery(
//				"TestMathPow",
//				db.Orders.Where(o => Math.Pow(o.OrderID < 1000 ? 1 : 2, 3) == 0));
//		}

//		[TestMethod]
//		public void TestMathRoundDefault()
//		{
//			TestQuery(
//				"TestMathRoundDefault",
//				db.Orders.Where(o => Math.Round((decimal)o.OrderID) == 0));
//		}

//		[TestMethod]
//		public void TestMathRoundToPlace()
//		{
//			TestQuery(
//				"TestMathRoundToPlace",
//				db.Orders.Where(o => Math.Round((decimal)o.OrderID, 2) == 0));
//		}

//		[TestMethod]
//		public void TestMathTruncate()
//		{
//			TestQuery(
//				"TestMathTruncate",
//				db.Orders.Where(o => Math.Truncate((double)o.OrderID) == 0));
//		}

//		[TestMethod]
//		public void TestStringCompareToLessThan()
//		{
//			TestQuery(
//				"TestStringCompareToLessThan",
//				db.Customers.Where(c => c.City.CompareTo("Seattle") < 0));
//		}

//		[TestMethod]
//		public void TestStringCompareToLessThanOrEqualTo()
//		{
//			TestQuery(
//				"TestStringCompareToLessThanOrEqualTo",
//				db.Customers.Where(c => c.City.CompareTo("Seattle") <= 0));
//		}

//		[TestMethod]
//		public void TestStringCompareToGreaterThan()
//		{
//			TestQuery(
//				"TestStringCompareToGreaterThan",
//				db.Customers.Where(c => c.City.CompareTo("Seattle") > 0));
//		}

//		[TestMethod]
//		public void TestStringCompareToGreaterThanOrEqualTo()
//		{
//			TestQuery(
//				"TestStringCompareToGreaterThanOrEqualTo",
//				db.Customers.Where(c => c.City.CompareTo("Seattle") >= 0));
//		}

//		[TestMethod]
//		public void TestStringCompareToEquals()
//		{
//			TestQuery(
//				"TestStringCompareToEquals",
//				db.Customers.Where(c => c.City.CompareTo("Seattle") == 0));
//		}

//		[TestMethod]
//		public void TestStringCompareToNotEquals()
//		{
//			TestQuery(
//				"TestStringCompareToNotEquals",
//				db.Customers.Where(c => c.City.CompareTo("Seattle") != 0));
//		}

//		[TestMethod]
//		public void TestStringCompareLessThan()
//		{
//			TestQuery(
//				"TestStringCompareLessThan",
//				db.Customers.Where(c => string.Compare(c.City, "Seattle") < 0));
//		}

//		[TestMethod]
//		public void TestStringCompareLessThanOrEqualTo()
//		{
//			TestQuery(
//				"TestStringCompareLessThanOrEqualTo",
//				db.Customers.Where(c => string.Compare(c.City, "Seattle") <= 0));
//		}

//		[TestMethod]
//		public void TestStringCompareGreaterThan()
//		{
//			TestQuery(
//				"TestStringCompareGreaterThan",
//				db.Customers.Where(c => string.Compare(c.City, "Seattle") > 0));
//		}

//		[TestMethod]
//		public void TestStringCompareGreaterThanOrEqualTo()
//		{
//			TestQuery(
//				"TestStringCompareGreaterThanOrEqualTo",
//				db.Customers.Where(c => string.Compare(c.City, "Seattle") >= 0));
//		}

//		[TestMethod]
//		public void TestStringCompareEquals()
//		{
//			TestQuery(
//				"TestStringCompareEquals",
//				db.Customers.Where(c => string.Compare(c.City, "Seattle") == 0));
//		}

//		[TestMethod]
//		public void TestStringCompareNotEquals()
//		{
//			TestQuery(
//				"TestStringCompareNotEquals",
//				db.Customers.Where(c => string.Compare(c.City, "Seattle") != 0));
//		}

//		[TestMethod]
//		public void TestIntCompareTo()
//		{
//			TestQuery(
//				"TestIntCompareTo",
//				db.Orders.Where(o => o.OrderID.CompareTo(1000) == 0));
//		}

//		[TestMethod]
//		public void TestDecimalCompare()
//		{
//			TestQuery(
//				"TestDecimalCompare",
//				db.Orders.Where(o => decimal.Compare((decimal)o.OrderID, 0.0m) == 0));
//		}

//		[TestMethod]
//		public void TestDecimalAdd()
//		{
//			TestQuery(
//				"TestDecimalAdd",
//				db.Orders.Where(o => decimal.Add(o.OrderID, 0.0m) == 0.0m));
//		}

//		[TestMethod]
//		public void TestDecimalSubtract()
//		{
//			TestQuery(
//				"TestDecimalSubtract",
//				db.Orders.Where(o => decimal.Subtract(o.OrderID, 0.0m) == 0.0m));
//		}

//		[TestMethod]
//		public void TestDecimalMultiply()
//		{
//			TestQuery(
//				"TestDecimalMultiply",
//				db.Orders.Where(o => decimal.Multiply(o.OrderID, 1.0m) == 1.0m));
//		}

//		[TestMethod]
//		public void TestDecimalDivide()
//		{
//			TestQuery(
//				"TestDecimalDivide",
//				db.Orders.Where(o => decimal.Divide(o.OrderID, 1.0m) == 1.0m));
//		}

//		[TestMethod]
//		public void TestDecimalRemainder()
//		{
//			TestQuery(
//				"TestDecimalRemainder",
//				db.Orders.Where(o => decimal.Remainder(o.OrderID, 1.0m) == 0.0m));
//		}

//		[TestMethod]
//		public void TestDecimalNegate()
//		{
//			TestQuery(
//				"TestDecimalNegate",
//				db.Orders.Where(o => decimal.Negate(o.OrderID) == 1.0m));
//		}

//		[TestMethod]
//		public void TestDecimalCeiling()
//		{
//			TestQuery(
//				"TestDecimalCeiling",
//				db.Orders.Where(o => decimal.Ceiling(o.OrderID) == 0.0m));
//		}

//		[TestMethod]
//		public void TestDecimalFloor()
//		{
//			TestQuery(
//				"TestDecimalFloor",
//				db.Orders.Where(o => decimal.Floor(o.OrderID) == 0.0m));
//		}

//		[TestMethod]
//		public void TestDecimalRoundDefault()
//		{
//			TestQuery(
//				"TestDecimalRoundDefault",
//				db.Orders.Where(o => decimal.Round(o.OrderID) == 0m));
//		}

//		[TestMethod]
//		public void TestDecimalRoundPlaces()
//		{
//			TestQuery(
//				"TestDecimalRoundPlaces",
//				db.Orders.Where(o => decimal.Round(o.OrderID, 2) == 0.00m));
//		}

//		[TestMethod]
//		public void TestDecimalTruncate()
//		{
//			TestQuery(
//				"TestDecimalTruncate",
//				db.Orders.Where(o => decimal.Truncate(o.OrderID) == 0m));
//		}

//		[TestMethod]
//		public void TestDecimalLessThan()
//		{
//			TestQuery(
//				"TestDecimalLessThan",
//				db.Orders.Where(o => ((decimal)o.OrderID) < 0.0m));
//		}

//		[TestMethod]
//		public void TestIntLessThan()
//		{
//			TestQuery(
//				"TestIntLessThan",
//				db.Orders.Where(o => o.OrderID < 0));
//		}

//		[TestMethod]
//		public void TestIntLessThanOrEqual()
//		{
//			TestQuery(
//				"TestIntLessThanOrEqual",
//				db.Orders.Where(o => o.OrderID <= 0));
//		}

//		[TestMethod]
//		public void TestIntGreaterThan()
//		{
//			TestQuery(
//				"TestIntGreaterThan",
//				db.Orders.Where(o => o.OrderID > 0));
//		}

//		[TestMethod]
//		public void TestIntGreaterThanOrEqual()
//		{
//			TestQuery(
//				"TestIntGreaterThanOrEqual",
//				db.Orders.Where(o => o.OrderID >= 0));
//		}

//		[TestMethod]
//		public void TestIntEqual()
//		{
//			TestQuery(
//				"TestIntEqual",
//				db.Orders.Where(o => o.OrderID == 0));
//		}

//		[TestMethod]
//		public void TestIntNotEqual()
//		{
//			TestQuery(
//				"TestIntNotEqual",
//				db.Orders.Where(o => o.OrderID != 0));
//		}

//		[TestMethod]
//		public void TestIntAdd()
//		{
//			TestQuery(
//				"TestIntAdd",
//				db.Orders.Where(o => o.OrderID + 0 == 0));
//		}

//		[TestMethod]
//		public void TestIntSubtract()
//		{
//			TestQuery(
//				"TestIntSubtract",
//				db.Orders.Where(o => o.OrderID - 0 == 0));
//		}

//		[TestMethod]
//		public void TestIntMultiply()
//		{
//			TestQuery(
//				"TestIntMultiply",
//				db.Orders.Where(o => o.OrderID * 1 == 1));
//		}

//		[TestMethod]
//		public void TestIntDivide()
//		{
//			TestQuery(
//				"TestIntDivide",
//				db.Orders.Where(o => o.OrderID / 1 == 1));
//		}

//		[TestMethod]
//		public void TestIntModulo()
//		{
//			TestQuery(
//				"TestIntModulo",
//				db.Orders.Where(o => o.OrderID % 1 == 0));
//		}

//		[TestMethod]
//		public void TestIntLeftShift()
//		{
//			TestQuery(
//				"TestIntLeftShift",
//				db.Orders.Where(o => o.OrderID << 1 == 0));
//		}

//		[TestMethod]
//		public void TestIntRightShift()
//		{
//			TestQuery(
//				"TestIntRightShift",
//				db.Orders.Where(o => o.OrderID >> 1 == 0));
//		}

//		[TestMethod]
//		public void TestIntBitwiseAnd()
//		{
//			TestQuery(
//				"TestIntBitwiseAnd",
//				db.Orders.Where(o => (o.OrderID & 1) == 0));
//		}

//		[TestMethod]
//		public void TestIntBitwiseOr()
//		{
//			TestQuery(
//				"TestIntBitwiseOr",
//				db.Orders.Where(o => (o.OrderID | 1) == 1));
//		}

//		[TestMethod]
//		public void TestIntBitwiseExclusiveOr()
//		{
//			TestQuery(
//				"TestIntBitwiseExclusiveOr",
//				db.Orders.Where(o => (o.OrderID ^ 1) == 1));
//		}

//		////[TestMethod]
//		////public void TestIntBitwiseNot()
//		////{
//		////	TestQuery(
//		////		"TestIntBitwiseNot",
//		////		db.Orders.Where(o => ~o.OrderID == 0));
//		////}

//		[TestMethod]
//		public void TestIntNegate()
//		{
//			TestQuery(
//				"TestIntNegate",
//				db.Orders.Where(o => -o.OrderID == -1));
//		}

//		[TestMethod]
//		public void TestAnd()
//		{
//			TestQuery(
//				"TestAnd",
//				db.Orders.Where(o => o.OrderID > 0 && o.OrderID < 2000));
//		}

//		[TestMethod]
//		public void TestOr()
//		{
//			TestQuery(
//				"TestOr",
//				db.Orders.Where(o => o.OrderID < 5 || o.OrderID > 10));
//		}

//		[TestMethod]
//		public void TestNot()
//		{
//			TestQuery(
//				"TestNot",
//				db.Orders.Where(o => !(o.OrderID == 0)));
//		}

//		[TestMethod]
//		public void TestEqualNull()
//		{
//			TestQuery(
//				"TestEqualNull",
//				db.Customers.Where(c => c.City == null));
//		}

//		[TestMethod]
//		public void TestEqualNullReverse()
//		{
//			TestQuery(
//				"TestEqualNullReverse",
//				db.Customers.Where(c => null == c.City));
//		}

//		[TestMethod]
//		public void TestConditional()
//		{
//			TestQuery(
//				"TestConditional",
//				db.Orders.Where(o => (o.CustomerID == "ALFKI" ? 1000 : 0) == 1000));
//		}

//		[TestMethod]
//		public void TestConditional2()
//		{
//			TestQuery(
//				"TestConditional2",
//				db.Orders.Where(o => (o.CustomerID == "ALFKI" ? 1000 : o.CustomerID == "ABCDE" ? 2000 : 0) == 1000));
//		}

//		[TestMethod]
//		public void TestConditionalTestIsValue()
//		{
//			TestQuery(
//				"TestConditionalTestIsValue",
//				db.Orders.Where(o => (((bool)(object)o.OrderID) ? 100 : 200) == 100));
//		}

//		////[TestMethod]
//		////public void TestConditionalResultsArePredicates()
//		////{
//		////	TestQuery(
//		////		"TestConditionalResultsArePredicates",
//		////		db.Orders.Where(o => (o.CustomerID == "ALFKI" ? o.OrderID < 10 : o.OrderID > 10)));
//		////}

//		////[TestMethod]
//		////public void TestSelectManyJoined()
//		////{
//		////	TestQuery(
//		////		"TestSelectManyJoined",
//		////		from c in db.Customers
//		////		from o in db.Orders.Where(o => o.CustomerID == c.CustomerID)
//		////		select new { c.ContactName, o.OrderDate });
//		////}

//		////[TestMethod]
//		////public void TestSelectManyJoinedDefaultIfEmpty()
//		////{
//		////	TestQuery(
//		////		"TestSelectManyJoinedDefaultIfEmpty",
//		////		from c in db.Customers
//		////		from o in db.Orders.Where(o => o.CustomerID == c.CustomerID).DefaultIfEmpty()
//		////		select new { c.ContactName, o.OrderDate });
//		////}

//		////[TestMethod]
//		////public void TestSelectWhereAssociation()
//		////{
//		////	TestQuery(
//		////		"TestSelectWhereAssociation",
//		////		from o in db.Orders
//		////		where o.Customer.City == "Seattle"
//		////		select o);
//		////}

//		////[TestMethod]
//		////public void TestSelectWhereAssociations()
//		////{
//		////	TestQuery(
//		////		"TestSelectWhereAssociations",
//		////		from o in db.Orders
//		////		where o.Customer.City == "Seattle" && o.Customer.Phone != "555 555 5555"
//		////		select o);
//		////}

//		////[TestMethod]
//		////public void TestSelectWhereAssociationTwice()
//		////{
//		////	TestQuery(
//		////		"TestSelectWhereAssociationTwice",
//		////		from o in db.Orders
//		////		where o.Customer.City == "Seattle" && o.Customer.Phone != "555 555 5555"
//		////		select o);
//		////}

//		////[TestMethod]
//		////public void TestSelectAssociation()
//		////{
//		////	TestQuery(
//		////		"TestSelectAssociation",
//		////		from o in db.Orders
//		////		select o.Customer);
//		////}

//		////[TestMethod]
//		////public void TestSelectAssociations()
//		////{
//		////	TestQuery(
//		////		"TestSelectAssociations",
//		////		from o in db.Orders
//		////		select new { A = o.Customer, B = o.Customer });
//		////}

//		////[TestMethod]
//		////public void TestSelectAssociationsWhereAssociations()
//		////{
//		////	TestQuery(
//		////		"TestSelectAssociationsWhereAssociations",
//		////		from o in db.Orders
//		////		where o.Customer.City == "Seattle"
//		////		where o.Customer.Phone != "555 555 5555"
//		////		select new { A = o.Customer, B = o.Customer });
//		////}

//		////[TestMethod]
//		////public void TestSingletonAssociationWithMemberAccess()
//		////{
//		////	TestQuery(
//		////		"TestSingletonAssociationWithMemberAccess",
//		////		from o in db.Orders
//		////		where o.Customer.City == "Seattle"
//		////		where o.Customer.Phone != "555 555 5555"
//		////		select new { A = o.Customer, B = o.Customer.City }
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestCompareDateTimesWithDifferentNullability()
//		////{
//		////	DateTime today = new DateTime(2013, 1, 1);
//		////	TestQuery(
//		////		"TestCompareDateTimesWithDifferentNullability",
//		////		from o in db.Orders
//		////		where o.OrderDate < today && ((DateTime?)o.OrderDate) < today
//		////		select o
//		////		);
//		////}

//		[TestMethod]
//		public void TestContainsWithEmptyLocalList()
//		{
//			var ids = new string[0];
//			TestQuery(
//				"TestContainsWithEmptyLocalList",
//				from c in db.Customers
//				where ids.Contains(c.CustomerID)
//				select c
//				);
//		}

//		[TestMethod]
//		public void TestContainsWithSubquery2()
//		{
//			var custsInLondon = db.Customers.Where(c => c.City == "London").Select(c => c.CustomerID);

//			TestQuery(
//				"TestContainsWithSubquery2",
//				from c in db.Customers
//				where custsInLondon.Contains(c.CustomerID)
//				select c
//				);
//		}

//		////[TestMethod]
//		////public void TestCombineQueriesDeepNesting()
//		////{
//		////	var custs = db.Customers.Where(c => c.ContactName.StartsWith("xxx"));
//		////	var ords = db.Orders.Where(o => custs.Any(c => c.CustomerID == o.CustomerID));
//		////	TestQuery(
//		////		"TestCombineQueriesDeepNesting",
//		////		db.OrderDetails.Where(d => ords.Any(o => o.OrderID == d.OrderID))
//		////		);
//		////}

//		////[TestMethod]
//		////public void TestLetWithSubquery()
//		////{
//		////	TestQuery(
//		////		"TestLetWithSubquery",
//		////		from customer in db.Customers
//		////		let orders =
//		////			from order in db.Orders
//		////			where order.CustomerID == customer.CustomerID
//		////			select order
//		////		select new
//		////		{
//		////			Customer = customer,
//		////			OrdersCount = orders.Count(),
//		////		}
//		////		);
//		////}

//		protected void TestQuery(string baseline, IQueryable query)
//		{
//			TestQuery(baseline, query.Expression);
//		}

//		protected void TestQuery(string baseline, Expression<Func<object>> query)
//		{
//			TestQuery(baseline, query.Body);
//		}

//		private void TestQuery(string baseline, Expression query)
//		{
//			var select = db.BuildSelectStatement(query);

//			// Test the SQLite command builder
//			var sqliteProvider = new SQLite.SQLiteDataAccessProvider();
//			var sqliteCommand = sqliteProvider.BuildCommand(select, db.Configuration);
//			TestQuery(query, sqliteBaselines[baseline], sqliteCommand, "*** SQLITE ***");

//			// Test the SQL Server command builder
//			var sqlServerProvider = new SqlServer.SqlServerDataAccessProvider();
//			var sqlServerCommand = sqlServerProvider.BuildCommand(select, db.Configuration);
//			TestQuery(query, sqlServerBaselines[baseline], sqlServerCommand, "*** SQL SERVER ***");
//		}

//		private void TestQuery(Expression query, string baseline, DbCommand command, string provider)
//		{
//			if (query.NodeType == ExpressionType.Convert && query.Type == typeof(object))
//			{
//				// Remove boxing
//				query = ((UnaryExpression)query).Operand;
//			}

//			string expected = TrimExtraWhiteSpace(baseline.Replace("\n\n", ") ("));
//			string actual = TrimExtraWhiteSpace(command.CommandText.ToString());

//			// Replace parameter references with their values so that we can check they have the correct value
//			for (int i = 0; i < command.Parameters.Count; i++)
//			{
//				Assert.IsTrue(actual.Contains("@" + i));
//				if (command.Parameters[i].Value is string ||
//					command.Parameters[i].Value is char)
//				{
//					actual = actual.Replace("@" + i, "'" + command.Parameters[i].Value.ToString() + "'");
//				}
//				else
//				{
//					actual = actual.Replace("@" + i, command.Parameters[i].Value.ToString());
//				}
//			}

//			Assert.AreEqual(expected, actual, provider);
//		}

//		private string TrimExtraWhiteSpace(string s)
//		{
//			string result = s.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Trim();
//			while (result.Contains("  "))
//			{
//				result = result.Replace("  ", " ");
//			}
//			result = result.Replace("( ", "(").Replace(" )", ")");
//			return result;
//		}
//	}
//}
