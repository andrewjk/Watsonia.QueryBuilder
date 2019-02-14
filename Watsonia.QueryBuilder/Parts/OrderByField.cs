using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// An expression that is used to order a select statement.
	/// </summary>
	public sealed class OrderByExpression : SourceExpression
	{
		/// <summary>
		/// Gets the type of the statement part.
		/// </summary>
		/// <value>
		/// The type of the part.
		/// </value>
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.OrderByField;
			}
		}

		/// <summary>
		/// Gets the expression that is ordered by.
		/// </summary>
		/// <value>
		/// The expression.
		/// </value>
		public SourceExpression Expression
		{
			get;
			internal set;
		}

		/// <summary>
		/// Gets the direction of ordering.
		/// </summary>
		/// <value>
		/// The direction.
		/// </value>
		public OrderDirection Direction
		{
			get;
			internal set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderByExpression" /> class.
		/// </summary>
		internal OrderByExpression()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderByExpression" /> class.
		/// </summary>
		/// <param name="expression">The expression that is ordered by.</param>
		public OrderByExpression(SourceExpression expression)
		{
			this.Expression = expression;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderByExpression" /> class.
		/// </summary>
		/// <param name="columnName">The name of the column to order by.</param>
		public OrderByExpression(string columnName)
		{
			this.Expression = new Column(columnName);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderByExpression" /> class.
		/// </summary>
		/// <param name="expression">The expression that is ordered by.</param>
		/// <param name="direction">The direction of ordering.</param>
		public OrderByExpression(SourceExpression expression, OrderDirection direction)
		{
			this.Expression = expression;
			this.Direction = direction;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderByExpression" /> class.
		/// </summary>
		/// <param name="columnName">The name of the column to order by.</param>
		/// <param name="direction">The direction of ordering.</param>
		public OrderByExpression(string columnName, OrderDirection direction)
		{
			this.Expression = new Column(columnName);
			this.Direction = direction;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if (this.Direction == OrderDirection.Ascending)
			{
				return this.Expression.ToString();
			}
			else
			{
				return this.Expression.ToString() + " " + this.Direction.ToString();
			}
		}
	}
}
