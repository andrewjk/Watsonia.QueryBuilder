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
	public sealed class SelectStatement<T> : Statement
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.GenericSelect;
			}
		}

		public Table<T> Source
		{
			get;
			internal set;
		}

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

		public SelectStatement CreateStatement(DatabaseMapper mapper)
		{
			var select = new SelectStatement();
			select.Source = new Table(mapper.GetTableName(this.Source.Type), this.Source.Alias);
			select.SourceFields.AddRange(this.SourceFields.Select(s => new Column(TableNameOrAlias(mapper, s.DeclaringType), mapper.GetColumnName(s))));
			select.SourceFields.AddRange(this.AggregateFields.Select(s => new Aggregate(s.Aggregate, new Column(s.Field != null ? TableNameOrAlias(mapper, s.Field.DeclaringType) : "", s.Field != null ? mapper.GetColumnName(s.Field) : "*"))));
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
			select.OrderByFields.AddRange(this.OrderByFields.Select(s => new OrderByExpression(new Column(TableNameOrAlias(mapper, s.Field.DeclaringType), mapper.GetColumnName(s.Field)), s.Direction)));
			select.GroupByFields.AddRange(this.GroupByFields.Select(s => new Column(TableNameOrAlias(mapper, s.DeclaringType), mapper.GetColumnName(s))));
			return select;
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
