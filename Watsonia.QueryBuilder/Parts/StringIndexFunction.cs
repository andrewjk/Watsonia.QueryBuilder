using System;
using System.Linq;
using System.Text;

namespace Watsonia.QueryBuilder
{
	public sealed class StringIndexFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.StringIndexFunction;
			}
		}

		public StatementPart Argument { get; set; }

		public StatementPart StringToFind { get; set; }

		public StatementPart StartIndex { get; set; }

		public override string ToString()
		{
			var b = new StringBuilder();
			b.Append("IndexOf(");
			b.Append(this.Argument.ToString());
			b.Append(", ");
			b.Append(this.StringToFind.ToString());
			if (this.StartIndex != null)
			{
				b.Append(", ");
				b.Append(this.StartIndex.ToString());
			}
			b.Append(")");
			return b.ToString();
		}
	}
}
