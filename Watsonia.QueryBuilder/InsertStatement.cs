using System;
using System.Collections.Generic;
using System.Linq;
using Watsonia.QueryBuilder;

namespace Watsonia.QueryBuilder
{
	public sealed class InsertStatement : Statement
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.Insert;
			}
		}

		public Table Target { get; set; }

		public List<SetValue> SetValues { get; } = new List<SetValue>();

		public List<Column> TargetFields { get; } = new List<Column>();

		public SelectStatement Source { get; set; }

		internal InsertStatement()
		{
		}
	}
}
