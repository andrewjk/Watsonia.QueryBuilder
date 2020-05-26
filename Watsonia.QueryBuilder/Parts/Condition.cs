using System;
using System.Collections.Generic;
using System.Collections;
using Watsonia.QueryBuilder;
using System.Text;

namespace Watsonia.QueryBuilder
{
	public class Condition : ConditionExpression
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.Condition;
			}
		}

		public StatementPart Field { get; set; }

		public SqlOperator Operator { get; set; }

		public StatementPart Value { get; set; }

		internal Condition()
		{
		}

		// TODO: Make these static i.e. public static Condition Where(...) ??
		public Condition(string fieldName, SqlOperator op, object value)
		{
			this.Field = new Column(fieldName);
			this.Operator = op;
			AddValue(value);
		}

		public Condition(string tableName, string fieldName, SqlOperator op, object value)
		{
			this.Field = new Column(tableName, fieldName);
			this.Operator = op;
			AddValue(value);
		}

		public Condition(SourceExpression column, SqlOperator op, object value)
		{
			this.Field = column;
			this.Operator = op;
			AddValue(value);
		}

		public static Condition Where(string fieldName, SqlOperator op, object value)
		{
			return new Condition(fieldName, op, value);
		}

		public static Condition Where(string tableName, string fieldName, SqlOperator op, object value)
		{
			return new Condition(tableName, fieldName, op, value);
		}

		public static Condition Or(string fieldName, SqlOperator op, object value)
		{
			return new Condition(fieldName, op, value) { Relationship = ConditionRelationship.Or };
		}

		public static Condition Or(string tableName, string fieldName, SqlOperator op, object value)
		{
			return new Condition(tableName, fieldName, op, value) { Relationship = ConditionRelationship.Or };
		}

		public static Condition And(string fieldName, SqlOperator op, object value)
		{
			return new Condition(fieldName, op, value) { Relationship = ConditionRelationship.And };
		}

		public static Condition And(string tableName, string fieldName, SqlOperator op, object value)
		{
			return new Condition(tableName, fieldName, op, value) { Relationship = ConditionRelationship.And };
		}

		private void AddValue(object value)
		{
			if (value == null)
			{
				this.Value = new ConstantPart(null);
				return;
			}

			//if (value is IEnumerable && !(value is string))
			//{
			//	foreach (object subval in (IEnumerable)value)
			//	{
			//		if (subval is StatementPart)
			//		{
			//			this.Value.Add((StatementPart)subval);
			//		}
			//		else
			//		{
			//			this.Value.Add(new ConstantPart(subval));
			//		}
			//	}
			//}
			//else
			//{
			if (value is StatementPart statementPartValue)
			{
				this.Value = statementPartValue;
			}
			else
			{
				this.Value = new ConstantPart(value);
			}
			//}
		}

		public override string ToString()
		{
			var b = new StringBuilder();
			if (this.Not)
			{
				b.Append("Not ");
			}
			b.Append(this.Field.ToString());
			b.Append(" ");
			b.Append(this.Operator.ToString());
			b.Append(" ");
            if (this.Value == null)
            {
                b.Append("Null");
            }
            else
            {
                b.Append(this.Value.ToString());
            }
			return b.ToString();
		}
	}
}
