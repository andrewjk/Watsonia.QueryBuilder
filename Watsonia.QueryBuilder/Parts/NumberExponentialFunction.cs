using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class NumberExponentialFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.NumberExponentialFunction;
			}
		}

		public StatementPart Argument { get; set; }

		public override string ToString()
		{
			return "Exponential(" + this.Argument.ToString() + ")";
		}
	}
}
