using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Remotion.Linq;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// A dummy implementation of IQueryExecutor for visiting statement conditions e.g. in Delete.Where.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Remotion.Linq.IQueryExecutor" />
	internal class StatementExecutor<T> : IQueryExecutor
	{
		////internal SelectStatement BuildSelectStatement(QueryModel queryModel)
		////{
		////	// Add joins for fields in tables that haven't been joined explicitly
		////	// e.g. when using something like DB.Query<T>().Where(x => x.Item.Property == y)
		////	SelectSourceExpander.Visit(queryModel, this.Database, this.Database.Configuration);

		////	// Create the select statement
		////	SelectStatement select = SelectStatementCreator.Visit(queryModel, this.Database.Configuration, true);

		////	// Add include paths if necessary
		////	if (!select.IsAggregate)
		////	{
		////		select.IncludePaths.AddRange(this.Query.IncludePaths);
		////	}

		////	// Add parameters if the source is a user-defined function
		////	if (select.Source.PartType == StatementPartType.UserDefinedFunction)
		////	{
		////		var function = (UserDefinedFunction)select.Source;
		////		function.Parameters.AddRange(this.Query.Parameters);
		////	}

		////	// Check whether we need to expand fields (if the select has no fields)
		////	// This will avoid the case where selecting fields from multiple tables with non-unique field
		////	// names (e.g. two tables with an ID field) fills the object with the wrong value
		////	if (select.SourceFields.Count == 0)
		////	{
		////		SelectFieldExpander.Visit(queryModel, select, this.Database.Configuration);
		////	}

		////	return select;
		////}

		/// <summary>
		/// Executes the given <paramref name="queryModel" /> as a collection query, i.e. as a query returning objects of type <typeparamref name="T" />.
		/// The query does not end with a scalar result operator, but it can end with a single result operator, for example
		/// <see cref="T:Remotion.Linq.Clauses.ResultOperators.SingleResultOperator" /> or <see cref="T:Remotion.Linq.Clauses.ResultOperators.FirstResultOperator" />. In such a case, the returned enumerable must yield exactly
		/// one object (or none if the last result operator allows empty result sets).
		/// </summary>
		/// <typeparam name="T">The type of the items returned by the query.</typeparam>
		/// <param name="queryModel">The <see cref="T:Remotion.Linq.QueryModel" /> representing the query to be executed. Analyze this via an
		/// <see cref="T:Remotion.Linq.IQueryModelVisitor" />.</param>
		/// <returns>
		/// A scalar value of type <typeparamref name="T" /> that represents the query's result.
		/// </returns>
		/// <exception cref="System.InvalidOperationException"></exception>
		public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
		{
			throw new InvalidOperationException();
		}

		/// <summary>
		/// Executes the given <paramref name="queryModel" /> as a scalar query, i.e. as a query returning a scalar value of type <typeparamref name="T" />.
		/// The query ends with a scalar result operator, for example a <see cref="T:Remotion.Linq.Clauses.ResultOperators.CountResultOperator" /> or a <see cref="T:Remotion.Linq.Clauses.ResultOperators.SumResultOperator" />.
		/// </summary>
		/// <typeparam name="T">The type of the scalar value returned by the query.</typeparam>
		/// <param name="queryModel">The <see cref="T:Remotion.Linq.QueryModel" /> representing the query to be executed. Analyze this via an
		/// <see cref="T:Remotion.Linq.IQueryModelVisitor" />.</param>
		/// <returns>
		/// A scalar value of type <typeparamref name="T" /> that represents the query's result.
		/// </returns>
		/// <exception cref="System.InvalidOperationException"></exception>
		/// <remarks>
		/// The difference between <see cref="M:Remotion.Linq.IQueryExecutor.ExecuteSingle``1(Remotion.Linq.QueryModel,System.Boolean)" /> and <see cref="M:Remotion.Linq.IQueryExecutor.ExecuteScalar``1(Remotion.Linq.QueryModel)" /> is in the kind of object that is returned.
		/// <see cref="M:Remotion.Linq.IQueryExecutor.ExecuteSingle``1(Remotion.Linq.QueryModel,System.Boolean)" /> is used when a query that would otherwise return a collection result set should pick a single value from the
		/// set, for example the first, last, minimum, maximum, or only value in the set. <see cref="M:Remotion.Linq.IQueryExecutor.ExecuteScalar``1(Remotion.Linq.QueryModel)" /> is used when a value is
		/// calculated or aggregated from all the values in the collection result set. This applies to, for example, item counts, average calculations,
		/// checks for the existence of a specific item, and so on.
		/// </remarks>
		public T ExecuteScalar<T>(QueryModel queryModel)
		{
			throw new InvalidOperationException();
		}

		/// <summary>
		/// Executes the given <paramref name="queryModel" /> as a single object query, i.e. as a query returning a single object of type
		/// <typeparamref name="T" />.
		/// The query ends with a single result operator, for example a <see cref="T:Remotion.Linq.Clauses.ResultOperators.FirstResultOperator" /> or a <see cref="T:Remotion.Linq.Clauses.ResultOperators.SingleResultOperator" />.
		/// </summary>
		/// <typeparam name="T">The type of the single value returned by the query.</typeparam>
		/// <param name="queryModel">The <see cref="T:Remotion.Linq.QueryModel" /> representing the query to be executed. Analyze this via an
		/// <see cref="T:Remotion.Linq.IQueryModelVisitor" />.</param>
		/// <param name="returnDefaultWhenEmpty">If <see langword="true" />, the executor must return a default value when its result set is empty;
		/// if <see langword="false" />, it should throw an <see cref="T:System.InvalidOperationException" /> when its result set is empty.</param>
		/// <returns>
		/// A single value of type <typeparamref name="T" /> that represents the query's result.
		/// </returns>
		/// <exception cref="System.InvalidOperationException"></exception>
		/// <remarks>
		/// The difference between <see cref="M:Remotion.Linq.IQueryExecutor.ExecuteSingle``1(Remotion.Linq.QueryModel,System.Boolean)" /> and <see cref="M:Remotion.Linq.IQueryExecutor.ExecuteScalar``1(Remotion.Linq.QueryModel)" /> is in the kind of object that is returned.
		/// <see cref="M:Remotion.Linq.IQueryExecutor.ExecuteSingle``1(Remotion.Linq.QueryModel,System.Boolean)" /> is used when a query that would otherwise return a collection result set should pick a single value from the
		/// set, for example the first, last, minimum, maximum, or only value in the set. <see cref="M:Remotion.Linq.IQueryExecutor.ExecuteScalar``1(Remotion.Linq.QueryModel)" /> is used when a value is
		/// calculated or aggregated from all the values in the collection result set. This applies to, for example, item counts, average calculations,
		/// checks for the existence of a specific item, and so on.
		/// </remarks>
		public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
		{
			throw new InvalidOperationException();
		}
	}
}
