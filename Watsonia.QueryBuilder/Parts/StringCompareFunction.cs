using System;
using System.Linq;
using System.Text;

namespace Watsonia.QueryBuilder
{
	public sealed class StringCompareFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.StringCompareFunction;
			}
		}

		public StatementPart Argument { get; set; }

		public StatementPart Other { get; set; }

		public override string ToString()
		{
			var b = new StringBuilder();
			b.Append("Compare(");
			b.Append(this.Argument.ToString());
			b.Append(", ");
			b.Append(this.Other.ToString());
			b.Append(")");
			return b.ToString();
		}
	}
}
