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

		public Type Source
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

		internal SelectStatement()
		{
			this.Source = typeof(T);
		}

		public SelectStatement CreateStatement(DatabaseMapper mapper)
		{
			var select = new SelectStatement();
			select.Source = new Table(mapper.GetTableName(this.Source));
			select.SourceFields.AddRange(this.SourceFields.Select(s => new Column(mapper.GetTableName(s.DeclaringType), mapper.GetColumnName(s))));
			select.SourceFields.AddRange(this.AggregateFields.Select(s => new Aggregate(s.Item2, new Column(s.Item1 != null ? mapper.GetTableName(s.Item1.DeclaringType) : "", s.Item1 != null ? mapper.GetColumnName(s.Item1) : "*"))));
			select.IsAny = this.IsAny;
			select.IsAll = this.IsAll;
			select.IsDistinct = this.IsDistinct;
			select.StartIndex = this.StartIndex;
			select.Limit = this.Limit;
			if (this.Conditions != null)
			{
				foreach (var condition in StatementCreator.VisitStatementConditions<T>(this.Conditions, mapper, false))
				{
					select.Conditions.Add(condition);
				}
			}
			select.OrderByFields.AddRange(this.OrderByFields.Select(s => new OrderByExpression(new Column(mapper.GetTableName(s.Item1.DeclaringType), mapper.GetColumnName(s.Item1)), s.Item2)));
			return select;
		}
	}
}
