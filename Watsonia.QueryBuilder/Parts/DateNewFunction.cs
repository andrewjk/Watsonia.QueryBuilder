using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class DateNewFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.DateNewFunction;
			}
		}

		public StatementPart Year { get; set; }

		public StatementPart Month { get; set; }

		public StatementPart Day { get; set; }

		public StatementPart Hour { get; set; }

		public StatementPart Minute { get; set; }

		public StatementPart Second { get; set; }

		public DateNewFunction()
		{
		}

		public override string ToString()
		{
			if (this.Hour != null || this.Minute != null || this.Second != null)
			{
				return "DateNew(" + this.Year.ToString() + ", " + this.Month.ToString() + ", " + this.Day.ToString() + ", " + this.Hour.ToString() + ", " + this.Minute.ToString() + ", " + this.Second.ToString() + ")";
			}
			else
			{
				return "DateNew(" + this.Year.ToString() + ", " + this.Month.ToString() + ", " + this.Day.ToString() + ")";
			}
		}
	}
}
