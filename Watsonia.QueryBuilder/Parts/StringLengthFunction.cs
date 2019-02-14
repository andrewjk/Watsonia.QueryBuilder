using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class StringLengthFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.StringLengthFunction;
			}
		}

		public StatementPart Argument { get; set; }

		public override string ToString()
		{
			return "Length(" + this.Argument.ToString() + ")";
		}
	}
}
