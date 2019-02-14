using System.Collections.Generic;
using Watsonia.QueryBuilder;
using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public static partial class Delete
	{
		public static DeleteStatement From(string tableName)
		{
			return Delete.From(new Table(tableName));
		}

		public static DeleteStatement From(Table table)
		{
			return new DeleteStatement() { Target = table };
		}

		//public static Delete<T> From<T>()
		//{
		//	return new Delete<T>();
		//}

		public static DeleteStatement Where(this DeleteStatement delete, bool all)
		{
			if (all)
			{
				var newCondition = new Condition();
				newCondition.Field = new ConstantPart(true);
				newCondition.Value = new ConstantPart(true);
				delete.Conditions.Add(newCondition);
			}
			return delete;
		}

		public static DeleteStatement Where(this DeleteStatement delete, string columnName, SqlOperator op, object value)
		{
			delete.Conditions.Add(new Condition(columnName, op, value));
			return delete;
		}

		public static DeleteStatement WhereNot(this DeleteStatement delete, string columnName, SqlOperator op, object value)
		{
			delete.Conditions.Add(new Condition(columnName, op, value) { Not = true });
			return delete;
		}

		public static DeleteStatement And(this DeleteStatement delete, string columnName, SqlOperator op, object value)
		{
			delete.Conditions.Add(new Condition(columnName, op, value) { Relationship = ConditionRelationship.And });
			return delete;
		}

		public static DeleteStatement Or(this DeleteStatement delete, string columnName, SqlOperator op, object value)
		{
			delete.Conditions.Add(new Condition(columnName, op, value) { Relationship = ConditionRelationship.Or });
			return delete;
		}
	}
}
