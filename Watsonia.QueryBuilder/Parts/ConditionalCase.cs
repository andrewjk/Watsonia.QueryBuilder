using System;
using System.Linq;
using System.Text;

namespace Watsonia.QueryBuilder
{
	public sealed class ConditionalCase : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.ConditionalCase;
			}
		}

		public StatementPart Test { get; set; }

		public StatementPart IfTrue { get; set; }

		public StatementPart IfFalse { get; set; }

		public override string ToString()
		{
			var b = new StringBuilder();
			if (this.Test is Condition)
			{
				b.Append("(Case When ");
				b.Append(this.Test.ToString());
				b.Append(" Then ");
				b.Append(this.IfTrue.ToString());
				StatementPart ifFalse = this.IfFalse;
				while (ifFalse is ConditionalCase)
				{
					ConditionalCase ifFalseCase = (ConditionalCase)ifFalse;
					b.Append(" When ");
					b.Append(ifFalseCase.Test.ToString());
					b.Append(" Then ");
					b.Append(ifFalseCase.IfTrue.ToString());
					ifFalse = ifFalseCase.IfFalse;
				}
				b.Append(" Else ");
				b.Append(ifFalse.ToString());
				b.Append(")");
			}
			else
			{
				b.Append("(Case ");
				b.Append(this.Test.ToString());
				b.Append(" When True Then ");
				b.Append(this.IfTrue.ToString());
				b.Append(" Else ");
				b.Append(this.IfFalse.ToString());
				b.Append(")");
			}
			return b.ToString();
		}
	}
}
