using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// Converts an expression to the supplied type.
	/// </summary>
	public sealed class ConvertFunction : StatementPart
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
				return StatementPartType.ConvertFunction;
			}
		}

		/// <summary>
		/// Gets or sets the expression to convert.
		/// </summary>
		/// <value>
		/// The expression to convert.
		/// </value>
		public SourceExpression Expression
		{
			get;
			internal set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConvertFunction" /> class.
		/// </summary>
		internal ConvertFunction()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConvertFunction" /> class.
		/// </summary>
		/// <param name="expression">The expression to convert.</param>
		public ConvertFunction(SourceExpression expression)
		{
			this.Expression = expression;
		}
	}
}
