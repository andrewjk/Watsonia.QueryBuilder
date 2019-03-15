using Microsoft.VisualStudio.TestTools.UnitTesting;
using Watsonia.QueryBuilder.Tests.Entities;

namespace Watsonia.QueryBuilder.Tests
{
	[TestClass]
	public class DeleteTTests : TestsBase
	{
		[TestMethod]
		public void TestDeleteTStatement()
		{
			var command =
				Delete.From<Customer>()
				.Where(c => c.Code == "HI123")
				.And(c => c.LicenseCount == 10)
				.Build();

			// Make sure the SQL is correct
			Assert.AreEqual(
				"DELETE FROM [Customer] WHERE ([Customer].[Code] = @p0 AND [Customer].[LicenseCount] = @p1)",
				TrimExtraWhiteSpace(command.CommandText));

			// Make sure the parameters are correct
			Assert.AreEqual(2, command.Parameters.Length);
			Assert.AreEqual("HI123", command.Parameters[0]);
			Assert.AreEqual(10, command.Parameters[1]);
		}
	}
}
