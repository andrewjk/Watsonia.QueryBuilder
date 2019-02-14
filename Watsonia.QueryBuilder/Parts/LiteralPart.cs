using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class LiteralPart : StatementPart
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.LiteralPart;
			}
		}

		public string Value { get; private set; }

		public LiteralPart(string value)
		{
			this.Value = value;
		}
	}
}
