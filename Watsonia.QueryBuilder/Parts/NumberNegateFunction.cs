using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class NumberNegateFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.NumberNegateFunction;
			}
		}

		public StatementPart Argument { get; set; }

		public override string ToString()
		{
			return "Negate(" + this.Argument.ToString() + ")";
		}
	}
}
