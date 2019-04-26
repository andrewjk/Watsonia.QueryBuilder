using System;
using System.Linq;
using System.Text;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// An aggregate operation (such as sum or count) on a source field.
	/// </summary>
	public sealed class Aggregate : Field
	{
		/// <summary>
		/// Gets the type of the statement part.
		/// </summary>
		/// <value>
		/// The type of the part.
		/// </value>
		public override StatementPartType PartType
		{
			get
			{
				return StatementPartType.Aggregate;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this aggregate operation is distinct.
		/// </summary>
		/// <value>
		/// <c>true</c> if this aggregate operation is distinct; otherwise, <c>false</c>.
		/// </value>
		public bool IsDistinct { get; set; }

		/// <summary>
		/// Gets or sets the type of the aggregate (e.g. sum, or count).
		/// </summary>
		/// <value>
		/// The type of the aggregate.
		/// </value>
		public AggregateType AggregateType
		{
			get;
			internal set;
		}

		/// <summary>
		/// Gets or sets the field to be aggregated.
		/// </summary>
		/// <value>
		/// The field.
		/// </value>
		public Field Field
		{
			get;
			internal set;
		}

		// TODO: Remove all of the internal empty constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Aggregate" /> class.
		/// </summary>
		internal Aggregate()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Aggregate" /> class.
		/// </summary>
		/// <param name="aggregateType">The type of the aggregate (e.g. sum, or count).</param>
		/// <param name="field">The field to be aggregated.</param>
		public Aggregate(AggregateType aggregateType, Field field)
		{
			this.AggregateType = aggregateType;
			this.Field = field;
		}

		/// <summary>
		/// Returns a <see cref="string" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="string" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			var b = new StringBuilder();
			b.Append(this.AggregateType.ToString());
			b.Append("(");
			if (this.IsDistinct)
			{
				b.Append("Distinct ");
			}
			if (this.Field != null)
			{
				b.Append(this.Field.ToString());
			}
			else
			{
				b.Append("All");
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
