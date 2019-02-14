using System;
using System.Collections.Generic;
using System.Linq;
using Watsonia.QueryBuilder;

namespace Watsonia.QueryBuilder
{
	public static partial class Insert
	{
		public static InsertStatement Into(string tableName)
		{
			return Insert.Into(new Table(tableName));
		}

		public static InsertStatement Into(Table table)
		{
			return new InsertStatement() { Target = table };
		}

		public static InsertStatement Value(this InsertStatement insert, string columnName, object value)
		{
			insert.SetValues.Add(new SetValue(columnName, value));
			return insert;
		}

		public static InsertStatement Columns(this InsertStatement insert, params string[] columnNames)
		{
			insert.TargetFields.AddRange(columnNames.Select(cn => new Column(cn)));
			return insert;
		}

		public static InsertStatement Select(this InsertStatement insert, SelectStatement statement)
		{
			insert.Source = statement;
			return insert;
		}
	}
}
