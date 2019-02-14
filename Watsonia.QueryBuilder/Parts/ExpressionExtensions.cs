using System;
using System.Linq.Expressions;

namespace Watsonia.QueryBuilder
{
	internal static class ExpressionExtensions
	{
		public static Expression Equal(this Expression expression1, Expression expression2)
		{
			ConvertExpressions(ref expression1, ref expression2);
			return Expression.Equal(expression1, expression2);
		}

		public static Expression NotEqual(this Expression expression1, Expression expression2)
		{
			ConvertExpressions(ref expression1, ref expression2);
			return Expression.NotEqual(expression1, expression2);
		}

		public static Expression GreaterThan(this Expression expression1, Expression expression2)
		{
			ConvertExpressions(ref expression1, ref expression2);
			return Expression.GreaterThan(expression1, expression2);
		}

		public static Expression GreaterThanOrEqual(this Expression expression1, Expression expression2)
		{
			ConvertExpressions(ref expression1, ref expression2);
			return Expression.GreaterThanOrEqual(expression1, expression2);
		}

		public static Expression LessThan(this Expression expression1, Expression expression2)
		{
			ConvertExpressions(ref expression1, ref expression2);
			return Expression.LessThan(expression1, expression2);
		}

		public static Expression LessThanOrEqual(this Expression expression1, Expression expression2)
		{
			ConvertExpressions(ref expression1, ref expression2);
			return Expression.LessThanOrEqual(expression1, expression2);
		}

		public static Expression And(this Expression expression1, Expression expression2)
		{
			ConvertExpressions(ref expression1, ref expression2);
			return Expression.And(expression1, expression2);
		}

		public static Expression AndAlso(this Expression expression1, Expression expression2)
		{
			ConvertExpressions(ref expression1, ref expression2);
			return Expression.AndAlso(expression1, expression2);
		}

		public static Expression Or(this Expression expression1, Expression expression2)
		{
			ConvertExpressions(ref expression1, ref expression2);
			return Expression.Or(expression1, expression2);
		}

		public static Expression OrElse(this Expression expression1, Expression expression2)
		{
			ConvertExpressions(ref expression1, ref expression2);
			return Expression.OrElse(expression1, expression2);
		}

		public static Expression Binary(this Expression expression1, ExpressionType op, Expression expression2)
		{
			ConvertExpressions(ref expression1, ref expression2);
			return Expression.MakeBinary(op, expression1, expression2);
		}

		private static void ConvertExpressions(ref Expression expression1, ref Expression expression2)
		{
			if (expression1.Type != expression2.Type)
			{
				var isNullable1 = TypeHelper.IsNullableType(expression1.Type);
				var isNullable2 = TypeHelper.IsNullableType(expression2.Type);
				if (isNullable1 || isNullable2)
				{
					if (TypeHelper.GetNonNullableType(expression1.Type) == TypeHelper.GetNonNullableType(expression2.Type))
					{
						if (!isNullable1)
						{
							expression1 = Expression.Convert(expression1, expression2.Type);
						}
						else if (!isNullable2)
						{
							expression2 = Expression.Convert(expression2, expression1.Type);
						}
					}
				}
			}
		}
	}
}