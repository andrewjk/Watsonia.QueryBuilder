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
	public static partial class Insert
	{
		public static InsertStatement<T> Into<T>()
		{
			return new InsertStatement<T>();
		}

		public static InsertStatement<T> Value<T>(this InsertStatement<T> insert, Expression<Func<T, object>> property, object value)
		{
			insert.SetValues.Add(new FieldValue(FuncToPropertyInfo(property), value));
			return insert;
		}

		// TODO: This should go into a helper
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
