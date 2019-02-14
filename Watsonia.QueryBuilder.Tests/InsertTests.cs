using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Watsonia.QueryBuilder.Tests
{
	[TestClass]
	public class InsertTests : TestsBase
	{
		[TestMethod]
		public void TestInsertStatement()
		{
			var command =
				Insert.Into("Customer")
				.Value("Code", "HI123")
				.Value("Description", "Hi I'm a test value")
				.Value("BusinessNumber", "123 456 789")
				.Value("LicenseCount", 5)
				.Build();

			// Make sure the SQL is correct
			Assert.AreEqual(
				"INSERT INTO [Customer] ([Code], [Description], [BusinessNumber], [LicenseCount]) VALUES (@p0, @p1, @p2, @p3)",
				TrimExtraWhiteSpace(command.CommandText));

			// Make sure the parameters are correct
			Assert.AreEqual(4, command.Parameters.Length);
			Assert.AreEqual("HI123", command.Parameters[0]);
			Assert.AreEqual("Hi I'm a test value", command.Parameters[1]);
			Assert.AreEqual("123 456 789", command.Parameters[2]);
			Assert.AreEqual(5, command.Parameters[3]);
		}
	}
}
