using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// An expression that can be used in the field list of a select statement.
	/// </summary>
	public abstract class SourceExpression : StatementPart
	{
		public string Alias { get; set; }
	}
}
