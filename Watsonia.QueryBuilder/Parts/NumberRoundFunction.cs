using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class NumberRoundFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.NumberRoundFunction;
			}
		}

		public StatementPart Argument { get; set; }

		public StatementPart Precision { get; set; }

		public override string ToString()
		{
			return "Round(" + this.Argument.ToString() + ", " + this.Precision + ")";
		}
	}
}
