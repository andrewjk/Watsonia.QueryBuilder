using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// A dummy implementation of QueryableBase for visiting statement conditions e.g. in Delete.Where.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Remotion.Linq.QueryableBase{T}" />
	internal class StatementQuery<T> : QueryableBase<T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="StatementQuery{T}"/> class.
		/// </summary>
		/// <param name="queryParser">The <see cref="T:Remotion.Linq.Parsing.Structure.IQueryParser" /> used to parse queries. Specify an instance of
		/// <see cref="T:Remotion.Linq.Parsing.Structure.QueryParser" /> for default behavior. See also <see cref="M:Remotion.Linq.Parsing.Structure.QueryParser.CreateDefault" />.</param>
		/// <param name="executor">The <see cref="T:Remotion.Linq.IQueryExecutor" /> used to execute the query represented by this <see cref="T:Remotion.Linq.QueryableBase`1" />.</param>
		public StatementQuery(IQueryParser queryParser, IQueryExecutor executor)
			: base(new DefaultQueryProvider(typeof(StatementQuery<>), queryParser, executor))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StatementQuery" /> class.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="expression">The expression.</param>
		public StatementQuery(IQueryProvider provider, Expression expression)
			: base(provider, expression)
		{
		}
	}
}
