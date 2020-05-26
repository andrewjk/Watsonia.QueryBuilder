using System;
using System.Linq;
using System.Text;

namespace Watsonia.QueryBuilder
{
	public sealed class Join : StatementPart
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.Join;
			}
		}

		public JoinType JoinType { get; internal set; }
		
		public StatementPart Table { get; internal set; }

		public ConditionCollection Conditions { get; } = new ConditionCollection();

		internal Join()
		{
		}

		public Join(JoinType joinType, StatementPart right, ConditionExpression condition)
		{
			this.JoinType = joinType;
			this.Table = right;
			this.Conditions.Add(condition);
		}

		public Join(string tableName, string leftTableName, string leftColumnName, string rightTableName, string rightColumnName)
		{
			this.JoinType = JoinType.Inner;
			// TODO: Fix this pug fugly syntax
			// TODO: Change field => column in all the SQL stuff?  Column if it's a column, field if it's a statement part
			//this.Left = new Table(leftTableName);
			this.Table = new Table(tableName);
			this.Conditions.Add(new Condition(leftTableName, leftColumnName, SqlOperator.Equals, new Column(rightTableName, rightColumnName)));
		}

		public Join(JoinType joinType, string tableName, string leftTableName, string leftColumnName, string rightTableName, string rightColumnName)
		{
			this.JoinType = joinType;
			// TODO: Fix this pug fugly syntax
			// TODO: Change field => column in all the SQL stuff?  Column if it's a column, field if it's a statement part
			//this.Left = new Table(leftTableName);
			this.Table = new Table(tableName);
			this.Conditions.Add(new Condition(leftTableName, leftColumnName, SqlOperator.Equals, new Column(rightTableName, rightColumnName)));
		}

		public Join(Table table, SourceExpression leftColumn, SourceExpression rightColumn)
		{
			this.JoinType = JoinType.Inner;
			this.Table = table;
			this.Conditions.Add(new Condition(leftColumn, SqlOperator.Equals, rightColumn));
		}

		public Join(JoinType joinType, Table table, SourceExpression leftColumn, SourceExpression rightColumn)
		{
			this.JoinType = joinType;
			this.Table = table;
			this.Conditions.Add(new Condition(leftColumn, SqlOperator.Equals, rightColumn));
		}

		public Join(Table table, ConditionCollection conditions)
		{
			this.JoinType = JoinType.Inner;
			this.Table = table;
			this.Conditions.AddRange(conditions);
		}

		public override string ToString()
		{
			var b = new StringBuilder();
			b.Append(this.JoinType.ToString());
			b.Append(" Join ");
			b.Append(this.Table.ToString());
			if (this.Conditions.Count > 0)
			{
				b.Append(" On ");
				b.Append(this.Conditions.ToString());
			}
			return b.ToString();
		}
	}
}
