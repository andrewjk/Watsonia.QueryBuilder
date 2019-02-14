using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class DatePartFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.DatePartFunction;
			}
		}

		public DatePart DatePart { get; set; }

		public StatementPart Argument { get; set; }

		internal DatePartFunction(DatePart datePart)
		{
			this.DatePart = datePart;
		}

		public override string ToString()
		{
			return $"DatePart({this.DatePart}, {this.Argument})";
		}
	}
}
