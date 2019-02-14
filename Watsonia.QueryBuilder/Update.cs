using System;
using System.Collections.Generic;
using System.Linq;
using Watsonia.QueryBuilder;

namespace Watsonia.QueryBuilder
{
	public static partial class Update
	{
		public static UpdateStatement Table(string tableName)
		{
			return Update.Table(new Table(tableName));
		}

		public static UpdateStatement Table(Table table)
		{
			return new UpdateStatement() { Target = table };
		}

		public static UpdateStatement Set(this UpdateStatement update, string columnName, object value)
		{
			update.SetValues.Add(new SetValue(columnName, value));
			return update;
		}

		public static UpdateStatement Where(this UpdateStatement update, bool all)
		{
			if (all)
			{
				var newCondition = new Condition();
				newCondition.Field = new ConstantPart(true);
				newCondition.Value = new ConstantPart(true);
				update.Conditions.Add(newCondition);
			}
			return update;
		}

		public static UpdateStatement Where(this UpdateStatement update, string columnName, SqlOperator op, object value)
		{
			update.Conditions.Add(new Condition(columnName, op, value));
			return update;
		}

		public static UpdateStatement WhereNot(this UpdateStatement update, string columnName, SqlOperator op, object value)
		{
			update.Conditions.Add(new Condition(columnName, op, value) { Not = true });
			return update;
		}

		public static UpdateStatement And(this UpdateStatement update, string columnName, SqlOperator op, object value)
		{
			update.Conditions.Add(new Condition(columnName, op, value) { Relationship = ConditionRelationship.And });
			return update;
		}

		public static UpdateStatement Or(this UpdateStatement update, string columnName, SqlOperator op, object value)
		{
			update.Conditions.Add(new Condition(columnName, op, value) { Relationship = ConditionRelationship.Or });
			return update;
		}
	}
}
