using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class NumberAbsoluteFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.NumberAbsoluteFunction;
			}
		}

		public StatementPart Argument { get; set; }

		public override string ToString()
		{
			return "Absolute(" + this.Argument.ToString() + ")";
		}
	}
}
