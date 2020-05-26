using System;
using System.Collections.Generic;
using System.Text;

namespace Watsonia.QueryBuilder
{
	public sealed class Table<T>
	{
		public Type Type { get; internal set; }

		public string Alias { get; internal set; }

		public Table(Type type)
		{
			this.Type = type;
		}

		public Table(Type type, string alias)
		{
			this.Type = type;
			this.Alias = alias;
		}
	}
}
