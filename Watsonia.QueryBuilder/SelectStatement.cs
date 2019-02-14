using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Watsonia.QueryBuilder;

namespace Watsonia.QueryBuilder
{
	public sealed class SelectStatement : Statement
	{
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.Select;
			}
		}

		public StatementPart Source
		{
			get;
			internal set;
		}

		public List<Join> SourceJoins { get; } = new List<Join>();

		public List<string> IncludePaths { get; } = new List<string>();

		public List<SourceExpression> SourceFields { get; } = new List<SourceExpression>();

		public List<Table> SourceFieldsFrom { get; } = new List<Table>();

		public bool IsAny { get; set; }

		public bool IsAll { get; set; }

		public bool IsContains { get; set; }

		public StatementPart ContainsItem { get; set; }

		public bool IsDistinct { get; set; }

		public int StartIndex { get; set; }

		public int Limit { get; set; }

		public ConditionCollection Conditions { get; } = new ConditionCollection();

		public List<OrderByExpression> OrderByFields { get; } = new List<OrderByExpression>();

		public List<Column> GroupByFields { get; } = new List<Column>();

		public List<SelectStatement> UnionStatements { get; } = new List<SelectStatement>();

		public string Alias { get; set; }

		public bool IsAggregate { get; set; }

		internal SelectStatement()
		{
		}

		public override string ToString()
		{
			var b = new StringBuilder();
			b.Append("(");
			b.Append("Select ");
			if (this.IsAny)
			{
				b.Append("Any ");
			}
			if (this.IsAll)
			{
				b.Append("All ");
			}
			if (this.IsContains)
			{
				b.Append("Contains ");
				b.Append(this.ContainsItem);
				b.Append(" In ");
			}
			if (this.IsDistinct)
			{
				b.Append("Distinct ");
			}
			if (this.Limit == 1)
			{
				b.Append("(Row ");
				b.Append(this.StartIndex);
				b.Append(") ");
			}
			else if (this.StartIndex != 0 || this.Limit != 0)
			{
				b.Append("(Rows ");
				b.Append(this.StartIndex);
				if (this.Limit == 0)
				{
					b.Append("+");
				}
				else
				{
					b.Append("-");
					b.Append(this.StartIndex + this.Limit);
				}
				b.Append(") ");
			}
			if (this.SourceFields.Count > 0)
			{
				b.Append(string.Join(", ", Array.ConvertAll(this.SourceFields.ToArray(), f => f.ToString())));
			}
			else
			{
				b.Append("* ");
			}
			b.AppendLine(" ");
			if (this.Source != null)
			{
				b.Append("From ");
				b.Append(this.Source.ToString());
				b.AppendLine(" ");
			}
			// TODO: Do these ever get used?
			if (this.SourceJoins.Count > 0)
			{
				b.Append("Join ");
				b.Append(string.Join(" And ", Array.ConvertAll(this.SourceJoins.ToArray(), j => j.ToString())));
				b.AppendLine(" ");
			}
			if (this.Conditions.Count > 0)
			{
				b.Append("Where ");
				b.Append(this.Conditions.ToString());
				b.AppendLine(" ");
			}
			if (this.GroupByFields.Count > 0)
			{
				b.Append("Group By ");
				b.Append(string.Join(", ", Array.ConvertAll(this.GroupByFields.ToArray(), f => f.ToString())));
			}
			if (this.OrderByFields.Count > 0)
			{
				b.Append("Order By ");
				b.Append(string.Join(", ", Array.ConvertAll(this.OrderByFields.ToArray(), f => f.ToString())));
			}
			b.Append(")");
			if (!string.IsNullOrEmpty(this.Alias))
			{
				b.Append(" As ");
				b.Append(this.Alias);
			}
			return b.ToString();
		}
	}
}
