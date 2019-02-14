using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public enum SqlOperator
	{
		Equals,
		NotEquals,
		IsLessThan,
		IsLessThanOrEqualTo,
		IsGreaterThan,
		IsGreaterThanOrEqualTo,
		IsIn,
		Contains,
		StartsWith,
		EndsWith
	}
}
