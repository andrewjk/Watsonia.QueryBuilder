using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Watsonia.QueryBuilder.Tests
{
	[TestClass]
	public class DeleteTests : TestsBase
	{
		[TestMethod]
		public void TestDeleteStatement()
		{
			var command =
				Delete.From("Customer")
				.Where("Code", SqlOperator.Equals, "HI123")
				.And("LicenseCount", SqlOperator.Equals, 10)
				.Build();

			// Make sure the SQL is correct
			Assert.AreEqual(
				"DELETE FROM [Customer] WHERE [Code] = @0 AND [LicenseCount] = @1",
				TrimExtraWhiteSpace(command.CommandText));

			// Make sure the parameters are correct
			Assert.AreEqual(2, command.Parameters.Length);
			Assert.AreEqual("HI123", command.Parameters[0]);
			Assert.AreEqual(10, command.Parameters[1]);
		}
	}
}
