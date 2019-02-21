using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Watsonia.QueryBuilder;
using Watsonia.QueryBuilder.Tests.Northwind;

// TODO: Implement all double commented (////) tests

namespace Watsonia.Data.Tests.Queries
{
	[TestClass]
	public class QueryLanguageTests
	{
		private class Thing
		{
			public IQueryable<Customer> Customers { get; internal set; }
			public IQueryable<Order> Orders { get; internal set; }
		}
		private Thing db = new Thing();

		private static DatabaseMapper mapper = new NorthwindMapper();
		private static Dictionary<string, string> sqlServerBaselines = new Dictionary<string, string>();
		private static Dictionary<string, string> sqliteBaselines = new Dictionary<string, string>();

		[ClassInitialize]
		public static void Initialize(TestContext context)
		{
			string sqlServerFileName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Baselines\SqlServer.xml";
			if (!string.IsNullOrEmpty(sqlServerFileName) && File.Exists(sqlServerFileName))
			{
				XDocument doc = XDocument.Load(sqlServerFileName);
				sqlServerBaselines = doc.Root.Elements("baseline").ToDictionary(e => (string)e.Attribute("key"), e => e.Value);
			}

			string sqliteFileName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Baselines\SQLite.xml";
			if (!string.IsNullOrEmpty(sqliteFileName) && File.Exists(sqliteFileName))
			{
				XDocument doc = XDocument.Load(sqliteFileName);
				sqliteBaselines = doc.Root.Elements("baseline").ToDictionary(e => (string)e.Attribute("key"), e => e.Value);
			}
		}

		[TestMethod]
		public void TestWhere()
		{
			TestQuery2(
				"TestWhere",
				Select.From<Customer>("c").Where(c => c.City == "London"));
		}

		////[TestMethod]
		////public void TestCompareEntityEqual()
		////{
		////	Customer alfki = new Customer { CustomerID = "ALFKI" };
		////	TestQuery2(
		////		"TestCompareEntityEqual",
		////		Select.From<Customer>("c").Where(c => c == alfki));
		////}

		////[TestMethod]
		////public void TestCompareEntityNotEqual()
		////{
		////	Customer alfki = new Customer { CustomerID = "ALFKI" };
		////	TestQuery2(
		////		"TestCompareEntityNotEqual",
		////		Select.From<Customer>("c").Where(c => c != alfki));
		////}

		[TestMethod]
		public void TestCompareConstructedEqual()
		{
			TestQuery2(
				"TestCompareConstructedEqual",
				Select.From<Customer>("c").Where(c => new { x = c.City } == new { x = "London" }));
		}

		////[TestMethod]
		////public void TestCompareConstructedMultiValueEqual()
		////{
		////	TestQuery2(
		////		"TestCompareConstructedMultiValueEqual",
		////		Select.From<Customer>("c").Where(c => new { x = c.City, y = c.Country } == new { x = "London", y = "UK" }));
		////}

		////[TestMethod]
		////public void TestCompareConstructedMultiValueNotEqual()
		////{
		////	TestQuery2(
		////		"TestCompareConstructedMultiValueNotEqual",
		////		Select.From<Customer>("c").Where(c => new { x = c.City, y = c.Country } != new { x = "London", y = "UK" }));
		////}

		[TestMethod]
		public void TestCompareConstructed()
		{
			TestQuery2(
				"TestCompareConstructed",
				Select.From<Customer>("c").Where(c => new { x = c.City } == new { x = "London" }));
		}

		[TestMethod]
		public void TestSelectScalar()
		{
			TestQuery2(
				"TestSelectScalar",
				Select.From<Customer>("c").Columns(c => c.City));
		}

		////[TestMethod]
		////public void TestSelectAnonymousOne()
		////{
		////	TestQuery2(
		////		"TestSelectAnonymousOne",
		////		Select.From<Customer>("c").Columns(c => new { c.City }));
		////}

		////[TestMethod]
		////public void TestSelectAnonymousTwo()
		////{
		////	TestQuery2(
		////		"TestSelectAnonymousTwo",
		////		Select.From<Customer>("c").Columns(c => new { c.City, c.Phone }));
		////}

		////[TestMethod]
		////public void TestSelectAnonymousThree()
		////{
		////	TestQuery2(
		////		"TestSelectAnonymousThree",
		////		Select.From<Customer>("c").Columns(c => new { c.City, c.Phone, c.Country }));
		////}

		////[TestMethod]
		////public void TestSelectCustomerTable()
		////{
		////	TestQuery2(
		////		"TestSelectCustomerTable",
		////		Select.From<Customer>("c"));
		////}

		////[TestMethod]
		////public void TestSelectCustomerIdentity()
		////{
		////	TestQuery2(
		////		"TestSelectCustomerIdentity",
		////		Select.From<Customer>("c").Columns(c => c));
		////}

		////[TestMethod]
		////public void TestSelectAnonymousWithObject()
		////{
		////	TestQuery2(
		////		"TestSelectAnonymousWithObject",
		////		Select.From<Customer>("c").Columns(c => new { c.City, c }));
		////}

		////[TestMethod]
		////public void TestSelectAnonymousNested()
		////{
		////	TestQuery2(
		////		"TestSelectAnonymousNested",
		////		Select.From<Customer>("c").Columns(c => new { c.City, Country = new { c.Country } }));
		////}

		////[TestMethod]
		////public void TestSelectAnonymousEmpty()
		////{
		////	TestQuery2(
		////		"TestSelectAnonymousEmpty",
		////		Select.From<Customer>("c").Columns(c => new { }));
		////}

		////[TestMethod]
		////public void TestSelectAnonymousLiteral()
		////{
		////	TestQuery2(
		////		"TestSelectAnonymousLiteral",
		////		Select.From<Customer>("c").Columns(c => new { X = 10 }));
		////}

		////[TestMethod]
		////public void TestSelectConstantInt()
		////{
		////	TestQuery2(
		////		"TestSelectConstantInt",
		////		Select.From<Customer>("c").Columns(c => 0));
		////}

		////[TestMethod]
		////public void TestSelectConstantNullString()
		////{
		////	TestQuery2(
		////		"TestSelectConstantNullString",
		////		Select.From<Customer>("c").Columns(c => (string)null));
		////}

		////[TestMethod]
		////public void TestSelectLocal()
		////{
		////	int x = 10;
		////	TestQuery2(
		////		"TestSelectLocal",
		////		Select.From<Customer>("c").Columns(c => x));
		////}

		////[TestMethod]
		////public void TestSelectNestedCollection()
		////{
		////	TestQuery2(
		////		"TestSelectNestedCollection",
		////		from c in Select.From<Customer>("c")
		////		where c.City == "London"
		////		select Select.From<Order>("o").Where(o => o.CustomerID == c.CustomerID && o.OrderDate.Year == 1997).Columns(o => o.OrderID));
		////}

		////[TestMethod]
		////public void TestSelectNestedCollectionInAnonymousType()
		////{
		////	TestQuery2(
		////		"TestSelectNestedCollectionInAnonymousType",
		////		from c in Select.From<Customer>("c")
		////		where c.CustomerID == "ALFKI"
		////		select new { Foos = Select.From<Order>("o").Where(o => o.CustomerID == c.CustomerID && o.OrderDate.Year == 1997).Columns(o => o.OrderID) });
		////}

		////[TestMethod]
		////public void TestJoinCustomerOrders()
		////{
		////	TestQuery2(
		////		"TestJoinCustomerOrders",
		////		from c in Select.From<Customer>("c")
		////		join o in Select.From<Order>("o") on c.CustomerID equals o.CustomerID
		////		select new { c.ContactName, o.OrderID });
		////}

		////[TestMethod]
		////public void TestJoinMultiKey()
		////{
		////	TestQuery2(
		////		"TestJoinMultiKey",
		////		from c in Select.From<Customer>("c")
		////		join o in Select.From<Order>("o") on new { a = c.CustomerID, b = c.CustomerID } equals new { a = o.CustomerID, b = o.CustomerID }
		////		select new { c, o });
		////}

		////[TestMethod]
		////public void TestJoinIntoCustomersOrders()
		////{
		////	TestQuery2(
		////		"TestJoinIntoCustomersOrders",
		////		from c in Select.From<Customer>("c")
		////		join o in Select.From<Order>("o") on c.CustomerID equals o.CustomerID into ords
		////		select new { cust = c, ords = ords.ToList() });
		////}

		////[TestMethod]
		////public void TestJoinIntoCustomersOrdersCount()
		////{
		////	TestQuery2(
		////		"TestJoinIntoCustomersOrdersCount",
		////		from c in Select.From<Customer>("c")
		////		join o in Select.From<Order>("o") on c.CustomerID equals o.CustomerID into ords
		////		select new { cust = c, ords = ords.Count() });
		////}

		////[TestMethod]
		////public void TestJoinIntoDefaultIfEmpty()
		////{
		////	TestQuery2(
		////		"TestJoinIntoDefaultIfEmpty",
		////		from c in Select.From<Customer>("c")
		////		join o in Select.From<Order>("o") on c.CustomerID equals o.CustomerID into ords
		////		from o in ords.DefaultIfEmpty()
		////		select new { c, o });
		////}

		////[TestMethod]
		////public void TestSelectManyCustomerOrders()
		////{
		////	TestQuery2(
		////		"TestSelectManyCustomerOrders",
		////		from c in Select.From<Customer>("c")
		////		from o in Select.From<Order>("o")
		////		where c.CustomerID == o.CustomerID
		////		select new { c.ContactName, o.OrderID }
		////		);
		////}

		////[TestMethod]
		////public void TestMultipleJoinsWithJoinConditionsInWhere()
		////{
		////	TestQuery2(
		////		"TestMultipleJoinsWithJoinConditionsInWhere",
		////		from c in Select.From<Customer>("c")
		////		from o in Select.From<Order>("o")
		////		from d in db.OrderDetails
		////		where o.CustomerID == c.CustomerID && o.OrderID == d.OrderID
		////		where c.CustomerID == "ALFKI"
		////		select d.ProductID
		////		);
		////}

		////[TestMethod]
		////public void TestMultipleJoinsWithMissingJoinCondition()
		////{
		////	TestQuery2(
		////		"TestMultipleJoinsWithMissingJoinCondition",
		////		from c in Select.From<Customer>("c")
		////		from o in Select.From<Order>("o")
		////		from d in db.OrderDetails
		////		where o.CustomerID == c.CustomerID /*&& o.OrderID == d.OrderID*/
		////		where c.CustomerID == "ALFKI"
		////		select d.ProductID
		////		);
		////}

		[TestMethod]
		public void TestOrderBy()
		{
			TestQuery2(
				"TestOrderBy",
				Select.From<Customer>("c").OrderBy(c => c.CustomerID)
				);
		}

		[TestMethod]
		public void TestOrderBySelect()
		{
			TestQuery2(
				"TestOrderBySelect",
				Select.From<Customer>("c").OrderBy(c => c.CustomerID).Columns(c => c.ContactName)
				);
		}

		[TestMethod]
		public void TestOrderByOrderBy()
		{
			TestQuery2(
				"TestOrderByOrderBy",
				Select.From<Customer>("c").OrderBy(c => c.CustomerID).OrderBy(c => c.Country).Columns(c => c.City)
				);
		}

		////[TestMethod]
		////public void TestOrderByThenBy()
		////{
		////	TestQuery2(
		////		"TestOrderByThenBy",
		////		Select.From<Customer>("c").OrderBy(c => c.CustomerID).ThenBy(c => c.Country).Columns(c => c.City)
		////		);
		////}

		[TestMethod]
		public void TestOrderByDescending()
		{
			TestQuery2(
				"TestOrderByDescending",
				Select.From<Customer>("c").OrderByDescending(c => c.CustomerID).Columns(c => c.City)
				);
		}

		////[TestMethod]
		////public void TestOrderByDescendingThenBy()
		////{
		////	TestQuery2(
		////		"TestOrderByDescendingThenBy",
		////		Select.From<Customer>("c").OrderByDescending(c => c.CustomerID).ThenBy(c => c.Country).Columns(c => c.City)
		////		);
		////}

		////[TestMethod]
		////public void TestOrderByDescendingThenByDescending()
		////{
		////	TestQuery2(
		////		"TestOrderByDescendingThenByDescending",
		////		Select.From<Customer>("c").OrderByDescending(c => c.CustomerID).ThenByDescending(c => c.Country).Columns(c => c.City)
		////		);
		////}

		////[TestMethod]
		////public void TestOrderByJoin()
		////{
		////	TestQuery2(
		////		"TestOrderByJoin",
		////		from c in Select.From<Customer>("c").OrderBy(c => c.CustomerID)
		////		join o in Select.From<Order>("o").OrderBy(o => o.OrderID) on c.CustomerID equals o.CustomerID
		////		select new { CustomerID = c.CustomerID, o.OrderID }
		////		);
		////}

		////[TestMethod]
		////public void TestOrderBySelectMany()
		////{
		////	TestQuery2(
		////		"TestOrderBySelectMany",
		////		from c in Select.From<Customer>("c").OrderBy(c => c.CustomerID)
		////		from o in Select.From<Order>("o").OrderBy(o => o.OrderID)
		////		where c.CustomerID == o.CustomerID
		////		select new { c.ContactName, o.OrderID }
		////		);
		////}

		////[TestMethod]
		////public void TestGroupBy()
		////{
		////	TestQuery2(
		////		"TestGroupBy",
		////		Select.From<Customer>("c").GroupBy(c => c.City)
		////		);
		////}

		////[TestMethod]
		////public void TestGroupBySelectMany()
		////{
		////	TestQuery2(
		////		"TestGroupBySelectMany",
		////		Select.From<Customer>("c").GroupBy(c => c.City).SelectMany(g => g)
		////		);
		////}

		////[TestMethod]
		////public void TestGroupBySum()
		////{
		////	TestQuery2(
		////		"TestGroupBySum",
		////		Select.From<Order>("o").GroupBy(o => o.CustomerID).Columns(g => g.Sum(o => o.OrderID))
		////		);
		////}

		////[TestMethod]
		////public void TestGroupByCount()
		////{
		////	TestQuery2(
		////		"TestGroupByCount",
		////		Select.From<Order>("o").GroupBy(o => o.CustomerID).Columns(g => g.Count())
		////		);
		////}

		////[TestMethod]
		////public void TestGroupByLongCount()
		////{
		////	TestQuery2(
		////		"TestGroupByLongCount",
		////		Select.From<Order>("o").GroupBy(o => o.CustomerID).Columns(g => g.LongCount()));
		////}

		////[TestMethod]
		////public void TestGroupBySumMinMaxAvg()
		////{
		////	TestQuery2(
		////		"TestGroupBySumMinMaxAvg",
		////		Select.From<Order>("o").GroupBy(o => o.CustomerID).Columns(g =>
		////			new
		////			{
		////				Sum = g.Sum(o => o.OrderID),
		////				Min = g.Min(o => o.OrderID),
		////				Max = g.Max(o => o.OrderID),
		////				Avg = g.Average(o => o.OrderID)
		////			})
		////		);
		////}

		////[TestMethod]
		////public void TestGroupByWithResultSelector()
		////{
		////	TestQuery2(
		////		"TestGroupByWithResultSelector",
		////		Select.From<Order>("o").GroupBy(o => o.CustomerID, (k, g) =>
		////			new
		////			{
		////				Sum = g.Sum(o => o.OrderID),
		////				Min = g.Min(o => o.OrderID),
		////				Max = g.Max(o => o.OrderID),
		////				Avg = g.Average(o => o.OrderID)
		////			})
		////		);
		////}

		////[TestMethod]
		////public void TestGroupByWithElementSelectorSum()
		////{
		////	TestQuery2(
		////		"TestGroupByWithElementSelectorSum",
		////		Select.From<Order>("o").GroupBy(o => o.CustomerID, o => o.OrderID).Columns(g => g.Sum())
		////		);
		////}

		////[TestMethod]
		////public void TestGroupByWithElementSelector()
		////{
		////	TestQuery2(
		////		"TestGroupByWithElementSelector",
		////		Select.From<Order>("o").GroupBy(o => o.CustomerID, o => o.OrderID)
		////		);
		////}

		////[TestMethod]
		////public void TestGroupByWithElementSelectorSumMax()
		////{
		////	TestQuery2(
		////		"TestGroupByWithElementSelectorSumMax",
		////		Select.From<Order>("o").GroupBy(o => o.CustomerID, o => o.OrderID).Columns(g => new { Sum = g.Sum(), Max = g.Max() })
		////		);
		////}

		////[TestMethod]
		////public void TestGroupByWithAnonymousElement()
		////{
		////	TestQuery2(
		////		"TestGroupByWithAnonymousElement",
		////		Select.From<Order>("o").GroupBy(o => o.CustomerID, o => new { o.OrderID }).Columns(g => g.Sum(x => x.OrderID))
		////		);
		////}

		////[TestMethod]
		////public void TestGroupByWithTwoPartKey()
		////{
		////	TestQuery2(
		////		"TestGroupByWithTwoPartKey",
		////		Select.From<Order>("o").GroupBy(o => new { CustomerID = o.CustomerID, o.OrderDate }).Columns(g => g.Sum(o => o.OrderID))
		////		);
		////}

		////[TestMethod]
		////public void TestOrderByGroupBy()
		////{
		////	TestQuery2(
		////		"TestOrderByGroupBy",
		////		Select.From<Order>("o").OrderBy(o => o.OrderID).GroupBy(o => o.CustomerID).Columns(g => g.Sum(o => o.OrderID))
		////		);
		////}

		////[TestMethod]
		////public void TestOrderByGroupBySelectMany()
		////{
		////	TestQuery2(
		////		"TestOrderByGroupBySelectMany",
		////		Select.From<Order>("o").OrderBy(o => o.OrderID).GroupBy(o => o.CustomerID).SelectMany(g => g)
		////		);
		////}

		////[TestMethod]
		////public void TestSumWithNoArg()
		////{
		////	TestQuery2(
		////		"TestSumWithNoArg",
		////		() => Select.From<Order>("o").Select(o => o.OrderID).Sum()
		////		);
		////}

		////[TestMethod]
		////public void TestSumWithArg()
		////{
		////	TestQuery2(
		////		"TestSumWithArg",
		////		() => Select.From<Order>("o").Sum(o => o.OrderID)
		////		);
		////}

		////[TestMethod]
		////public void TestCountWithNoPredicate()
		////{
		////	TestQuery2(
		////		"TestCountWithNoPredicate",
		////		() => Select.From<Order>("o").Count()
		////		);
		////}

		////[TestMethod]
		////public void TestCountWithPredicate()
		////{
		////	TestQuery2(
		////		"TestCountWithPredicate",
		////		() => Select.From<Order>("o").Count(o => o.CustomerID == "ALFKI")
		////		);
		////}

		////[TestMethod]
		////public void TestDistinct()
		////{
		////	TestQuery2(
		////		"TestDistinct",
		////		Select.From<Customer>("c").Distinct()
		////		);
		////}

		////[TestMethod]
		////public void TestDistinctScalar()
		////{
		////	TestQuery2(
		////		"TestDistinctScalar",
		////		Select.From<Customer>("c").Columns(c => c.City).Distinct()
		////		);
		////}

		////[TestMethod]
		////public void TestOrderByDistinct()
		////{
		////	TestQuery2(
		////		"TestOrderByDistinct",
		////		Select.From<Customer>("c").OrderBy(c => c.CustomerID).Columns(c => c.City).Distinct()
		////		);
		////}

		////[TestMethod]
		////public void TestDistinctOrderBy()
		////{
		////	TestQuery2(
		////		"TestDistinctOrderBy",
		////		Select.From<Customer>("c").Columns(c => c.City).Distinct().OrderBy(c => c)
		////		);
		////}

		////[TestMethod]
		////public void TestDistinctGroupBy()
		////{
		////	TestQuery2(
		////		"TestDistinctGroupBy",
		////		Select.From<Order>("o").Distinct().GroupBy(o => o.CustomerID)
		////		);
		////}

		////[TestMethod]
		////public void TestGroupByDistinct()
		////{
		////	TestQuery2(
		////		"TestGroupByDistinct",
		////		Select.From<Order>("o").GroupBy(o => o.CustomerID).Distinct()
		////		);

		////}

		////[TestMethod]
		////public void TestDistinctCount()
		////{
		////	TestQuery2(
		////		"TestDistinctCount",
		////		() => Select.From<Customer>("c").Distinct().Count()
		////		);
		////}

		////[TestMethod]
		////public void TestSelectDistinctCount()
		////{
		////	TestQuery2(
		////		"TestSelectDistinctCount",
		////		() => Select.From<Customer>("c").Columns(c => c.City).Distinct().Count()
		////		);
		////}

		////[TestMethod]
		////public void TestSelectSelectDistinctCount()
		////{
		////	TestQuery2(
		////		"TestSelectSelectDistinctCount",
		////		() => Select.From<Customer>("c").Columns(c => c.City).Columns(c => c).Distinct().Count()
		////		);
		////}

		////[TestMethod]
		////public void TestDistinctCountPredicate()
		////{
		////	TestQuery2(
		////		"TestDistinctCountPredicate",
		////		() => Select.From<Customer>("c").Distinct().Count(c => c.CustomerID == "ALFKI")
		////		);
		////}

		////[TestMethod]
		////public void TestDistinctSumWithArg()
		////{
		////	TestQuery2(
		////		"TestDistinctSumWithArg",
		////		() => Select.From<Order>("o").Distinct().Sum(o => o.OrderID)
		////		);
		////}

		////[TestMethod]
		////public void TestSelectDistinctSum()
		////{
		////	TestQuery2(
		////		"TestSelectDistinctSum",
		////		() => Select.From<Order>("o").Select(o => o.OrderID).Distinct().Sum()
		////		);
		////}

		////[TestMethod]
		////public void TestTake()
		////{
		////	TestQuery2(
		////		"TestTake",
		////		Select.From<Order>("o").Take(5)
		////		);
		////}

		////[TestMethod]
		////public void TestTakeDistinct()
		////{
		////	TestQuery2(
		////		"TestTakeDistinct",
		////		Select.From<Order>("o").Take(5).Distinct()
		////		);
		////}

		////[TestMethod]
		////public void TestDistinctTake()
		////{
		////	TestQuery2(
		////		"TestDistinctTake",
		////		Select.From<Order>("o").Distinct().Take(5)
		////		);
		////}

		////[TestMethod]
		////public void TestDistinctTakeCount()
		////{
		////	TestQuery2(
		////		"TestDistinctTakeCount",
		////		() => Select.From<Order>("o").Distinct().Take(5).Count()
		////		);
		////}

		////[TestMethod]
		////public void TestTakeDistinctCount()
		////{
		////	TestQuery2(
		////		"TestTakeDistinctCount",
		////		() => Select.From<Order>("o").Take(5).Distinct().Count()
		////		);
		////}

		////[TestMethod]
		////public void TestSkip()
		////{
		////	TestQuery2(
		////		"TestSkip",
		////		Select.From<Customer>("c").OrderBy(c => c.ContactName).Skip(5)
		////		);
		////}

		////[TestMethod]
		////public void TestTakeSkip()
		////{
		////	TestQuery2(
		////		"TestTakeSkip",
		////		Select.From<Customer>("c").OrderBy(c => c.ContactName).Take(10).Skip(5)
		////		);
		////}

		////[TestMethod]
		////public void TestDistinctSkip()
		////{
		////	TestQuery2(
		////		"TestDistinctSkip",
		////		Select.From<Customer>("c").Distinct().OrderBy(c => c.ContactName).Skip(5)
		////		);
		////}

		////[TestMethod]
		////public void TestSkipTake()
		////{
		////	TestQuery2(
		////		"TestSkipTake",
		////		Select.From<Customer>("c").OrderBy(c => c.ContactName).Skip(5).Take(10)
		////		);
		////}

		////[TestMethod]
		////public void TestDistinctSkipTake()
		////{
		////	TestQuery2(
		////		"TestDistinctSkipTake",
		////		Select.From<Customer>("c").Distinct().OrderBy(c => c.ContactName).Skip(5).Take(10)
		////		);
		////}

		////[TestMethod]
		////public void TestSkipDistinct()
		////{
		////	TestQuery2(
		////		"TestSkipDistinct",
		////		Select.From<Customer>("c").OrderBy(c => c.ContactName).Skip(5).Distinct()
		////		);
		////}

		////[TestMethod]
		////public void TestSkipTakeDistinct()
		////{
		////	TestQuery2(
		////		"TestSkipTakeDistinct",
		////		Select.From<Customer>("c").OrderBy(c => c.ContactName).Skip(5).Take(10).Distinct()
		////		);
		////}

		////[TestMethod]
		////public void TestTakeSkipDistinct()
		////{
		////	TestQuery2(
		////		"TestTakeSkipDistinct",
		////		Select.From<Customer>("c").OrderBy(c => c.ContactName).Take(10).Skip(5).Distinct()
		////		);
		////}

		////////[TestMethod]
		////////public void TestFirst()
		////////{
		////////	TestQuery2(
		////////		"TestFirst",
		////////		() => Select.From<Customer>("c").OrderBy(c => c.ContactName).First()
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestFirstPredicate()
		////////{
		////////	TestQuery2(
		////////		"TestFirstPredicate",
		////////		() => Select.From<Customer>("c").OrderBy(c => c.ContactName).First(c => c.City == "London")
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestWhereFirst()
		////////{
		////////	TestQuery2(
		////////		"TestWhereFirst",
		////////		() => Select.From<Customer>("c").OrderBy(c => c.ContactName).Where(c => c.City == "London").First()
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestFirstOrDefault()
		////////{
		////////	TestQuery2(
		////////		"TestFirstOrDefault",
		////////		() => Select.From<Customer>("c").OrderBy(c => c.ContactName).FirstOrDefault()
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestFirstOrDefaultPredicate()
		////////{
		////////	TestQuery2(
		////////		"TestFirstOrDefaultPredicate",
		////////		() => Select.From<Customer>("c").OrderBy(c => c.ContactName).FirstOrDefault(c => c.City == "London")
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestWhereFirstOrDefault()
		////////{
		////////	TestQuery2(
		////////		"TestWhereFirstOrDefault",
		////////		() => Select.From<Customer>("c").OrderBy(c => c.ContactName).Where(c => c.City == "London").FirstOrDefault()
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestReverse()
		////////{
		////////	TestQuery2(
		////////		"TestReverse",
		////////		Select.From<Customer>("c").OrderBy(c => c.ContactName).Reverse()
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestReverseReverse()
		////////{
		////////	TestQuery2(
		////////		"TestReverseReverse",
		////////		Select.From<Customer>("c").OrderBy(c => c.ContactName).Reverse().Reverse()
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestReverseWhereReverse()
		////////{
		////////	TestQuery2(
		////////		"TestReverseWhereReverse",
		////////		Select.From<Customer>("c").OrderBy(c => c.ContactName).Reverse().Where(c => c.City == "London").Reverse()
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestReverseTakeReverse()
		////////{
		////////	TestQuery2(
		////////		"TestReverseTakeReverse",
		////////		Select.From<Customer>("c").OrderBy(c => c.ContactName).Reverse().Take(5).Reverse()
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestReverseWhereTakeReverse()
		////////{
		////////	TestQuery2(
		////////		"TestReverseWhereTakeReverse",
		////////		Select.From<Customer>("c").OrderBy(c => c.ContactName).Reverse().Where(c => c.City == "London").Take(5).Reverse()
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestLast()
		////////{
		////////	TestQuery2(
		////////		"TestLast",
		////////		() => Select.From<Customer>("c").OrderBy(c => c.ContactName).Last()
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestLastPredicate()
		////////{
		////////	TestQuery2(
		////////		"TestLastPredicate",
		////////		() => Select.From<Customer>("c").OrderBy(c => c.ContactName).Last(c => c.City == "London")
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestWhereLast()
		////////{
		////////	TestQuery2(
		////////		"TestWhereLast",
		////////		() => Select.From<Customer>("c").OrderBy(c => c.ContactName).Where(c => c.City == "London").Last()
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestLastOrDefault()
		////////{
		////////	TestQuery2(
		////////		"TestLastOrDefault",
		////////		() => Select.From<Customer>("c").OrderBy(c => c.ContactName).LastOrDefault()
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestLastOrDefaultPredicate()
		////////{
		////////	TestQuery2(
		////////		"TestLastOrDefaultPredicate",
		////////		() => Select.From<Customer>("c").OrderBy(c => c.ContactName).LastOrDefault(c => c.City == "London")
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestWhereLastOrDefault()
		////////{
		////////	TestQuery2(
		////////		"TestWhereLastOrDefault",
		////////		() => Select.From<Customer>("c").OrderBy(c => c.ContactName).Where(c => c.City == "London").LastOrDefault()
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestSingle()
		////////{
		////////	TestQuery2(
		////////		"TestSingle",
		////////		() => Select.From<Customer>("c").Single());
		////////}

		////////[TestMethod]
		////////public void TestSinglePredicate()
		////////{
		////////	TestQuery2(
		////////		"TestSinglePredicate",
		////////		() => Select.From<Customer>("c").Single(c => c.CustomerID == "ALFKI")
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestWhereSingle()
		////////{
		////////	TestQuery2(
		////////		"TestWhereSingle",
		////////		() => Select.From<Customer>("c").Where(c => c.CustomerID == "ALFKI").Single()
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestSingleOrDefault()
		////////{
		////////	TestQuery2(
		////////		"TestSingleOrDefault",
		////////		() => Select.From<Customer>("c").SingleOrDefault());
		////////}

		////////[TestMethod]
		////////public void TestSingleOrDefaultPredicate()
		////////{
		////////	TestQuery2(
		////////		"TestSingleOrDefaultPredicate",
		////////		() => Select.From<Customer>("c").SingleOrDefault(c => c.CustomerID == "ALFKI")
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestWhereSingleOrDefault()
		////////{
		////////	TestQuery2(
		////////		"TestWhereSingleOrDefault",
		////////		() => Select.From<Customer>("c").Where(c => c.CustomerID == "ALFKI").SingleOrDefault()
		////////		);
		////////}

		////[TestMethod]
		////public void TestAnyWithSubquery()
		////{
		////	TestQuery2(
		////		"TestAnyWithSubquery",
		////		Select.From<Customer>("c").Where(c => Select.From<Order>("o").Where(o => o.CustomerID == c.CustomerID).Any(o => o.OrderDate.Year == 1997))
		////		);
		////}

		////[TestMethod]
		////public void TestAnyWithSubqueryNoPredicate()
		////{
		////	TestQuery2(
		////		"TestAnyWithSubqueryNoPredicate",
		////		Select.From<Customer>("c").Where(c => Select.From<Order>("o").Where(o => o.CustomerID == c.CustomerID).Any())
		////		);
		////}

		////[TestMethod]
		////public void TestAnyWithLocalCollection()
		////{
		////	string[] ids = new[] { "ABCDE", "ALFKI" };
		////	TestQuery2(
		////		"TestAnyWithLocalCollection",
		////		Select.From<Customer>("c").Where(c => ids.Any(id => c.CustomerID == id))
		////		);
		////}

		////////[TestMethod]
		////////public void TestAnyTopLevel()
		////////{
		////////	TestQuery2(
		////////		"TestAnyTopLevel",
		////////		() => Select.From<Customer>("c").Any()
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestAllWithSubquery()
		////////{
		////////	TestQuery2(
		////////		"TestAllWithSubquery",
		////////		Select.From<Customer>("c").Where(c => Select.From<Order>("o").Where(o => o.CustomerID == c.CustomerID).All(o => o.OrderDate.Year == 1997))
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestAllWithLocalCollection()
		////////{
		////////	string[] patterns = new[] { "a", "e" };

		////////	TestQuery2(
		////////		"TestAllWithLocalCollection",
		////////		Select.From<Customer>("c").Where(c => patterns.All(p => c.ContactName.Contains(p)))
		////////		);
		////////}

		////////[TestMethod]
		////////public void TestAllTopLevel()
		////////{
		////////	TestQuery2(
		////////		"TestAllTopLevel",
		////////		() => Select.From<Customer>("c").All(c => c.ContactName.StartsWith("a"))
		////////		);
		////////}

		////[TestMethod]
		////public void TestContainsWithSubquery()
		////{
		////	TestQuery2(
		////		"TestContainsWithSubquery",
		////		Select.From<Customer>("c").Where(c => Select.From<Order>("o").Columns(o => o.CustomerID).Contains(c.CustomerID))
		////		);
		////}

		[TestMethod]
		public void TestContainsWithLocalCollection()
		{
			string[] ids = new[] { "ABCDE", "ALFKI" };
			TestQuery2(
				"TestContainsWithLocalCollection",
				Select.From<Customer>("c").Where(c => ids.Contains(c.CustomerID))
				);
		}

		////////[TestMethod]
		////////public void TestContainsTopLevel()
		////////{
		////////	TestQuery2(
		////////		"TestContainsTopLevel",
		////////		() => Select.From<Customer>("c").Columns(c => c.CustomerID).Contains("ALFKI")
		////////		);
		////////}

		////[TestMethod]
		////public void TestCoalesce()
		////{
		////	TestQuery2(
		////		"TestCoalesce",
		////		Select.From<Customer>("c").Where(c => (c.City ?? "Seattle") == "Seattle"));
		////}

		////[TestMethod]
		////public void TestCoalesce2()
		////{
		////	TestQuery2(
		////		"TestCoalesce2",
		////		Select.From<Customer>("c").Where(c => (c.City ?? c.Country ?? "Seattle") == "Seattle"));
		////}

		[TestMethod]
		public void TestStringLength()
		{
			TestQuery2(
				"TestStringLength",
				Select.From<Customer>("c").Where(c => c.City.Length == 7));
		}

		[TestMethod]
		public void TestStringStartsWithLiteral()
		{
			TestQuery2(
				"TestStringStartsWithLiteral",
				Select.From<Customer>("c").Where(c => c.ContactName.StartsWith("M")));
		}

		[TestMethod]
		public void TestStringStartsWithColumn()
		{
			TestQuery2(
				"TestStringStartsWithColumn",
				Select.From<Customer>("c").Where(c => c.ContactName.StartsWith(c.ContactName)));
		}

		[TestMethod]
		public void TestStringEndsWithLiteral()
		{
			TestQuery2(
				"TestStringEndsWithLiteral",
				Select.From<Customer>("c").Where(c => c.ContactName.EndsWith("s")));
		}

		[TestMethod]
		public void TestStringEndsWithColumn()
		{
			TestQuery2(
				"TestStringEndsWithColumn",
				Select.From<Customer>("c").Where(c => c.ContactName.EndsWith(c.ContactName)));
		}

		[TestMethod]
		public void TestStringContainsLiteral()
		{
			TestQuery2(
				"TestStringContainsLiteral",
				Select.From<Customer>("c").Where(c => c.ContactName.Contains("and")));
		}

		[TestMethod]
		public void TestStringContainsColumn()
		{
			TestQuery2(
				"TestStringContainsColumn",
				Select.From<Customer>("c").Where(c => c.ContactName.Contains(c.ContactName)));
		}

		[TestMethod]
		public void TestStringConcatImplicit2Args()
		{
			TestQuery2(
				"TestStringConcatImplicit2Args",
				Select.From<Customer>("c").Where(c => c.ContactName + "X" == "X"));
		}

		[TestMethod]
		public void TestStringConcatExplicit2Args()
		{
			TestQuery2(
				"TestStringConcatExplicit2Args",
				Select.From<Customer>("c").Where(c => string.Concat(c.ContactName, "X") == "X"));
		}

		[TestMethod]
		public void TestStringConcatExplicit3Args()
		{
			TestQuery2(
				"TestStringConcatExplicit3Args",
				Select.From<Customer>("c").Where(c => string.Concat(c.ContactName, "X", c.Country) == "X"));
		}

		[TestMethod]
		public void TestStringConcatExplicitNArgs()
		{
			TestQuery2(
				"TestStringConcatExplicitNArgs",
				Select.From<Customer>("c").Where(c => string.Concat(new string[] { c.ContactName, "X", c.Country }) == "X"));
		}

		[TestMethod]
		public void TestStringIsNullOrEmpty()
		{
			TestQuery2(
				"TestStringIsNullOrEmpty",
				Select.From<Customer>("c").Where(c => string.IsNullOrEmpty(c.City)));
		}

		[TestMethod]
		public void TestStringToUpper()
		{
			TestQuery2(
				"TestStringToUpper",
				Select.From<Customer>("c").Where(c => c.City.ToUpper() == "SEATTLE"));
		}

		[TestMethod]
		public void TestStringToLower()
		{
			TestQuery2(
				"TestStringToLower",
				Select.From<Customer>("c").Where(c => c.City.ToLower() == "seattle"));
		}

		[TestMethod]
		public void TestStringSubstring()
		{
			TestQuery2(
				"TestStringSubstring",
				Select.From<Customer>("c").Where(c => c.City.Substring(0, 4) == "Seat"));
		}

		[TestMethod]
		public void TestStringSubstringNoLength()
		{
			TestQuery2(
				"TestStringSubstringNoLength",
				Select.From<Customer>("c").Where(c => c.City.Substring(4) == "tle"));
		}

		[TestMethod]
		public void TestStringIndexOf()
		{
			TestQuery2(
				"TestStringIndexOf",
				Select.From<Customer>("c").Where(c => c.City.IndexOf("tt") == 4));
		}

		[TestMethod]
		public void TestStringIndexOfChar()
		{
			TestQuery2(
				"TestStringIndexOfChar",
				Select.From<Customer>("c").Where(c => c.City.IndexOf('t') == 4));
		}

		[TestMethod]
		public void TestStringReplace()
		{
			TestQuery2(
				"TestStringReplace",
				Select.From<Customer>("c").Where(c => c.City.Replace("ea", "ae") == "Saettle"));
		}

		[TestMethod]
		public void TestStringReplaceChars()
		{
			TestQuery2(
				"TestStringReplaceChars",
				Select.From<Customer>("c").Where(c => c.City.Replace("e", "y") == "Syattly"));
		}

		[TestMethod]
		public void TestStringTrim()
		{
			TestQuery2(
				"TestStringTrim",
				Select.From<Customer>("c").Where(c => c.City.Trim() == "Seattle"));
		}

		[TestMethod]
		public void TestStringToString()
		{
			TestQuery2(
				"TestStringToString",
				Select.From<Customer>("c").Where(c => c.City.ToString() == "Seattle"));
		}

		[TestMethod]
		public void TestStringRemove()
		{
			TestQuery2(
				"TestStringRemove",
				Select.From<Customer>("c").Where(c => c.City.Remove(1, 2) == "Sttle"));
		}

		[TestMethod]
		public void TestStringRemoveNoCount()
		{
			TestQuery2(
				"TestStringRemoveNoCount",
				Select.From<Customer>("c").Where(c => c.City.Remove(4) == "Seat"));
		}

		[TestMethod]
		public void TestDateTimeConstructYmd()
		{
			TestQuery2(
				"TestDateTimeConstructYmd",
				Select.From<Order>("o").Where(o => o.OrderDate == new DateTime(o.OrderDate.Year, 1, 1)));
		}

		[TestMethod]
		public void TestDateTimeConstructYmdhms()
		{
			TestQuery2(
				"TestDateTimeConstructYmdhms",
				Select.From<Order>("o").Where(o => o.OrderDate == new DateTime(o.OrderDate.Year, 1, 1, 10, 25, 55)));
		}

		[TestMethod]
		public void TestDateTimeDay()
		{
			TestQuery2(
				"TestDateTimeDay",
				Select.From<Order>("o").Where(o => o.OrderDate.Day == 5));
		}

		[TestMethod]
		public void TestDateTimeMonth()
		{
			TestQuery2(
				"TestDateTimeMonth",
				Select.From<Order>("o").Where(o => o.OrderDate.Month == 12));
		}

		[TestMethod]
		public void TestDateTimeYear()
		{
			TestQuery2(
				"TestDateTimeYear",
				Select.From<Order>("o").Where(o => o.OrderDate.Year == 1997));
		}

		[TestMethod]
		public void TestDateTimeHour()
		{
			TestQuery2(
				"TestDateTimeHour",
				Select.From<Order>("o").Where(o => o.OrderDate.Hour == 6));
		}

		[TestMethod]
		public void TestDateTimeMinute()
		{
			TestQuery2(
				"TestDateTimeMinute",
				Select.From<Order>("o").Where(o => o.OrderDate.Minute == 32));
		}

		[TestMethod]
		public void TestDateTimeSecond()
		{
			TestQuery2(
				"TestDateTimeSecond",
				Select.From<Order>("o").Where(o => o.OrderDate.Second == 47));
		}

		[TestMethod]
		public void TestDateTimeMillisecond()
		{
			TestQuery2(
				"TestDateTimeMillisecond",
				Select.From<Order>("o").Where(o => o.OrderDate.Millisecond == 200));
		}

		[TestMethod]
		public void TestDateTimeDayOfWeek()
		{
			TestQuery2(
				"TestDateTimeDayOfWeek",
				Select.From<Order>("o").Where(o => o.OrderDate.DayOfWeek == DayOfWeek.Friday));
		}

		[TestMethod]
		public void TestDateTimeDayOfYear()
		{
			TestQuery2(
				"TestDateTimeDayOfYear",
				Select.From<Order>("o").Where(o => o.OrderDate.DayOfYear == 360));
		}

		[TestMethod]
		public void TestMathAbs()
		{
			TestQuery2(
				"TestMathAbs",
				Select.From<Order>("o").Where(o => Math.Abs(o.OrderID) == 10));
		}

		[TestMethod]
		public void TestMathAcos()
		{
			TestQuery2(
				"TestMathAcos",
				Select.From<Order>("o").Where(o => Math.Acos(o.OrderID) == 0),
				false);
		}

		[TestMethod]
		public void TestMathAsin()
		{
			TestQuery2(
				"TestMathAsin",
				Select.From<Order>("o").Where(o => Math.Asin(o.OrderID) == 0),
				false);
		}

		[TestMethod]
		public void TestMathAtan()
		{
			TestQuery2(
				"TestMathAtan",
				Select.From<Order>("o").Where(o => Math.Atan(o.OrderID) == 0),
				false);
		}

		[TestMethod]
		public void TestMathAtan2()
		{
			TestQuery2(
				"TestMathAtan2",
				Select.From<Order>("o").Where(o => Math.Atan2(o.OrderID, 3) == 0),
				false);
		}

		[TestMethod]
		public void TestMathCos()
		{
			TestQuery2(
				"TestMathCos",
				Select.From<Order>("o").Where(o => Math.Cos(o.OrderID) == 0),
				false);
		}

		[TestMethod]
		public void TestMathSin()
		{
			TestQuery2(
				"TestMathSin",
				Select.From<Order>("o").Where(o => Math.Sin(o.OrderID) == 0),
				false);
		}

		[TestMethod]
		public void TestMathTan()
		{
			TestQuery2(
				"TestMathTan",
				Select.From<Order>("o").Where(o => Math.Tan(o.OrderID) == 0),
				false);
		}

		[TestMethod]
		public void TestMathExp()
		{
			TestQuery2(
				"TestMathExp",
				Select.From<Order>("o").Where(o => Math.Exp(o.OrderID) == 0),
				false);
		}

		[TestMethod]
		public void TestMathLog()
		{
			TestQuery2(
				"TestMathLog",
				Select.From<Order>("o").Where(o => Math.Log(o.OrderID) == 0),
				false);
		}

		[TestMethod]
		public void TestMathLog10()
		{
			TestQuery2(
				"TestMathLog10",
				Select.From<Order>("o").Where(o => Math.Log10(o.OrderID) == 0),
				false);
		}

		[TestMethod]
		public void TestMathSqrt()
		{
			TestQuery2(
				"TestMathSqrt",
				Select.From<Order>("o").Where(o => Math.Sqrt(o.OrderID) == 0),
				false);
		}

		[TestMethod]
		public void TestMathCeiling()
		{
			TestQuery2(
				"TestMathCeiling",
				Select.From<Order>("o").Where(o => Math.Ceiling((double)o.OrderID) == 0));
		}

		[TestMethod]
		public void TestMathFloor()
		{
			TestQuery2(
				"TestMathFloor",
				Select.From<Order>("o").Where(o => Math.Floor((double)o.OrderID) == 0));
		}

		[TestMethod]
		public void TestMathPow()
		{
			TestQuery2(
				"TestMathPow",
				Select.From<Order>("o").Where(o => Math.Pow(o.OrderID < 1000 ? 1 : 2, 3) == 0),
				false);
		}

		[TestMethod]
		public void TestMathRoundDefault()
		{
			TestQuery2(
				"TestMathRoundDefault",
				Select.From<Order>("o").Where(o => Math.Round((decimal)o.OrderID) == 0));
		}

		[TestMethod]
		public void TestMathRoundToPlace()
		{
			TestQuery2(
				"TestMathRoundToPlace",
				Select.From<Order>("o").Where(o => Math.Round((decimal)o.OrderID, 2) == 0));
		}

		[TestMethod]
		public void TestMathTruncate()
		{
			TestQuery2(
				"TestMathTruncate",
				Select.From<Order>("o").Where(o => Math.Truncate((double)o.OrderID) == 0));
		}

		[TestMethod]
		public void TestStringCompareToLessThan()
		{
			TestQuery2(
				"TestStringCompareToLessThan",
				Select.From<Customer>("c").Where(c => c.City.CompareTo("Seattle") < 0));
		}

		[TestMethod]
		public void TestStringCompareToLessThanOrEqualTo()
		{
			TestQuery2(
				"TestStringCompareToLessThanOrEqualTo",
				Select.From<Customer>("c").Where(c => c.City.CompareTo("Seattle") <= 0));
		}

		[TestMethod]
		public void TestStringCompareToGreaterThan()
		{
			TestQuery2(
				"TestStringCompareToGreaterThan",
				Select.From<Customer>("c").Where(c => c.City.CompareTo("Seattle") > 0));
		}

		[TestMethod]
		public void TestStringCompareToGreaterThanOrEqualTo()
		{
			TestQuery2(
				"TestStringCompareToGreaterThanOrEqualTo",
				Select.From<Customer>("c").Where(c => c.City.CompareTo("Seattle") >= 0));
		}

		[TestMethod]
		public void TestStringCompareToEquals()
		{
			TestQuery2(
				"TestStringCompareToEquals",
				Select.From<Customer>("c").Where(c => c.City.CompareTo("Seattle") == 0));
		}

		[TestMethod]
		public void TestStringCompareToNotEquals()
		{
			TestQuery2(
				"TestStringCompareToNotEquals",
				Select.From<Customer>("c").Where(c => c.City.CompareTo("Seattle") != 0));
		}

		[TestMethod]
		public void TestStringCompareLessThan()
		{
			TestQuery2(
				"TestStringCompareLessThan",
				Select.From<Customer>("c").Where(c => string.Compare(c.City, "Seattle") < 0));
		}

		[TestMethod]
		public void TestStringCompareLessThanOrEqualTo()
		{
			TestQuery2(
				"TestStringCompareLessThanOrEqualTo",
				Select.From<Customer>("c").Where(c => string.Compare(c.City, "Seattle") <= 0));
		}

		[TestMethod]
		public void TestStringCompareGreaterThan()
		{
			TestQuery2(
				"TestStringCompareGreaterThan",
				Select.From<Customer>("c").Where(c => string.Compare(c.City, "Seattle") > 0));
		}

		[TestMethod]
		public void TestStringCompareGreaterThanOrEqualTo()
		{
			TestQuery2(
				"TestStringCompareGreaterThanOrEqualTo",
				Select.From<Customer>("c").Where(c => string.Compare(c.City, "Seattle") >= 0));
		}

		[TestMethod]
		public void TestStringCompareEquals()
		{
			TestQuery2(
				"TestStringCompareEquals",
				Select.From<Customer>("c").Where(c => string.Compare(c.City, "Seattle") == 0));
		}

		[TestMethod]
		public void TestStringCompareNotEquals()
		{
			TestQuery2(
				"TestStringCompareNotEquals",
				Select.From<Customer>("c").Where(c => string.Compare(c.City, "Seattle") != 0));
		}

		[TestMethod]
		public void TestIntCompareTo()
		{
			TestQuery2(
				"TestIntCompareTo",
				Select.From<Order>("o").Where(o => o.OrderID.CompareTo(1000) == 0));
		}

		[TestMethod]
		public void TestDecimalCompare()
		{
			TestQuery2(
				"TestDecimalCompare",
				Select.From<Order>("o").Where(o => decimal.Compare((decimal)o.OrderID, 0.0m) == 0));
		}

		[TestMethod]
		public void TestDecimalAdd()
		{
			TestQuery2(
				"TestDecimalAdd",
				Select.From<Order>("o").Where(o => decimal.Add(o.OrderID, 0.0m) == 0.0m));
		}

		[TestMethod]
		public void TestDecimalSubtract()
		{
			TestQuery2(
				"TestDecimalSubtract",
				Select.From<Order>("o").Where(o => decimal.Subtract(o.OrderID, 0.0m) == 0.0m));
		}

		[TestMethod]
		public void TestDecimalMultiply()
		{
			TestQuery2(
				"TestDecimalMultiply",
				Select.From<Order>("o").Where(o => decimal.Multiply(o.OrderID, 1.0m) == 1.0m));
		}

		[TestMethod]
		public void TestDecimalDivide()
		{
			TestQuery2(
				"TestDecimalDivide",
				Select.From<Order>("o").Where(o => decimal.Divide(o.OrderID, 1.0m) == 1.0m));
		}

		[TestMethod]
		public void TestDecimalRemainder()
		{
			TestQuery2(
				"TestDecimalRemainder",
				Select.From<Order>("o").Where(o => decimal.Remainder(o.OrderID, 1.0m) == 0.0m));
		}

		[TestMethod]
		public void TestDecimalNegate()
		{
			TestQuery2(
				"TestDecimalNegate",
				Select.From<Order>("o").Where(o => decimal.Negate(o.OrderID) == 1.0m));
		}

		[TestMethod]
		public void TestDecimalCeiling()
		{
			TestQuery2(
				"TestDecimalCeiling",
				Select.From<Order>("o").Where(o => decimal.Ceiling(o.OrderID) == 0.0m));
		}

		[TestMethod]
		public void TestDecimalFloor()
		{
			TestQuery2(
				"TestDecimalFloor",
				Select.From<Order>("o").Where(o => decimal.Floor(o.OrderID) == 0.0m));
		}

		[TestMethod]
		public void TestDecimalRoundDefault()
		{
			TestQuery2(
				"TestDecimalRoundDefault",
				Select.From<Order>("o").Where(o => decimal.Round(o.OrderID) == 0m));
		}

		[TestMethod]
		public void TestDecimalRoundPlaces()
		{
			TestQuery2(
				"TestDecimalRoundPlaces",
				Select.From<Order>("o").Where(o => decimal.Round(o.OrderID, 2) == 0.00m));
		}

		[TestMethod]
		public void TestDecimalTruncate()
		{
			TestQuery2(
				"TestDecimalTruncate",
				Select.From<Order>("o").Where(o => decimal.Truncate(o.OrderID) == 0m));
		}

		[TestMethod]
		public void TestDecimalLessThan()
		{
			TestQuery2(
				"TestDecimalLessThan",
				Select.From<Order>("o").Where(o => ((decimal)o.OrderID) < 0.0m));
		}

		[TestMethod]
		public void TestIntLessThan()
		{
			TestQuery2(
				"TestIntLessThan",
				Select.From<Order>("o").Where(o => o.OrderID < 0));
		}

		[TestMethod]
		public void TestIntLessThanOrEqual()
		{
			TestQuery2(
				"TestIntLessThanOrEqual",
				Select.From<Order>("o").Where(o => o.OrderID <= 0));
		}

		[TestMethod]
		public void TestIntGreaterThan()
		{
			TestQuery2(
				"TestIntGreaterThan",
				Select.From<Order>("o").Where(o => o.OrderID > 0));
		}

		[TestMethod]
		public void TestIntGreaterThanOrEqual()
		{
			TestQuery2(
				"TestIntGreaterThanOrEqual",
				Select.From<Order>("o").Where(o => o.OrderID >= 0));
		}

		[TestMethod]
		public void TestIntEqual()
		{
			TestQuery2(
				"TestIntEqual",
				Select.From<Order>("o").Where(o => o.OrderID == 0));
		}

		[TestMethod]
		public void TestIntNotEqual()
		{
			TestQuery2(
				"TestIntNotEqual",
				Select.From<Order>("o").Where(o => o.OrderID != 0));
		}

		[TestMethod]
		public void TestIntAdd()
		{
			TestQuery2(
				"TestIntAdd",
				Select.From<Order>("o").Where(o => o.OrderID + 0 == 0));
		}

		[TestMethod]
		public void TestIntSubtract()
		{
			TestQuery2(
				"TestIntSubtract",
				Select.From<Order>("o").Where(o => o.OrderID - 0 == 0));
		}

		[TestMethod]
		public void TestIntMultiply()
		{
			TestQuery2(
				"TestIntMultiply",
				Select.From<Order>("o").Where(o => o.OrderID * 1 == 1));
		}

		[TestMethod]
		public void TestIntDivide()
		{
			TestQuery2(
				"TestIntDivide",
				Select.From<Order>("o").Where(o => o.OrderID / 1 == 1));
		}

		[TestMethod]
		public void TestIntModulo()
		{
			TestQuery2(
				"TestIntModulo",
				Select.From<Order>("o").Where(o => o.OrderID % 1 == 0));
		}

		[TestMethod]
		public void TestIntLeftShift()
		{
			TestQuery2(
				"TestIntLeftShift",
				Select.From<Order>("o").Where(o => o.OrderID << 1 == 0));
		}

		[TestMethod]
		public void TestIntRightShift()
		{
			TestQuery2(
				"TestIntRightShift",
				Select.From<Order>("o").Where(o => o.OrderID >> 1 == 0));
		}

		[TestMethod]
		public void TestIntBitwiseAnd()
		{
			TestQuery2(
				"TestIntBitwiseAnd",
				Select.From<Order>("o").Where(o => (o.OrderID & 1) == 0));
		}

		[TestMethod]
		public void TestIntBitwiseOr()
		{
			TestQuery2(
				"TestIntBitwiseOr",
				Select.From<Order>("o").Where(o => (o.OrderID | 1) == 1));
		}

		[TestMethod]
		public void TestIntBitwiseExclusiveOr()
		{
			TestQuery2(
				"TestIntBitwiseExclusiveOr",
				Select.From<Order>("o").Where(o => (o.OrderID ^ 1) == 1));
		}

		////[TestMethod]
		////public void TestIntBitwiseNot()
		////{
		////	TestQuery2(
		////		"TestIntBitwiseNot",
		////		Select.From<Order>("o").Where(o => ~o.OrderID == 0));
		////}

		[TestMethod]
		public void TestIntNegate()
		{
			TestQuery2(
				"TestIntNegate",
				Select.From<Order>("o").Where(o => -o.OrderID == -1));
		}

		[TestMethod]
		public void TestAnd()
		{
			TestQuery2(
				"TestAnd",
				Select.From<Order>("o").Where(o => o.OrderID > 0 && o.OrderID < 2000));
		}

		[TestMethod]
		public void TestOr()
		{
			TestQuery2(
				"TestOr",
				Select.From<Order>("o").Where(o => o.OrderID < 5 || o.OrderID > 10));
		}

		[TestMethod]
		public void TestNot()
		{
			TestQuery2(
				"TestNot",
				Select.From<Order>("o").Where(o => !(o.OrderID == 0)));
		}

		[TestMethod]
		public void TestEqualNull()
		{
			TestQuery2(
				"TestEqualNull",
				Select.From<Customer>("c").Where(c => c.City == null));
		}

		[TestMethod]
		public void TestEqualNullReverse()
		{
			TestQuery2(
				"TestEqualNullReverse",
				Select.From<Customer>("c").Where(c => null == c.City));
		}

		[TestMethod]
		public void TestConditional()
		{
			TestQuery2(
				"TestConditional",
				Select.From<Order>("o").Where(o => (o.CustomerID == "ALFKI" ? 1000 : 0) == 1000));
		}

		[TestMethod]
		public void TestConditional2()
		{
			TestQuery2(
				"TestConditional2",
				Select.From<Order>("o").Where(o => (o.CustomerID == "ALFKI" ? 1000 : o.CustomerID == "ABCDE" ? 2000 : 0) == 1000));
		}

		[TestMethod]
		public void TestConditionalTestIsValue()
		{
			TestQuery2(
				"TestConditionalTestIsValue",
				Select.From<Order>("o").Where(o => (((bool)(object)o.OrderID) ? 100 : 200) == 100));
		}

		////[TestMethod]
		////public void TestConditionalResultsArePredicates()
		////{
		////	TestQuery2(
		////		"TestConditionalResultsArePredicates",
		////		Select.From<Order>("o").Where(o => (o.CustomerID == "ALFKI" ? o.OrderID < 10 : o.OrderID > 10)));
		////}

		////[TestMethod]
		////public void TestSelectManyJoined()
		////{
		////	TestQuery2(
		////		"TestSelectManyJoined",
		////		from c in Select.From<Customer>("c")
		////		from o in Select.From<Order>("o").Where(o => o.CustomerID == c.CustomerID)
		////		select new { c.ContactName, o.OrderDate });
		////}

		////[TestMethod]
		////public void TestSelectManyJoinedDefaultIfEmpty()
		////{
		////	TestQuery2(
		////		"TestSelectManyJoinedDefaultIfEmpty",
		////		from c in Select.From<Customer>("c")
		////		from o in Select.From<Order>("o").Where(o => o.CustomerID == c.CustomerID).DefaultIfEmpty()
		////		select new { c.ContactName, o.OrderDate });
		////}

		////[TestMethod]
		////public void TestSelectWhereAssociation()
		////{
		////	TestQuery2(
		////		"TestSelectWhereAssociation",
		////		from o in Select.From<Order>("o")
		////		where o.Customer.City == "Seattle"
		////		select o);
		////}

		////[TestMethod]
		////public void TestSelectWhereAssociations()
		////{
		////	TestQuery2(
		////		"TestSelectWhereAssociations",
		////		from o in Select.From<Order>("o")
		////		where o.Customer.City == "Seattle" && o.Customer.Phone != "555 555 5555"
		////		select o);
		////}

		////[TestMethod]
		////public void TestSelectWhereAssociationTwice()
		////{
		////	TestQuery2(
		////		"TestSelectWhereAssociationTwice",
		////		from o in Select.From<Order>("o")
		////		where o.Customer.City == "Seattle" && o.Customer.Phone != "555 555 5555"
		////		select o);
		////}

		////[TestMethod]
		////public void TestSelectAssociation()
		////{
		////	TestQuery2(
		////		"TestSelectAssociation",
		////		from o in Select.From<Order>("o")
		////		select o.Customer);
		////}

		////[TestMethod]
		////public void TestSelectAssociations()
		////{
		////	TestQuery2(
		////		"TestSelectAssociations",
		////		from o in Select.From<Order>("o")
		////		select new { A = o.Customer, B = o.Customer });
		////}

		////[TestMethod]
		////public void TestSelectAssociationsWhereAssociations()
		////{
		////	TestQuery2(
		////		"TestSelectAssociationsWhereAssociations",
		////		from o in Select.From<Order>("o")
		////		where o.Customer.City == "Seattle"
		////		where o.Customer.Phone != "555 555 5555"
		////		select new { A = o.Customer, B = o.Customer });
		////}

		////[TestMethod]
		////public void TestSingletonAssociationWithMemberAccess()
		////{
		////	TestQuery2(
		////		"TestSingletonAssociationWithMemberAccess",
		////		from o in Select.From<Order>("o")
		////		where o.Customer.City == "Seattle"
		////		where o.Customer.Phone != "555 555 5555"
		////		select new { A = o.Customer, B = o.Customer.City }
		////		);
		////}

		////[TestMethod]
		////public void TestCompareDateTimesWithDifferentNullability()
		////{
		////	DateTime today = new DateTime(2013, 1, 1);
		////	TestQuery2(
		////		"TestCompareDateTimesWithDifferentNullability",
		////		from o in Select.From<Order>("o")
		////		where o.OrderDate < today && ((DateTime?)o.OrderDate) < today
		////		select o
		////		);
		////}

		[TestMethod]
		public void TestContainsWithEmptyLocalList()
		{
			var ids = new string[0];
			TestQuery2(
				"TestContainsWithEmptyLocalList",
				from c in Select.From<Customer>("c")
				where ids.Contains(c.CustomerID)
				select c
				);
		}

		////[TestMethod]
		////public void TestContainsWithSubquery2()
		////{
		////	var custsInLondon = Select.From<Customer>("c").Where(c => c.City == "London").Columns(c => c.CustomerID);

		////	TestQuery2(
		////		"TestContainsWithSubquery2",
		////		from c in Select.From<Customer>("c")
		////		where custsInLondon.Contains(c.CustomerID)
		////		select c
		////		);
		////}

		////[TestMethod]
		////public void TestCombineQueriesDeepNesting()
		////{
		////	var custs = Select.From<Customer>("c").Where(c => c.ContactName.StartsWith("xxx"));
		////	var ords = Select.From<Order>("o").Where(o => custs.Any(c => c.CustomerID == o.CustomerID));
		////	TestQuery2(
		////		"TestCombineQueriesDeepNesting",
		////		db.OrderDetails.Where(d => ords.Any(o => o.OrderID == d.OrderID))
		////		);
		////}

		////[TestMethod]
		////public void TestLetWithSubquery()
		////{
		////	TestQuery2(
		////		"TestLetWithSubquery",
		////		from customer in Select.From<Customer>("c")
		////		let orders =
		////			from order in Select.From<Order>("o")
		////			where order.CustomerID == customer.CustomerID
		////			select order
		////		select new
		////		{
		////			Customer = customer,
		////			OrdersCount = orders.Count(),
		////		}
		////		);
		////}

		private void TestQuery2(string baseline, Statement query, bool testSQLite = true, bool testSqlServer = true)
		{
			if (testSQLite)
			{
				// Test the SQLite command builder
				var sqliteBuilder = new SQLiteCommandBuilder();
				var sqliteCommand = query.Build(mapper, sqliteBuilder);
				TestQuery2(sqliteBaselines[baseline], query, sqliteCommand, "*** SQLITE ***");
			}

			if (testSqlServer)
			{
				// Test the SQL Server command builder
				var sqlServerBuilder = new SqlServerCommandBuilder();
				var sqlServerCommand = query.Build(mapper, sqlServerBuilder);
				TestQuery2(sqlServerBaselines[baseline], query, sqlServerCommand, "*** SQL SERVER ***");
			}
		}

		private void TestQuery2(string baseline, Statement query, Command command, string provider)
		{
			string expected = TrimExtraWhiteSpace(baseline.Replace("\n\n", ") ("));
			string actual = TrimExtraWhiteSpace(command.CommandText.ToString());

			// Replace parameter references with their values so that we can check they have the correct value
			for (int i = 0; i < command.Parameters.Length; i++)
			{
				Assert.IsTrue(actual.Contains("@p" + i));
				if (command.Parameters[i] is string ||
					command.Parameters[i] is char)
				{
					actual = actual.Replace("@p" + i, "'" + command.Parameters[i].ToString() + "'");
				}
				else
				{
					actual = actual.Replace("@p" + i, command.Parameters[i].ToString());
				}
			}

			Assert.AreEqual(expected, actual, provider);
		}

		private string TrimExtraWhiteSpace(string s)
		{
			string result = s.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Trim();
			while (result.Contains("  "))
			{
				result = result.Replace("  ", " ");
			}
			result = result.Replace("( ", "(").Replace(" )", ")");
			return result;
		}
	}
}
