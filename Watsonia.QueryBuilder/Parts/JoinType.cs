using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public enum JoinType
	{
		Inner,
		Left,
		Right,
		Cross,
		CrossApply,
		OuterApply
	}
}
