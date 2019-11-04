using System;
using System.Collections.Generic;
using System.Text;

namespace Watsonia.QueryBuilder
{
	public abstract class GenericStatement : Statement
	{
		public abstract Statement CreateStatement(DatabaseMapper mapper);
	}
}
