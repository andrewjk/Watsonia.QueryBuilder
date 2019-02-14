using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class NumberTruncateFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.NumberTruncateFunction;
			}
		}

		public StatementPart Argument { get; set; }

		public override string ToString()
		{
			return "Truncate(" + this.Argument.ToString() + ")";
		}
	}
}
