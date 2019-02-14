using System;
using System.Collections.Generic;
using System.Linq;
using Watsonia.QueryBuilder;

namespace Watsonia.QueryBuilder
{
	public sealed class UpdateStatement : Statement
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.Update;
			}
		}

		public Table Target { get; set; }

		public List<SetValue> SetValues { get; } = new List<SetValue>();

		public List<ConditionExpression> Conditions { get; internal set; } = new List<ConditionExpression>();

		internal UpdateStatement()
		{
		}
	}
}
