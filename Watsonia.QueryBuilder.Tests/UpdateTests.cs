using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Watsonia.QueryBuilder.Tests
{
	[TestClass]
	public class UpdateTests : TestsBase
	{
		[TestMethod]
		public void TestUpdateStatement()
		{
			var command =
				Update.Table("Customer")
				.Set("Code", "HI456")
				.Set("Description", "Hi I'm a test value")
				.Set("LicenseCount", 10)
				.Where("Code", SqlOperator.Equals, "HI123")
				.And("BusinessNumber", SqlOperator.Equals, "123 456 789")
				.Build();

			// Make sure the SQL is correct
			Assert.AreEqual(
				"UPDATE [Customer] SET [Code] = @0, [Description] = @1, [LicenseCount] = @2 WHERE [Code] = @3 AND [BusinessNumber] = @4",
				TrimExtraWhiteSpace(command.CommandText));

			// Make sure the parameters are correct
			Assert.AreEqual(5, command.Parameters.Count);
			Assert.AreEqual("HI456", command.Parameters[0]);
			Assert.AreEqual("Hi I'm a test value", command.Parameters[1]);
			Assert.AreEqual(10, command.Parameters[2]);
			Assert.AreEqual("HI123", command.Parameters[3]);
			Assert.AreEqual("123 456 789", command.Parameters[4]);
		}
	}
}
