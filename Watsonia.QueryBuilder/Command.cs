using System;
using System.Collections.Generic;
using System.Text;

namespace Watsonia.QueryBuilder
{
	public class Command
	{
		public Statement Statement { get; }

		public string CommandText { get; }

		public IList<object> Parameters { get; }

		public Command(Statement statement, string commandText, object[] parameters)
		{
			this.Statement = statement;
			this.CommandText = commandText;
			this.Parameters = parameters;
		}
	}
}
