using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class StringTrimFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.StringTrimFunction;
			}
		}

		public StatementPart Argument { get; set; }

		public override string ToString()
		{
			return "Trim(" + this.Argument.ToString() + ")";
		}
	}
}
