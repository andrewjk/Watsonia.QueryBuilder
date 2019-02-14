using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Watsonia.QueryBuilder;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// A collection of conditions.
	/// </summary>
	public sealed class ConditionCollection : ConditionExpression, IEnumerable<ConditionExpression>
	{
		private List<ConditionExpression> _conditions = new List<ConditionExpression>();

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
				return StatementPartType.ConditionCollection;
			}
		}

		public int Count
		{
			get
			{
				return _conditions.Count;
			}
		}

		public ConditionExpression this[int index]
		{
			get
			{
				return _conditions[index];
			}
			set
			{
				_conditions[index] = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConditionCollection"/> class.
		/// </summary>
		/// <param name="conditions">The conditions.</param>
		public ConditionCollection(params ConditionExpression[] conditions)
		{
			_conditions.AddRange(conditions);
		}

		public void Add(ConditionExpression item)
		{
			_conditions.Add(item);
		}

		public void Insert(int index, ConditionExpression item)
		{
			_conditions.Insert(index, item);
		}

		public void AddRange(IEnumerable<ConditionExpression> collection)
		{
			_conditions.AddRange(collection);
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			var b = new StringBuilder();
			if (this.Not)
			{
				b.Append("Not ");
			}
			if (this.Count > 1)
			{
				b.Append("(");
			}
			for (int i = 0; i < this.Count; i++)
			{
				if (i > 0)
				{
					b.Append(" ");
					b.Append(this[i].Relationship.ToString());
					b.Append(" ");
				}
				b.Append(this[i].ToString());
			}
			if (this.Count > 1)
			{
				b.Append(")");
			}
			return b.ToString();
		}

		public IEnumerator<ConditionExpression> GetEnumerator()
		{
			return _conditions.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _conditions.GetEnumerator();
		}
	}
}
