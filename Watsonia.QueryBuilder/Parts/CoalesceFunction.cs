using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// Returns the first non-null expression.
	/// </summary>
	public sealed class CoalesceFunction : Field
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
				return StatementPartType.CoalesceFunction;
			}
		}

		/// <summary>
		/// Gets or sets the first expression.
		/// </summary>
		/// <value>
		/// The first expression.
		/// </value>
		public List<SourceExpression> Arguments { get; } = new List<SourceExpression>();

		/// <summary>
		/// Initializes a new instance of the <see cref="CoalesceFunction" /> class.
		/// </summary>
		internal CoalesceFunction()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CoalesceFunction" /> class.
		/// </summary>
		/// <param name="arguments">The arguments.</param>
		public CoalesceFunction(params SourceExpression[] arguments)
		{
			this.Arguments.AddRange(arguments);
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
			b.Append("Coalesce(");
			b.Append(string.Join(", ", this.Arguments.Select(a => a.ToString())));
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
