using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class StringReplaceFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.StringReplaceFunction;
			}
		}

		public StatementPart Argument { get; set; }

		public StatementPart OldValue { get; set; }

		public StatementPart NewValue { get; set; }

		public override string ToString()
		{
			return "Replace(" + this.Argument.ToString() + ", " + this.OldValue.ToString() + ", " + this.NewValue.ToString() + ")";
		}
	}
}
