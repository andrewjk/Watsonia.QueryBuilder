using System;
using System.Collections.Generic;
using System.Text;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// An interface for building command text and parameters from a statement.
	/// </summary>
	public interface ICommandBuilder
	{
		/// <summary>
		/// Gets the command text.
		/// </summary>
		/// <value>
		/// The command text.
		/// </value>
		StringBuilder CommandText { get; }

		/// <summary>
		/// Gets the parameter values.
		/// </summary>
		/// <value>
		/// The parameter values.
		/// </value>
		List<object> ParameterValues { get; }

		/// <summary>
		/// Visits the statement and builds the command text and parameters.
		/// </summary>
		/// <param name="statement">The statement.</param>
		/// <param name="mapper">The mapper.</param>
		void VisitStatement(Statement statement, DatabaseMapper mapper);
	}
}
