using System;
using System.Collections.Generic;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public sealed class RowNumber : SourceExpression
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.RowNumber;
			}
		}

		public List<OrderByExpression> OrderByFields { get; } = new List<OrderByExpression>();

		public RowNumber(params OrderByExpression[] orderByFields)
		{
			this.OrderByFields.AddRange(orderByFields);
		}

		public override string ToString()
		{
			return "RowNumber";
		}
	}
}
