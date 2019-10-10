using Microsoft.VisualStudio.TestTools.UnitTesting;
using Watsonia.QueryBuilder.Tests.Entities;

namespace Watsonia.QueryBuilder.Tests
{
	[TestClass]
	public class UpdateTTests : TestsBase
	{
		[TestMethod]
		public void TestUpdateTStatement()
		{
			var command =
				Update.Table<Customer>()
				.Set(c => c.Code, "HI456")
				.Set(c => c.Description, "Hi I'm a test value")
				.Set(c => c.LicenseCount, 10)
				.Where(c => c.Code == "HI123")
				.And(c => c.BusinessNumber == "123 456 789")
				.Build();

			// Make sure the SQL is correct
			Assert.AreEqual(
				"UPDATE [Customer] SET [Code] = @0, [Description] = @1, [LicenseCount] = @2 WHERE ([Customer].[Code] = @3 AND [Customer].[BusinessNumber] = @4)",
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
