using System;
using System.Linq;
using System.Text;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// A column in a table.
	/// </summary>
	public sealed class Column : Field
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
				return StatementPartType.Column;
			}
		}

		/// <summary>
		/// Gets or sets the table.
		/// </summary>
		/// <value>
		/// The table.
		/// </value>
		public Table Table { get; set; }

		/// <summary>
		/// Gets the name of the column.
		/// </summary>
		/// <value>
		/// The name of the column.
		/// </value>
		public string Name { get; private set; }

		internal Type PropertyType { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Column" /> class.
		/// </summary>
		/// <param name="name">The name of the column.</param>
		public Column(string name)
		{
			this.Name = name;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Column" /> class.
		/// </summary>
		/// <param name="tableName">The name of the table.</param>
		/// <param name="name">The name of the column.</param>
		public Column(string tableName, string name)
		{
			this.Table = new Table(tableName);
			this.Name = name;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Column"/> class.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <param name="name">The name.</param>
		public Column(Table table, string name)
		{
			this.Table = table;
			this.Name = name;
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
			if (this.Table != null)
			{
				b.Append(this.Table.ToString());
				b.Append(".");
			}
			b.Append(this.Name);
			if (!string.IsNullOrEmpty(this.Alias))
			{
				b.Append(" As ");
				b.Append(this.Alias);
			}
			return b.ToString();
		}
	}
}
