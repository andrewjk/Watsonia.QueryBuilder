using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class DateAddFunction : StatementPart
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.DateAddFunction;
			}
		}

		public DatePart DatePart { get; set; }

		public StatementPart Argument { get; set; }

		public StatementPart Number { get; set; }

		internal DateAddFunction(DatePart datePart)
		{
			this.DatePart = datePart;
		}

		public override string ToString()
		{
			return $"DateAdd({this.DatePart}, {this.Argument}, {this.Number})";
		}
	}
}
