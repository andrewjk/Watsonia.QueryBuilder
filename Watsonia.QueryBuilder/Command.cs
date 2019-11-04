using System;
using System.Collections.Generic;
using System.Text;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// Contains command text and parameters for running a statement against a database.
	/// </summary>
	public class Command
	{
		/// <summary>
		/// Gets the statement that this command was built from.
		/// </summary>
		/// <value>
		/// The statement.
		/// </value>
		public Statement Statement { get; }

		/// <summary>
		/// Gets the command text.
		/// </summary>
		/// <value>
		/// The command text.
		/// </value>
		public string CommandText { get; }

		/// <summary>
		/// Gets the parameters.
		/// </summary>
		/// <value>
		/// The parameters.
		/// </value>
		public IList<object> Parameters { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Command"/> class.
		/// </summary>
		/// <param name="statement">The statement that this command was built from.</param>
		/// <param name="commandText">The command text.</param>
		/// <param name="parameters">The parameters.</param>
		public Command(Statement statement, string commandText, object[] parameters)
		{
			this.Statement = statement;
			this.CommandText = commandText;
			this.Parameters = parameters;
		}
	}
}
