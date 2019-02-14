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
	public sealed class DeleteStatement<T> : Statement
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.GenericDelete;
			}
		}

		public Type Target
		{
			get;
			internal set;
		}

		public Expression<Func<T, bool>> Conditions
		{
			get;
			internal set;
		}

		internal DeleteStatement()
		{
			this.Target = typeof(T);
		}

		public DeleteStatement CreateStatement(DatabaseMapper mapper)
		{
			var delete = new DeleteStatement();
			delete.Target = new Table(mapper.GetTableName(this.Target));
			foreach (var condition in StatementCreator.VisitStatementConditions<T>(this.Conditions, mapper, false))
			{
				delete.Conditions.Add(condition);
			}
			return delete;
		}
	}
}
