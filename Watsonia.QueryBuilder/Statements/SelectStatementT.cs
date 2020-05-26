using System;
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
	public sealed class SelectStatement<T> : GenericStatement
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.GenericSelect;
			}
		}

		public Table<T> Source { get; internal set; }

		public List<PropertyInfo> SourceFields { get; } = new List<PropertyInfo>();

		public List<FieldAggregate> AggregateFields { get; } = new List<FieldAggregate>();

		public bool IsAny { get; set; }

		public bool IsAll { get; set; }

		public bool IsDistinct { get; set; }

		public int StartIndex { get; set; }

		public int Limit { get; set; }

		public Expression<Func<T, bool>> Conditions { get; internal set; }

		public List<FieldOrder> OrderByFields { get; internal set; } = new List<FieldOrder>();

		public List<PropertyInfo> GroupByFields { get; internal set; } = new List<PropertyInfo>();

		internal SelectStatement(string alias = null)
		{
			this.Source = new Table<T>(typeof(T), alias);
		}

		public override Statement CreateStatement(DatabaseMapper mapper)
		{
			var select = new SelectStatement();
			select.Source = new Table(mapper.GetTableName(this.Source.Type), this.Source.Alias);
			select.SourceFields.AddRange(this.SourceFields.Select(s => PropertyToSourceField(s, mapper)));
			select.SourceFields.AddRange(this.AggregateFields.Select(s => PropertyToAggregate(s, mapper)));
			select.IsAny = this.IsAny;
			select.IsAll = this.IsAll;
			select.IsDistinct = this.IsDistinct;
			select.StartIndex = this.StartIndex;
			select.Limit = this.Limit;
			if (this.Conditions != null)
			{
				// TODO: Need to handle columns from multiple tables...
				var aliasTables = !string.IsNullOrEmpty(this.Source.Alias);
				foreach (var condition in StatementCreator.VisitStatementConditions(this.Conditions, mapper, aliasTables))
				{
					select.Conditions.Add(condition);
				}
			}
			select.OrderByFields.AddRange(this.OrderByFields.Select(s => PropertyToOrderBy(s, mapper)));
			select.GroupByFields.AddRange(this.GroupByFields.Select(s => PropertyToGroupBy(s, mapper)));
			return select;
		}

		private SourceExpression PropertyToSourceField(PropertyInfo prop, DatabaseMapper mapper)
		{
			if (prop != null)
			{
				return new Column(TableNameOrAlias(mapper, prop.DeclaringType), mapper.GetColumnName(prop));
			}
			else
			{
				return new ConstantPart(null);
			}
		}

		private SourceExpression PropertyToAggregate(FieldAggregate field, DatabaseMapper mapper)
		{
			return new Aggregate(
				field.Aggregate,
				new Column(
					field.Field != null ? TableNameOrAlias(mapper, field.Field.DeclaringType) : "",
					field.Field != null ? mapper.GetColumnName(field.Field) : "*")
				);
		}

		private OrderByExpression PropertyToOrderBy(FieldOrder field, DatabaseMapper mapper)
		{
			return new OrderByExpression(
				new Column(
					TableNameOrAlias(mapper, field.Field.DeclaringType),
					mapper.GetColumnName(field.Field)), field.Direction);
		}

		private Column PropertyToGroupBy(PropertyInfo prop, DatabaseMapper mapper)
		{
			return new Column(
					 TableNameOrAlias(mapper, prop.DeclaringType),
					 mapper.GetColumnName(prop));
		}

		private string TableNameOrAlias(DatabaseMapper mapper, Type t)
		{
			if (t == this.Source.Type && !string.IsNullOrEmpty(this.Source.Alias))
			{
				return this.Source.Alias;
			}
			else
			{
				return mapper.GetTableName(t);
			}
		}
	}
}
