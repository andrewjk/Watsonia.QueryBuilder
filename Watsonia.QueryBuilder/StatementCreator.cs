using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Parsing.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// Converts QueryModels into Select statements for passing to the database.
	/// </summary>
	public class StatementCreator : QueryModelVisitorBase
	{
		private DatabaseMapper Configuration { get; set; }

        private bool AliasTables
        {
            get;
            set;
        }

		private SelectStatement SelectStatement { get; set; }

		private StatementCreator(DatabaseMapper mapper, bool aliasTables)
		{
			this.Configuration = mapper;
            this.AliasTables = aliasTables;
			this.SelectStatement = new SelectStatement();
		}

		public static SelectStatement Visit(QueryModel queryModel, DatabaseMapper mapper, bool aliasTables)
		{
			var visitor = new StatementCreator(mapper, aliasTables);
			queryModel.Accept(visitor);
			return visitor.SelectStatement;
		}

		public static ConditionCollection VisitStatementConditions<T>(Expression<Func<T, bool>> conditions, DatabaseMapper mapper, bool aliasTables)
		{
			// Build a new query
			var queryParser = QueryParser.CreateDefault();
			var queryExecutor = new StatementExecutor();
			var query = new StatementQuery<T>(queryParser, queryExecutor);

			// Create an expression to select from the query with the conditions so that we have a sequence for Re-Linq to parse
			MethodCallExpression expression = Expression.Call(
				typeof(Queryable),
				"Where",
				new Type[] { query.ElementType },
				query.Expression,
				conditions);

			// Parse the expression with Re-Linq
			QueryModel queryModel = queryParser.GetParsedQuery(expression);

			// Get the conditions from the query model
			var visitor = new StatementCreator(mapper, aliasTables);
			visitor.SelectStatement = new SelectStatement();
			queryModel.Accept(visitor);
			return visitor.SelectStatement.Conditions;
		}

		public override void VisitSelectClause(SelectClause selectClause, QueryModel queryModel)
		{
			if (selectClause.Selector.NodeType != ExpressionType.Extension)
			{
				StatementPart fields = StatementPartCreator.Visit(queryModel, selectClause.Selector, this.Configuration, this.AliasTables);
				this.SelectStatement.SourceFields.Add((SourceExpression)fields);
			}

			base.VisitSelectClause(selectClause, queryModel);
		}

		public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
		{
			if (this.Configuration.IsFunction(fromClause.ItemType))
			{
				string functionName = this.Configuration.GetFunctionName(fromClause.ItemType);
				string alias = fromClause.ItemName.Replace("<generated>", "g");
				this.SelectStatement.Source = new UserDefinedFunction(functionName) { Alias = alias };
			}
			else
			{
				string tableName = this.Configuration.GetTableName(fromClause.ItemType);
				string alias = fromClause.ItemName.Replace("<generated>", "g");
				this.SelectStatement.Source = new Table(tableName) { Alias = alias };
			}
			base.VisitMainFromClause(fromClause, queryModel);
		}

		public override void VisitJoinClause(JoinClause joinClause, QueryModel queryModel, int index)
		{
			// TODO: This seems heavy...
			// TODO: And like it's only going to deal with certain types of joins
			var table = (Table)StatementPartCreator.Visit(queryModel, joinClause.InnerSequence, this.Configuration, this.AliasTables);
            table.Alias = joinClause.ItemName.Replace("<generated>", "g");
			var leftColumn = (SourceExpression)StatementPartCreator.Visit(queryModel, joinClause.OuterKeySelector, this.Configuration, this.AliasTables);
			var rightColumn = (SourceExpression)StatementPartCreator.Visit(queryModel, joinClause.InnerKeySelector, this.Configuration, this.AliasTables);

			if (leftColumn is FieldCollection && rightColumn is FieldCollection)
			{
				var leftColumnCollection = (FieldCollection)leftColumn;
				var rightColumnCollection = (FieldCollection)rightColumn;
				var joinConditions = new ConditionCollection();
				for (int i = 0; i < leftColumnCollection.Count; i++)
				{
					joinConditions.Add(new Condition(leftColumnCollection[i], SqlOperator.Equals, rightColumnCollection[i]));
				}
				this.SelectStatement.SourceJoins.Add(new Join(table, joinConditions) { JoinType = JoinType.Left });
			}
			else
			{
				this.SelectStatement.SourceJoins.Add(new Join(table, leftColumn, rightColumn) { JoinType = JoinType.Left });
			}

			base.VisitJoinClause(joinClause, queryModel, index);
		}

		public override void VisitOrdering(Ordering ordering, QueryModel queryModel, OrderByClause orderByClause, int index)
		{
			Column column = (Column)StatementPartCreator.Visit(queryModel, ordering.Expression, this.Configuration, this.AliasTables);

			switch (ordering.OrderingDirection)
			{
				case OrderingDirection.Asc:
				{
					this.SelectStatement.OrderByFields.Add(new OrderByExpression(column, OrderDirection.Ascending));
					break;
				}
				case OrderingDirection.Desc:
				{
					this.SelectStatement.OrderByFields.Add(new OrderByExpression(column, OrderDirection.Descending));
					break;
				}
				default:
				{
					throw new InvalidOperationException($"Invalid ordering direction: {ordering.OrderingDirection}");
				}
			}

			base.VisitOrdering(ordering, queryModel, orderByClause, index);
		}

		public override void VisitResultOperator(ResultOperatorBase resultOperator, QueryModel queryModel, int index)
		{
			if (resultOperator is AnyResultOperator)
			{
				this.SelectStatement.IsAny = true;
				this.SelectStatement.IsAggregate = true;
				return;
			}

			if (resultOperator is AllResultOperator)
			{
				this.SelectStatement.IsAll = true;
				this.SelectStatement.IsAggregate = true;
				var predicate = ((AllResultOperator)resultOperator).Predicate;
				if (predicate != null)
				{
					VisitPredicate(predicate, queryModel);
				}
				return;
			}

			if (resultOperator is ContainsResultOperator)
			{
				this.SelectStatement.IsContains = true;
				this.SelectStatement.IsAggregate = true;
				var item = ((ContainsResultOperator)resultOperator).Item;
				if (item != null && item.NodeType == ExpressionType.Constant)
				{
					this.SelectStatement.ContainsItem = new ConstantPart(((ConstantExpression)item).Value);
				}
				return;
			}

			if (resultOperator is FirstResultOperator)
			{
				this.SelectStatement.Limit = 1;
				return;
			}

			if (resultOperator is LastResultOperator)
			{
				this.SelectStatement.Limit = 1;
				foreach (OrderByExpression orderBy in this.SelectStatement.OrderByFields)
				{
					orderBy.Direction = (orderBy.Direction == OrderDirection.Ascending) ? OrderDirection.Descending : OrderDirection.Ascending;
				}
				return;
			}

			if (resultOperator is CountResultOperator || resultOperator is LongCountResultOperator)
			{
				// Throw an exception if there is more than one field
				if (this.SelectStatement.SourceFields.Count > 1)
				{
					throw new InvalidOperationException("can't count multiple fields");
				}

				// Count the first field
				if (this.SelectStatement.SourceFields.Count == 0)
				{
					this.SelectStatement.SourceFields.Add(new Aggregate(AggregateType.Count, new Column("*")));
				}
				else
				{
					this.SelectStatement.SourceFields[0] = new Aggregate(AggregateType.Count, (Field)this.SelectStatement.SourceFields[0]);
				}

				this.SelectStatement.IsAggregate = true;

				return;
			}

			if (resultOperator is SumResultOperator)
			{
				// Throw an exception if there is not one field
				if (this.SelectStatement.SourceFields.Count != 1)
				{
					throw new InvalidOperationException("can't sum multiple or no fields");
				}

				// Sum the first field
				this.SelectStatement.SourceFields[0] = new Aggregate(AggregateType.Sum, (Field)this.SelectStatement.SourceFields[0]);
				this.SelectStatement.IsAggregate = true;

				return;
			}

			if (resultOperator is MinResultOperator)
			{
				// Throw an exception if there is not one field
				if (this.SelectStatement.SourceFields.Count != 1)
				{
					throw new InvalidOperationException("can't min multiple or no fields");
				}

				// Sum the first field
				this.SelectStatement.SourceFields[0] = new Aggregate(AggregateType.Min, (Field)this.SelectStatement.SourceFields[0]);
				this.SelectStatement.IsAggregate = true;

				return;
			}

			if (resultOperator is MaxResultOperator)
			{
				// Throw an exception if there is not one field
				if (this.SelectStatement.SourceFields.Count != 1)
				{
					throw new InvalidOperationException("can't max multiple or no fields");
				}

				// Sum the first field
				this.SelectStatement.SourceFields[0] = new Aggregate(AggregateType.Max, (Field)this.SelectStatement.SourceFields[0]);
				this.SelectStatement.IsAggregate = true;

				return;
			}

			if (resultOperator is AverageResultOperator)
			{
				// Throw an exception if there is not one field
				if (this.SelectStatement.SourceFields.Count != 1)
				{
					throw new InvalidOperationException("can't average multiple or no fields");
				}

				// Sum the first field
				this.SelectStatement.SourceFields[0] = new Aggregate(AggregateType.Average, (Field)this.SelectStatement.SourceFields[0]);
				this.SelectStatement.IsAggregate = true;

				return;
			}

			if (resultOperator is DistinctResultOperator)
			{
				this.SelectStatement.IsDistinct = true;
				return;
			}

			if (resultOperator is TakeResultOperator)
			{
				var exp = ((TakeResultOperator)resultOperator).Count;
				if (exp.NodeType == ExpressionType.Constant)
				{
					this.SelectStatement.Limit = (int)((ConstantExpression)exp).Value;
				}
				else
				{
					throw new NotSupportedException("Currently not supporting methods or variables in the Skip or Take clause.");
				}
				return;
			}

			if (resultOperator is SkipResultOperator)
			{
				var exp = ((SkipResultOperator)resultOperator).Count;
				if (exp.NodeType == ExpressionType.Constant)
				{
					this.SelectStatement.StartIndex = (int)((ConstantExpression)exp).Value;
				}
				else
				{
					throw new NotSupportedException("Currently not supporting methods or variables in the Skip or Take clause.");
				}
				return;
			}

			if (resultOperator is ReverseResultOperator)
			{
				foreach (OrderByExpression orderBy in this.SelectStatement.OrderByFields)
				{
					orderBy.Direction = (orderBy.Direction == OrderDirection.Ascending) ? OrderDirection.Descending : OrderDirection.Ascending;
				}
				return;
			}

			base.VisitResultOperator(resultOperator, queryModel, index);
		}

		public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
		{
			VisitPredicate(whereClause.Predicate, queryModel);

			base.VisitWhereClause(whereClause, queryModel, index);
		}

		private void VisitPredicate(Expression predicate, QueryModel queryModel)
		{
			StatementPart whereStatement = StatementPartCreator.Visit(queryModel, predicate, this.Configuration, this.AliasTables);
			ConditionExpression condition;
			if (whereStatement is ConditionExpression)
			{
				condition = (ConditionExpression)whereStatement;
			}
			else if (whereStatement is UnaryOperation && ((UnaryOperation)whereStatement).Expression is ConditionExpression)
			{
				condition = (ConditionExpression)((UnaryOperation)whereStatement).Expression;
			}
			else if (whereStatement is UnaryOperation && ((UnaryOperation)whereStatement).Expression is Column)
			{
				var unary = (UnaryOperation)whereStatement;
				var column = (Column)unary.Expression;
				condition = new Condition(column, SqlOperator.Equals, new ConstantPart(unary.Operator != UnaryOperator.Not));
			}
			else if (whereStatement is ConstantPart && ((ConstantPart)whereStatement).Value is bool)
			{
				bool value = (bool)((ConstantPart)whereStatement).Value;
				condition = new Condition() { Field = new ConstantPart(value), Operator = SqlOperator.Equals, Value = new ConstantPart(true) };
			}
			else if (whereStatement is Column && ((Column)whereStatement).PropertyType == typeof(bool))
			{
				condition = new Condition((Column)whereStatement, SqlOperator.Equals, new ConstantPart(true));
			}
			else
			{
				throw new InvalidOperationException();
			}
			this.SelectStatement.Conditions.Add(condition);
		}
	}
}
