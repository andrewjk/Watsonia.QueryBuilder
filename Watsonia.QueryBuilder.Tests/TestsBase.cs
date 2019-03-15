using System;
using System.Collections.Generic;
using System.Text;

namespace Watsonia.QueryBuilder.Tests
{
	public abstract class TestsBase
	{
		protected string TrimExtraWhiteSpace(string s)
		{
			var result = s.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Trim();
			while (result.Contains("  "))
			{
				result = result.Replace("  ", " ");
			}
			result = result.Replace("( ", "(").Replace(" )", ")");
			return result;
		}
	}
}
