using System;
using System.Collections.Generic;
using System.Text;

namespace Watsonia.QueryBuilder
{
	public interface ICommandBuilder
	{
		StringBuilder CommandText { get; }

		List<object> ParameterValues { get; }

		void VisitStatement(Statement statement, DatabaseMapper mapper);
	}
}
