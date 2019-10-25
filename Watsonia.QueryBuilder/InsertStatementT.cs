﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Watsonia.QueryBuilder;

namespace Watsonia.QueryBuilder
{
	public sealed class InsertStatement<T> : Statement
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.GenericInsert;
			}
		}

		public Type Target
		{
			get;
			internal set;
		}

		public List<FieldValue> SetValues { get; } = new List<FieldValue>();

		public Expression Conditions { get; private set; }

		internal InsertStatement()
		{
			this.Target = typeof(T);
		}

		public InsertStatement CreateStatement(DatabaseMapper mapper)
		{
			var insert = new InsertStatement();
			insert.Target = new Table(mapper.GetTableName(this.Target));
			insert.SetValues.AddRange(this.SetValues.Select(sv => new SetValue(new Column(mapper.GetColumnName(sv.Field)), sv.Value)));
			return insert;
		}
	}
}
