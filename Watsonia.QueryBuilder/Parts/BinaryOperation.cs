using System;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// An operation with a binary operator e.g. 1 + 2.
	/// </summary>
	public sealed class BinaryOperation : Field
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
				return StatementPartType.BinaryOperation;
			}
		}

		/// <summary>
		/// Gets or sets the expression on the left of the operator.
		/// </summary>
		/// <value>
		/// The left expression.
		/// </value>
		public SourceExpression Left
		{
			get;
			internal set;
		}

		/// <summary>
		/// Gets or sets the operator.
		/// </summary>
		/// <value>
		/// The operator.
		/// </value>
		public BinaryOperator Operator
		{
			get;
			internal set;
		}

		private string OperatorString
		{
			get
			{
				switch (this.Operator)
				{
					case BinaryOperator.Add:
					{
						return "+";
					}
					case BinaryOperator.Subtract:
					{
						return "-";
					}
					case BinaryOperator.Multiply:
					{
						return "*";
					}
					case BinaryOperator.Divide:
					{
						return "/";
					}
					case BinaryOperator.Remainder:
					{
						return "%";
					}
					case BinaryOperator.ExclusiveOr:
					{
						return "^";
					}
					case BinaryOperator.LeftShift:
					{
						return "<<";
					}
					case BinaryOperator.RightShift:
					{
						return ">>";
					}
					case BinaryOperator.BitwiseAnd:
					{
						return "&";
					}
					case BinaryOperator.BitwiseOr:
					{
						return "|";
					}
					case BinaryOperator.BitwiseExclusiveOr:
					{
						return "^";
					}
					default:
					{
						throw new InvalidOperationException("Invalid Operator: " + this.Operator);
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the expression on the right of the operator.
		/// </summary>
		/// <value>
		/// The right expression.
		/// </value>
		public SourceExpression Right
		{
			get;
			internal set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryOperation" /> class.
		/// </summary>
		internal BinaryOperation()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryOperation" /> class.
		/// </summary>
		/// <param name="left">The expression on the left of the operator.</param>
		/// <param name="op">The operator.</param>
		/// <param name="right">The expression on the right of the operator.</param>
		public BinaryOperation(SourceExpression left, BinaryOperator op, SourceExpression right)
		{
			this.Left = left;
			this.Operator = op;
			this.Right = right;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return "(" + this.Left.ToString() + " " + this.OperatorString + " " + this.Right.ToString() + ")";
		}
	}
}
