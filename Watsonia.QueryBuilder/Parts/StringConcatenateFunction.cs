using System;
using System.Collections.Generic;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class StringConcatenateFunction : Field
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.StringConcatenateFunction;
			}
		}

		public List<StatementPart> Arguments { get; } = new List<StatementPart>();

		public override string ToString()
		{
			return "Concat(" + string.Join(", ", this.Arguments.Select(a => a.ToString())) + ")";
		}
	}
}
