using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class StringRemoveFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.StringRemoveFunction;
			}
		}

		public StatementPart Argument { get; set; }

		public StatementPart StartIndex { get; set; }

		public StatementPart Length { get; set; }

		public override string ToString()
		{
			return "Remove(" + this.Argument.ToString() + ", " + this.StartIndex.ToString() + ", " + this.Length.ToString() + ")";
		}
	}
}
