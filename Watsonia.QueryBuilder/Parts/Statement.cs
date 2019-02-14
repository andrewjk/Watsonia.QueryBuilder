using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	public abstract class Statement : StatementPart
	{
		public Command Build()
		{
			return Build(new DatabaseMapper(), new SqlCommandBuilder());
		}

		public Command Build(DatabaseMapper mapper)
		{
			return Build(mapper, new SqlCommandBuilder());
		}

		public Command Build(ICommandBuilder builder)
		{
			return Build(new DatabaseMapper(), builder);
		}

		public Command Build(DatabaseMapper mapper, ICommandBuilder builder)
		{
			builder.VisitStatement(this, mapper);
			return new Command(this, builder.CommandText.ToString(), builder.ParameterValues.ToArray());
		}
	}
}
