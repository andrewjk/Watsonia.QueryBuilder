using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Watsonia.QueryBuilder.Tests
{
	[TestClass]
	public class SelectTests : TestsBase
	{
		[TestMethod]
		public void TestSelectStatement()
		{
			var command =
				Select.From("Customer")
				.Columns("Name", "Code", "LicenseCount")
				.Where("Code", SqlOperator.Equals, "HI123")
				.And("BusinessNumber", SqlOperator.Equals, "123 456 789")
				.OrderBy("Name")
				.Build();

			// Make sure the SQL is correct
			Assert.AreEqual(
				"SELECT [Name], [Code], [LicenseCount] FROM [Customer] WHERE [Code] = @0 AND [BusinessNumber] = @1 ORDER BY [Name]",
				TrimExtraWhiteSpace(command.CommandText));

			// Make sure the parameters are correct
			Assert.AreEqual(2, command.Parameters.Length);
			Assert.AreEqual("HI123", command.Parameters[0]);
			Assert.AreEqual("123 456 789", command.Parameters[1]);
		}
	}
}
