using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class NumberCeilingFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.NumberCeilingFunction;
			}
		}

		public StatementPart Argument { get; set; }

		public override string ToString()
		{
			return "Ceiling(" + this.Argument.ToString() + ")";
		}
	}
}
