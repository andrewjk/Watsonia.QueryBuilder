using System;
using System.Collections.Generic;
using System.Text;

namespace Watsonia.QueryBuilder
{
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
