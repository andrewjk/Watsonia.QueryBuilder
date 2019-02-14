using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class NumberFloorFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.NumberFloorFunction;
			}
		}

		public StatementPart Argument { get; set; }

		public override string ToString()
		{
			return "Floor(" + this.Argument.ToString() + ")";
		}
	}
}
