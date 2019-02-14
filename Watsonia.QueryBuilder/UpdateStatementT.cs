using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Watsonia.QueryBuilder;

namespace Watsonia.QueryBuilder
{
	public sealed class UpdateStatement<T> : Statement
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.GenericUpdate;
			}
		}

		public Type Target
		{
			get;
			internal set;
		}

		public List<Tuple<PropertyInfo, object>> SetValues { get; } = new List<Tuple<PropertyInfo, object>>();

		public Expression<Func<T, bool>> Conditions
		{
			get;
			internal set;
		}

		internal UpdateStatement()
		{
			this.Target = typeof(T);
		}

		public UpdateStatement CreateStatement(DatabaseMapper mapper)
		{
			var update = new UpdateStatement();
			update.Target = new Table(mapper.GetTableName(this.Target));
			update.SetValues.AddRange(this.SetValues.Select(sv => new SetValue(new Column(mapper.GetTableName(sv.Item1.DeclaringType), mapper.GetColumnName(sv.Item1)), sv.Item2)));
			foreach (var condition in StatementCreator.VisitStatementConditions<T>(this.Conditions, mapper, false))
			{
				update.Conditions.Add(condition);
			}
			return update;
		}
	}
}
