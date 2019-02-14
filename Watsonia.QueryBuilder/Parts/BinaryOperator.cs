using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// An operator that is performed on two expressions.
	/// </summary>
	public enum BinaryOperator
	{
		/// <summary>
		/// Add the expressions together.
		/// </summary>
		Add,
		/// <summary>
		/// Subtract the right expression from the left.
		/// </summary>
		Subtract,
		/// <summary>
		/// Multiply the expressions together.
		/// </summary>
		Multiply,
		/// <summary>
		/// Divide the left expression by the right.
		/// </summary>
		Divide,
		/// <summary>
		/// Divide the left expression by the right and return the remainder.
		/// </summary>
		Remainder,
		/// <summary>
		/// Perform an exclusive OR operation on the expressions.
		/// </summary>
		ExclusiveOr,
		/// <summary>
		/// Perform a left shift operation on the expressions.
		/// </summary>
		LeftShift,
		/// <summary>
		/// Perform a right shift operation on the expressions.
		/// </summary>
		RightShift,
		/// <summary>
		/// Perform a bitwise AND operation on the expressions.
		/// </summary>
		BitwiseAnd,
		/// <summary>
		/// Perform a bitwise OR operation on the expressions.
		/// </summary>
		BitwiseOr,
		/// <summary>
		/// Perform a bitwise exclusive OR operation on the expressions.
		/// </summary>
		BitwiseExclusiveOr,
		/// <summary>
		/// Perform a bitwise NOT operation on the expressions.
		/// </summary>
		BitwiseNot,
	}
}
