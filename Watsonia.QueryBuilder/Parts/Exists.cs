using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class Exists : ConditionExpression
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.Exists;
			}
		}

		public SelectStatement Select { get; set; }

		public override string ToString()
		{
			if (this.Not)
			{
				return "Not Exists " + this.Select.ToString();
			}
			else
			{
				return "Exists " + this.Select.ToString();
			}
		}
	}
}
