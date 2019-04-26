using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Watsonia.QueryBuilder;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// A collection of fields.
	/// </summary>
	public sealed class FieldCollection : SourceExpression, IEnumerable<SourceExpression>
	{
		private readonly List<SourceExpression> _fields = new List<SourceExpression>();

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
				return StatementPartType.FieldCollection;
			}
		}
		
		public int Count
		{
			get
			{
				return _fields.Count;
			}
		}

		public SourceExpression this[int index]
		{
			get
			{
				return _fields[index];
			}
			set
			{
				_fields[index] = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldCollection"/> class.
		/// </summary>
		/// <param name="fields">The fields.</param>
		public FieldCollection(params Field[] fields)
		{
			_fields.AddRange(fields);
		}

		public void Add(SourceExpression item)
		{
			_fields.Add(item);
		}

		public void Insert(int index, SourceExpression item)
		{
			_fields.Insert(index, item);
		}

		public void AddRange(IEnumerable<SourceExpression> collection)
		{
			_fields.AddRange(collection);
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
			if (this.Count > 1)
			{
				b.Append("(");
			}
			for (var i = 0; i < this.Count; i++)
			{
				if (i > 0)
				{
					b.Append(", ");
				}
				b.Append(this[i].ToString());
			}
			if (this.Count > 1)
			{
				b.Append(")");
			}
			return b.ToString();
		}

		public IEnumerator<SourceExpression> GetEnumerator()
		{
			return _fields.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _fields.GetEnumerator();
		}
	}
}
