using System;
using System.Collections.Generic;
using System.Text;

namespace Watsonia.QueryBuilder.SqlServer
{
	/// <summary>
	/// Builds command text and parameters from a statement for use in an Sql Server database.
	/// </summary>
	/// <seealso cref="Watsonia.QueryBuilder.SqlCommandBuilder" />
	public class SqlServerCommandBuilder : SqlCommandBuilder
	{
		protected override void VisitLimitAtStart(SelectStatement select)
		{
			this.CommandText.Append("TOP (");
			this.CommandText.Append(select.Limit);
			this.CommandText.Append(") ");
		}
	}
}
