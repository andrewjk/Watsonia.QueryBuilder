using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class NumberPowerFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.NumberPowerFunction;
			}
		}

		public StatementPart Argument { get; set; }

		public StatementPart Power { get; set; }

		public override string ToString()
		{
			return "Power(" + this.Argument.ToString() + ", " + this.Power.ToString() + ")";
		}
	}
}
