using System;
using System.Collections.Generic;
using System.Text;

namespace Watsonia.QueryBuilder
{
	public class SQLiteCommandBuilder : SqlCommandBuilder
	{
		protected override void VisitLimitAtEnd(SelectStatement select)
		{
			this.CommandText.Append(" LIMIT ");
			this.CommandText.Append(select.Limit);
		}

		protected override void VisitCondition(Condition condition)
		{
			// Check for null comparisons first
			var fieldIsNull = (condition.Field is ConstantPart && ((ConstantPart)condition.Field).Value == null);
			var valueIsNull = (condition.Value is ConstantPart && ((ConstantPart)condition.Value).Value == null);
			if ((condition.Operator == SqlOperator.Equals || condition.Operator == SqlOperator.NotEquals) &&
				(fieldIsNull || valueIsNull))
			{
				base.VisitCondition(condition);
			}
			else
			{
				switch (condition.Operator)
				{
					case SqlOperator.Contains:
					{
						this.VisitField(condition.Field);
						this.CommandText.Append(" LIKE '%' || ");
						this.VisitField(condition.Value);
						this.CommandText.Append(" || '%'");
						break;
					}
					case SqlOperator.StartsWith:
					{
						this.VisitField(condition.Field);
						this.CommandText.Append(" LIKE ");
						this.VisitField(condition.Value);
						this.CommandText.Append(" || '%'");
						break;
					}
					case SqlOperator.EndsWith:
					{
						this.VisitField(condition.Field);
						this.CommandText.Append(" LIKE '%' || ");
						this.VisitField(condition.Value);
						break;
					}
					default:
					{
						base.VisitCondition(condition);
						break;
					}
				}
			}
		}

		protected override void VisitDateNewFunction(DateNewFunction function)
		{
			this.VisitDatePartField(function.Year);
			this.CommandText.Append(" || '-' || ");
			this.VisitDatePartField(function.Month);
			this.CommandText.Append(" || '-' || ");
			this.VisitDatePartField(function.Day);

			if (function.Hour != null)
			{
				this.CommandText.Append(" || ' ' || ");
				this.VisitDatePartField(function.Hour);
				this.CommandText.Append(" || ':' || ");
				this.VisitDatePartField(function.Minute);
				this.CommandText.Append(" || ':' || ");
				this.VisitDatePartField(function.Second);
			}
		}

		private void VisitDatePartField(StatementPart datePart)
		{
			if (datePart.PartType == StatementPartType.ConstantPart)
			{
				var constantPart = (ConstantPart)datePart;
				// HACK: Not 100% sure it will always be convertible to an int?
				var value = Convert.ToInt32(constantPart.Value).ToString("D2");
				this.VisitField(new ConstantPart(value));
			}
			else
			{
				this.VisitField(datePart);
			}
		}

		protected override void VisitDatePartFunction(DatePartFunction function)
		{
			switch (function.DatePart)
			{
				case DatePart.Millisecond:
				{
					this.CommandText.Append("CAST(STRFTIME('%s', ");
					this.VisitField(function.Argument);
					this.CommandText.Append(") AS INT)");
					break;
				}
				case DatePart.Second:
				{
					this.CommandText.Append("CAST(STRFTIME('%S', ");
					this.VisitField(function.Argument);
					this.CommandText.Append(") AS INT)");
					break;
				}
				case DatePart.Minute:
				{
					this.CommandText.Append("CAST(STRFTIME('%M', ");
					this.VisitField(function.Argument);
					this.CommandText.Append(") AS INT)");
					break;
				}
				case DatePart.Hour:
				{
					this.CommandText.Append("CAST(STRFTIME('%H', ");
					this.VisitField(function.Argument);
					this.CommandText.Append(") AS INT)");
					break;
				}
				case DatePart.Day:
				{
					this.CommandText.Append("CAST(STRFTIME('%d', ");
					this.VisitField(function.Argument);
					this.CommandText.Append(") AS INT)");
					break;
				}
				case DatePart.Month:
				{
					this.CommandText.Append("CAST(STRFTIME('%m', ");
					this.VisitField(function.Argument);
					this.CommandText.Append(") AS INT)");
					break;
				}
				case DatePart.Year:
				{
					this.CommandText.Append("CAST(STRFTIME('%Y', ");
					this.VisitField(function.Argument);
					this.CommandText.Append(") AS INT)");
					break;
				}
				case DatePart.DayOfWeek:
				{
					this.CommandText.Append("CAST(STRFTIME('%w', ");
					this.VisitField(function.Argument);
					this.CommandText.Append(") AS INT)");
					break;
				}
				case DatePart.DayOfYear:
				{
					this.CommandText.Append("(CAST(STRFTIME('%j', ");
					this.VisitField(function.Argument);
					this.CommandText.Append(") AS INT) - 1)");
					break;
				}
				case DatePart.Date:
				{
					this.CommandText.Append("DATE(");
					this.VisitField(function.Argument);
					this.CommandText.Append(")");
					break;
				}
				default:
				{
					throw new InvalidOperationException("Invalid date part: " + function.DatePart);
				}
			}
		}

		protected override void VisitDateAddFunction(DateAddFunction function)
		{
			// HACK: This may lose precision, like if you want to add days but keep the time!
			if (function.DatePart == DatePart.Hour ||
				function.DatePart == DatePart.Minute ||
				function.DatePart == DatePart.Second ||
				function.DatePart == DatePart.Millisecond)
			{
				this.CommandText.Append("DATETIME(");
			}
			else
			{
				this.CommandText.Append("DATE(");
			}
			this.VisitField(function.Argument);
			this.CommandText.Append(", ");
			this.VisitField(function.Number);
			this.CommandText.Append(" || ' ");
			this.CommandText.Append(function.DatePart.ToString().ToLowerInvariant());
			this.CommandText.Append("'");
			this.CommandText.Append(")");
		}

		protected override void VisitNumberFloorFunction(NumberFloorFunction function)
		{
			// HACK: No floor function in SQLite, so we do it with round per https://stackoverflow.com/a/24821301
			this.CommandText.Append("ROUND(");
			this.VisitField(function.Argument);
			this.CommandText.Append(" - 0.5)");
		}

		protected override void VisitNumberTruncateFunction(NumberTruncateFunction function)
		{
			// HACK: No truncate function in SQLite, so we do it by casting to an int per https://stackoverflow.com/a/16628655
			this.CommandText.Append("CAST((");
			this.VisitField(function.Argument);
			this.CommandText.Append(" * 10) AS INT) / 10");
		}

		protected override void VisitStringLengthFunction(StringLengthFunction function)
		{
			VisitFunction("LENGTH", function.Argument);
		}

		protected override void VisitSubstringFunction(SubstringFunction function)
		{
			this.CommandText.Append("SUBSTR(");
			this.VisitField(function.Argument);
			this.CommandText.Append(", ");
			this.VisitField(function.StartIndex);
			this.CommandText.Append(" + 1");
			if (function.Length != null)
			{
				this.CommandText.Append(", ");
				this.VisitField(function.Length);
			}
			this.CommandText.Append(")");
		}

		protected override void VisitStringRemoveFunction(StringRemoveFunction function)
		{
			this.CommandText.Append("STUFF(");
			this.VisitField(function.Argument);
			this.CommandText.Append(", ");
			this.VisitField(function.StartIndex);
			this.CommandText.Append(" + 1, ");
			this.VisitField(function.Length);
			this.CommandText.Append(", '')");
		}

		protected override void VisitStringCharIndexFunction(StringIndexFunction function)
		{
			this.CommandText.Append("(");
			if (function.StartIndex != null)
			{
				// e.g. INSTR(SUBSTR(text, startIndex), stringToFind) + startIndex
				var substring = new SubstringFunction() { Argument = function.Argument, StartIndex = function.StartIndex };
				this.VisitFunction("INSTR", substring, function.StringToFind);
				this.CommandText.Append(" + ");
				this.VisitField(function.StartIndex);
			}
			else
			{
				this.VisitFunction("INSTR", function.Argument, function.StringToFind);
			}
			this.CommandText.Append(" - 1)");
		}

		protected override void VisitBinaryOperation(BinaryOperation operation)
		{
			if (operation.Operator == BinaryOperator.LeftShift)
			{
				this.CommandText.Append("(");
				this.VisitField(operation.Left);
				this.CommandText.Append(" << ");
				this.VisitField(operation.Right);
				this.CommandText.Append(")");
			}
			else if (operation.Operator == BinaryOperator.RightShift)
			{
				this.CommandText.Append("(");
				this.VisitField(operation.Left);
				this.CommandText.Append(" >> ");
				this.VisitField(operation.Right);
				this.CommandText.Append(")");
			}
			else
			{
				this.CommandText.Append("(");
				this.VisitField(operation.Left);
				this.CommandText.Append(" ");
				this.CommandText.Append(GetOperatorName(operation.Operator));
				this.CommandText.Append(" ");
				this.VisitField(operation.Right);
				this.CommandText.Append(")");
			}
		}

		private string GetOperatorName(BinaryOperator op)
		{
			switch (op)
			{
				case BinaryOperator.Add:
				{
					return "+";
				}
				case BinaryOperator.Subtract:
				{
					return "-";
				}
				case BinaryOperator.Multiply:
				{
					return "*";
				}
				case BinaryOperator.Divide:
				{
					return "/";
				}
				case BinaryOperator.Remainder:
				{
					return "%";
				}
				case BinaryOperator.ExclusiveOr:
				{
					return "&~";
				}
				case BinaryOperator.LeftShift:
				{
					return "<<";
				}
				case BinaryOperator.RightShift:
				{
					return ">>";
				}
				case BinaryOperator.BitwiseAnd:
				{
					return "&";
				}
				case BinaryOperator.BitwiseOr:
				case BinaryOperator.BitwiseExclusiveOr:
				{
					return "|";
				}
				case BinaryOperator.BitwiseNot:
				{
					return "~";
				}
				default:
				{
					throw new InvalidOperationException();
				}
			}
		}

		protected override void VisitUnaryOperation(UnaryOperation operation)
		{
			this.CommandText.Append(GetOperatorName(operation.Operator));
			// TODO: If isbinary: this.Builder.Append(" ");
			this.VisitField(operation.Expression);
		}

		private string GetOperatorName(UnaryOperator op)
		{
			switch (op)
			{
				case UnaryOperator.Not:
				{
					// TODO: return IsBoolean(unary.Expression.Type) ? "NOT" : "~";
					return "NOT ";
				}
				case UnaryOperator.Negate:
				{
					return "-";
				}
				default:
				{
					throw new InvalidOperationException();
				}
			}
		}
	}
}
