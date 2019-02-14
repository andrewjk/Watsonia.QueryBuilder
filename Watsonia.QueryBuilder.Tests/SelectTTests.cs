using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Watsonia.QueryBuilder.Tests
{
	[TestClass]
	public class SelectTTests : TestsBase
	{
		[TestMethod]
		public void TestSelectTStatement()
		{
			var command =
				Select.From<Customer>()
				.Columns(c => c.Name)
				.Columns(c => c.Code)
				.Columns(c => c.LicenseCount)
				.Where(c => c.Code == "HI123")
				.And(c => c.BusinessNumber == "123 456 789")
				.OrderBy(c => c.Name)
				.Build();

			// Make sure the SQL is correct
			Assert.AreEqual(
				"SELECT [Customer].[Name], [Customer].[Code], [Customer].[LicenseCount] FROM [Customer] WHERE ([Customer].[Code] = @p0 AND [Customer].[BusinessNumber] = @p1) ORDER BY [Customer].[Name]",
				TrimExtraWhiteSpace(command.CommandText));

			// Make sure the parameters are correct
			Assert.AreEqual(2, command.Parameters.Length);
			Assert.AreEqual("HI123", command.Parameters[0]);
			Assert.AreEqual("123 456 789", command.Parameters[1]);
		}
	}
}
