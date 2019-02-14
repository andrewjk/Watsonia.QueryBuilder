using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// An expression that can be used as a condition.
	/// </summary>
	public abstract class ConditionExpression : StatementPart
	{
		public ConditionRelationship Relationship { get; set; }

		public bool Not { get; set; }
	}
}
