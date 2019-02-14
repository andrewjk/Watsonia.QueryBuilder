using System;
using Watsonia.QueryBuilder;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// The basic building blocks of SQL statements.
	/// </summary>
	public abstract class StatementPart
	{
		/// <summary>
		/// Gets the type of the statement part.
		/// </summary>
		/// <value>
		/// The type of the part.
		/// </value>
		public abstract StatementPartType PartType
		{
			get;
		}
	}
}
