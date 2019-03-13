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
	public static partial class Delete
	{
		public static DeleteStatement<T> From<T>()
		{
			return new DeleteStatement<T>() { Target = typeof(T) };
		}

		public static DeleteStatement<T> Where<T>(this DeleteStatement<T> delete, Expression<Func<T, bool>> condition)
		{
			delete.Conditions = condition;
			return delete;
		}

		public static DeleteStatement<T> And<T>(this DeleteStatement<T> delete, Expression<Func<T, bool>> condition)
		{
			var combined = delete.Conditions.Body.AndAlso(condition.Body);
			combined = AnonymousParameterReplacer.Replace(combined, condition.Parameters);
			delete.Conditions = Expression.Lambda<Func<T, bool>>(combined, condition.Parameters);
			return delete;
		}

		public static DeleteStatement<T> Or<T>(this DeleteStatement<T> delete, Expression<Func<T, bool>> condition)
		{
			var combined = delete.Conditions.Body.OrElse(condition.Body);
			combined = AnonymousParameterReplacer.Replace(combined, condition.Parameters);
			delete.Conditions = Expression.Lambda<Func<T, bool>>(combined, condition.Parameters);
			return delete;
		}
	}
}
