using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Watsonia.QueryBuilder;

namespace Watsonia.QueryBuilder
{
	public static partial class Update
	{
		public static UpdateStatement<T> Table<T>()
		{
			return new UpdateStatement<T>() { Target = typeof(T) };
		}

		public static UpdateStatement<T> Set<T>(this UpdateStatement<T> update, Expression<Func<T, object>> property, object value)
		{
			update.SetValues.Add(new Tuple<PropertyInfo, object>(FuncToPropertyInfo(property), value));
			return update;
		}

		public static UpdateStatement<T> Where<T>(this UpdateStatement<T> update, Expression<Func<T, bool>> condition)
		{
			update.Conditions = condition;
			return update;
		}

		public static UpdateStatement<T> And<T>(this UpdateStatement<T> update, Expression<Func<T, bool>> condition)
		{
			var combined = update.Conditions.Body.AndAlso(condition.Body);
			combined = AnonymousParameterReplacer.Replace(combined, condition.Parameters);
			update.Conditions = Expression.Lambda<Func<T, bool>>(combined, condition.Parameters);
			return update;
		}

		public static UpdateStatement<T> Or<T>(this UpdateStatement<T> update, Expression<Func<T, bool>> condition)
		{
			var combined = update.Conditions.Body.OrElse(condition.Body);
			combined = AnonymousParameterReplacer.Replace(combined, condition.Parameters);
			update.Conditions = Expression.Lambda<Func<T, bool>>(combined, condition.Parameters);
			return update;
		}

		private static PropertyInfo FuncToPropertyInfo<T>(Expression<Func<T, object>> selector)
		{
			if (selector.Body is MemberExpression mex)
			{
				return (PropertyInfo)mex.Member;
			}
			else if (selector.Body is UnaryExpression uex) // Throw away Converts
			{
				if (uex.Operand is MemberExpression omex)
				{
					return (PropertyInfo)omex.Member;
				}
			}

			throw new InvalidOperationException();
		}
	}
}
