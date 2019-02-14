using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class NumberTrigFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.NumberTrigFunction;
			}
		}

		public TrigFunction Function { get; set; }

		public StatementPart Argument { get; set; }

		// For Atan2
		public StatementPart Argument2 { get; set; }

		public override string ToString()
		{
			return this.Function.ToString() + "(" + this.Argument.ToString() + ")";
		}
	}
}
