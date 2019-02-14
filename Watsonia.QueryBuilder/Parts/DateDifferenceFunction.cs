using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class DateDifferenceFunction : StatementPart
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.DateDifferenceFunction;
			}
		}

		public StatementPart Date1 { get; set; }

		public StatementPart Date2 { get; set; }
	}
}
