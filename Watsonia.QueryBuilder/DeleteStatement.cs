using System.Collections.Generic;
using Watsonia.QueryBuilder;
using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class DeleteStatement : Statement
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.Delete;
			}
		}

		public Table Target { get; set; }

		public ConditionCollection Conditions { get; private set; }

		internal DeleteStatement()
		{
			this.Conditions = new ConditionCollection();
		}
	}
}
