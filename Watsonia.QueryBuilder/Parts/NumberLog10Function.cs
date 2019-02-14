using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class NumberLog10Function : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.NumberLog10Function;
			}
		}

		public StatementPart Argument { get; set; }

		public override string ToString()
		{
			return "Log10(" + this.Argument.ToString() + ")";
		}
	}
}
