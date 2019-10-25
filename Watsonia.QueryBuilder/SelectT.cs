﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Watsonia.QueryBuilder;

namespace Watsonia.QueryBuilder
{
	public static partial class Select
	{
		public static SelectStatement<T> From<T>(string alias = null)
		{
			return new SelectStatement<T>(alias);
		}

		public static SelectStatement<T> Columns<T>(this SelectStatement<T> select, Expression<Func<T, object>> property)
		{
			var field = FuncToPropertyInfo(property, true);
			if (field == null)
			{
				if (property.Body is NewExpression)
				{
					// It's a new anonymous object, so add each of its arguments
					foreach (var anonArg in ((NewExpression)property.Body).Arguments)
					{
						if (anonArg is MemberExpression mex)
						{
							select.SourceFields.Add((PropertyInfo)mex.Member);
						}
					}
				}
				else
				{
					throw new InvalidOperationException();
				}
			}
			else
			{
				select.SourceFields.Add(field);
			}
			return select;
		}

		public static SelectStatement<T> Distinct<T>(this SelectStatement<T> select)
		{
			select.IsDistinct = true;
			return select;
		}

		public static SelectStatement<T> Count<T>(this SelectStatement<T> select, Expression<Func<T, bool>> condition)
		{
			select.AggregateFields.Add(new FieldAggregate(null, AggregateType.Count));
			return select.And(condition);
		}

		public static SelectStatement<T> Count<T>(this SelectStatement<T> select)
		{
			select.AggregateFields.Add(new FieldAggregate(null, AggregateType.Count));
			return select;
		}

		public static SelectStatement<T> Sum<T>(this SelectStatement<T> select, Expression<Func<T, object>> property)
		{
			select.AggregateFields.Add(new FieldAggregate(FuncToPropertyInfo(property), AggregateType.Sum));
			return select;
		}

		public static SelectStatement<T> Skip<T>(this SelectStatement<T> select, int startIndex)
		{
			select.StartIndex = startIndex;
			return select;
		}

		public static SelectStatement<T> Take<T>(this SelectStatement<T> select, int limit)
		{
			select.Limit = limit;
			return select;
		}

		public static SelectStatement<T> Where<T>(this SelectStatement<T> select, Expression<Func<T, bool>> condition)
		{
			return select.And(condition);
		}

		public static SelectStatement<T> And<T>(this SelectStatement<T> select, Expression<Func<T, bool>> condition)
		{
			if (select.Conditions != null)
			{
				var combined = select.Conditions.Body.AndAlso(condition.Body);
				combined = AnonymousParameterReplacer.Replace(combined, condition.Parameters);
				select.Conditions = Expression.Lambda<Func<T, bool>>(combined, condition.Parameters);
			}
			else
			{
				select.Conditions = condition;
			}
			return select;
		}

		public static SelectStatement<T> Or<T>(this SelectStatement<T> select, Expression<Func<T, bool>> condition)
		{
			if (select.Conditions != null)
			{
				var combined = select.Conditions.Body.OrElse(condition.Body);
				combined = AnonymousParameterReplacer.Replace(combined, condition.Parameters);
				select.Conditions = Expression.Lambda<Func<T, bool>>(combined, condition.Parameters);
			}
			else
			{
				select.Conditions = condition;
			}
			return select;
		}

		public static SelectStatement<T> OrderBy<T>(this SelectStatement<T> select, Expression<Func<T, object>> property)
		{
			select.OrderByFields.Add(new FieldOrder(FuncToPropertyInfo(property), OrderDirection.Ascending));
			return select;
		}

		public static SelectStatement<T> OrderByDescending<T>(this SelectStatement<T> select, Expression<Func<T, object>> property)
		{
			select.OrderByFields.Add(new FieldOrder(FuncToPropertyInfo(property), OrderDirection.Descending));
			return select;
		}

		public static SelectStatement<T> GroupBy<T>(this SelectStatement<T> select, Expression<Func<T, object>> property)
		{
			select.GroupByFields.Add(FuncToPropertyInfo(property));
			return select;
		}

		private static PropertyInfo FuncToPropertyInfo<T>(Expression<Func<T, object>> selector, bool returnNull = false)
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

			// HACK: Yes, this is ugly!
			if (returnNull)
			{
				return null;
			}
			else
			{
				throw new InvalidOperationException();
			}
		}
	}
}
