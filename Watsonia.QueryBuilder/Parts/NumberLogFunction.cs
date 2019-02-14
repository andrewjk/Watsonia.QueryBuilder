using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class NumberLogFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.NumberLogFunction;
			}
		}

		public StatementPart Argument { get; set; }

		public override string ToString()
		{
			return "Log(" + this.Argument.ToString() + ")";
		}
	}
}
