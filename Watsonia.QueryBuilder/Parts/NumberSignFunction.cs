using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class NumberSignFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.NumberSignFunction;
			}
		}

		public StatementPart Argument { get; set; }

		public override string ToString()
		{
			return "Sign(" + this.Argument.ToString() + ")";
		}
	}
}
