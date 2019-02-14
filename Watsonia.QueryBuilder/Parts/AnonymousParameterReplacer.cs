using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// A class for replacing parameters in an expression.
	/// </summary>
	/// <remarks>
	/// This class is used to consolidate anonymous parameters when combining lambda expressions, so
	/// that all of the parameters have the same object reference.
	/// </remarks>
	internal sealed class AnonymousParameterReplacer : ExpressionVisitor
	{
		private ReadOnlyCollection<ParameterExpression> _parameters;

		/// <summary>
		/// Prevents a default instance of the <see cref="AnonymousParameterReplacer" /> class from being created.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		private AnonymousParameterReplacer(ReadOnlyCollection<ParameterExpression> parameters)
		{
			_parameters = parameters;
		}

		/// <summary>
		/// Replaces the parameters in an expression with the supplied parameters.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns></returns>
		public static Expression Replace(Expression expression, ReadOnlyCollection<ParameterExpression> parameters)
		{
			return new AnonymousParameterReplacer(parameters).Visit(expression);
		}

		/// <summary>
		/// Visits the <see cref="T:System.Linq.Expressions.ParameterExpression" />.
		/// </summary>
		/// <param name="node">The expression to visit.</param>
		/// <returns>
		/// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
		/// </returns>
		protected override Expression VisitParameter(ParameterExpression node)
		{
			foreach (var parameter in _parameters)
			{
				if (parameter.Type == node.Type)
				{
					return parameter;
				}
			}
			return node;
		}
	}
}
