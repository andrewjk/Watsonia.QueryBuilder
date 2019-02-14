using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public class SelectExpression : SourceExpression
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.SelectExpression;
			}
		}

		public SelectStatement Select { get; set; }

		public SelectExpression(SelectStatement select, string alias = null)
		{
			this.Select = select;
			this.Alias = alias;
		}
	}
}
