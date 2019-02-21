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

		public List<Tuple<PropertyInfo, AggregateType>> AggregateFields { get; } = new List<Tuple<PropertyInfo, AggregateType>>();

		public bool IsAny { get; set; }

		public bool IsAll { get; set; }

		public bool IsDistinct { get; set; }

		public int StartIndex { get; set; }

		public int Limit { get; set; }

		public Expression<Func<T, bool>> Conditions { get; internal set; }

		public List<Tuple<PropertyInfo, OrderDirection>> OrderByFields { get; internal set; } = new List<Tuple<PropertyInfo, OrderDirection>>();

		internal SelectStatement(string alias = null)
		{
			this.Source = new Table<T>(typeof(T), alias);
		}

		public SelectStatement CreateStatement(DatabaseMapper mapper)
		{
			var select = new SelectStatement();
			select.Source = new Table(mapper.GetTableName(this.Source.Type), this.Source.Alias);
			select.SourceFields.AddRange(this.SourceFields.Select(s => new Column(TableNameOrAlias(mapper, s.DeclaringType), mapper.GetColumnName(s))));
			select.SourceFields.AddRange(this.AggregateFields.Select(s => new Aggregate(s.Item2, new Column(s.Item1 != null ? TableNameOrAlias(mapper, s.Item1.DeclaringType) : "", s.Item1 != null ? mapper.GetColumnName(s.Item1) : "*"))));
			select.IsAny = this.IsAny;
			select.IsAll = this.IsAll;
			select.IsDistinct = this.IsDistinct;
			select.StartIndex = this.StartIndex;
			select.Limit = this.Limit;
			if (this.Conditions != null)
			{
				// TODO: Need to handle columns from multiple tables...
				bool aliasTables = !string.IsNullOrEmpty(this.Source.Alias);
				foreach (var condition in StatementCreator.VisitStatementConditions<T>(this.Conditions, mapper, aliasTables))
				{
					select.Conditions.Add(condition);
				}
			}
			select.OrderByFields.AddRange(this.OrderByFields.Select(s => new OrderByExpression(new Column(TableNameOrAlias(mapper, s.Item1.DeclaringType), mapper.GetColumnName(s.Item1)), s.Item2)));
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
