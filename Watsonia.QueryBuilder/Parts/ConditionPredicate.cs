using System;
using System.Linq;
using System.Text;

namespace Watsonia.QueryBuilder
{
	// TODO: What even is this
	public sealed class ConditionPredicate : SourceExpression
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.ConditionPredicate;
			}
		}

		public StatementPart Predicate { get; set; }

		public override string ToString()
		{
			var b = new StringBuilder();
			b.Append("(Case When ");
			b.Append(this.Predicate.ToString());
			b.Append(" Then True Else False)");
			if (!string.IsNullOrEmpty(this.Alias))
			{
				b.Append(" As ");
				b.Append(this.Alias);
			}
			return b.ToString();
		}
	}
}
