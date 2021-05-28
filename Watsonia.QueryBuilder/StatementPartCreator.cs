using Remotion.Linq;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// Converts Expressions (such as those in Re-Linq's QueryModels) into StatementParts.
	/// </summary>
	internal class StatementPartCreator : RelinqExpressionVisitor
	{
		private QueryModel QueryModel { get; set; }

		private DatabaseMapper Configuration { get; set; }

		private bool AliasTables { get; set; }

		private Stack<StatementPart> Stack { get; set; }

		private StatementPartCreator(QueryModel queryModel, DatabaseMapper mapper, bool aliasTables)
		{
			this.QueryModel = queryModel;
			this.Configuration = mapper;
			this.AliasTables = aliasTables;
			this.Stack = new Stack<StatementPart>();
		}

		public static StatementPart Visit(QueryModel queryModel, Expression expression, DatabaseMapper mapper, bool aliasTables)
		{
			var visitor = new StatementPartCreator(queryModel, mapper, aliasTables);
			visitor.Visit(expression);
			return visitor.Stack.Pop();
		}

		protected override Expression VisitBinary(BinaryExpression expression)
		{
			switch (expression.NodeType)
			{
				case ExpressionType.And:
				case ExpressionType.AndAlso:
				case ExpressionType.Or:
				case ExpressionType.OrElse:
				case ExpressionType.ExclusiveOr:
				{
					if (expression.Type == typeof(bool))
					{
						return VisitBinaryConditionCollection(expression);
					}
					else
					{
						return VisitBinaryOperation(expression);
					}
				}
				case ExpressionType.Equal:
				case ExpressionType.NotEqual:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				{
					return VisitBinaryCondition(expression);
				}
				case ExpressionType.Add:
				case ExpressionType.Subtract:
				case ExpressionType.Multiply:
				case ExpressionType.Divide:
				case ExpressionType.Modulo:
				case ExpressionType.LeftShift:
				case ExpressionType.RightShift:
				{
					return VisitBinaryOperation(expression);
				}
			}

			return base.VisitBinary(expression);
		}

		private Expression VisitBinaryConditionCollection(BinaryExpression expression)
		{
			Visit(expression.Left);
			Visit(expression.Right);

			// Convert the conditions on the stack to a collection and set each condition's relationship
			var newCondition = new ConditionCollection();
			for (var i = 0; i < 2; i++)
			{
				ConditionExpression subCondition;
				if (this.Stack.Peek() is ConditionExpression)
				{
					subCondition = (ConditionExpression)this.Stack.Pop();
				}
				else if (this.Stack.Peek() is UnaryOperation unaryOp && unaryOp.Expression is ConditionExpression)
				{
					var unary = (UnaryOperation)this.Stack.Pop();
					subCondition = (ConditionExpression)unary.Expression;
				}
				else if (this.Stack.Peek() is UnaryOperation unaryOp2 && unaryOp2.Expression is Column)
				{
					var unary = (UnaryOperation)this.Stack.Pop();
					var column = (Column)unary.Expression;
					subCondition = new Condition(column, SqlOperator.Equals, new ConstantPart(unary.Operator != UnaryOperator.Not));
				}
				else if (this.Stack.Peek() is ConstantPart constantPart && constantPart.Value is bool)
				{
					var constant = (ConstantPart)this.Stack.Pop();
					var value = (bool)constant.Value;
					subCondition = new Condition() {
						Field = new ConstantPart(value),
						Operator = SqlOperator.Equals,
						Value = new ConstantPart(true)
					};
				}
				else if (this.Stack.Peek() is Column columnPart && columnPart.PropertyType == typeof(bool))
				{
					var column = (Column)this.Stack.Pop();
					subCondition = new Condition(column, SqlOperator.Equals, new ConstantPart(true));
				}
				else
				{
					break;
				}

				if (subCondition != null)
				{
					newCondition.Insert(0, subCondition);

					if (expression.NodeType == ExpressionType.And ||
						expression.NodeType == ExpressionType.AndAlso)
					{
						subCondition.Relationship = ConditionRelationship.And;
					}
					else
					{
						subCondition.Relationship = ConditionRelationship.Or;
					}
				}
			}

			if (newCondition.Count > 1)
			{
				this.Stack.Push(newCondition);
			}
			else
			{
				this.Stack.Push(newCondition[0]);
			}

			return expression;
		}

		private Expression VisitBinaryCondition(BinaryExpression expression)
		{
			var newCondition = new Condition();
			Visit(expression.Left);
			newCondition.Field = this.Stack.Pop();

			switch (expression.NodeType)
			{
				case ExpressionType.Equal:
				{
					newCondition.Operator = SqlOperator.Equals;
					break;
				}
				case ExpressionType.NotEqual:
				{
					newCondition.Operator = SqlOperator.NotEquals;
					break;
				}
				case ExpressionType.LessThan:
				{
					newCondition.Operator = SqlOperator.IsLessThan;
					break;
				}
				case ExpressionType.LessThanOrEqual:
				{
					newCondition.Operator = SqlOperator.IsLessThanOrEqualTo;
					break;
				}
				case ExpressionType.GreaterThan:
				{
					newCondition.Operator = SqlOperator.IsGreaterThan;
					break;
				}
				case ExpressionType.GreaterThanOrEqual:
				{
					newCondition.Operator = SqlOperator.IsGreaterThanOrEqualTo;
					break;
				}
				default:
				{
					// TODO: Throw that as a better exception
					throw new NotSupportedException("Unhandled NodeType: " + expression.NodeType);
				}
			}

			Visit(expression.Right);
			newCondition.Value = this.Stack.Pop();

			if (newCondition.Field.PartType == StatementPartType.FieldCollection)
			{
				// If anonymous types have been passed in for multi-value checking, we need to split
				// them out manually from the field collection and constant part that Relinq creates
				var fields = (FieldCollection)newCondition.Field;
				var value = ((ConstantPart)newCondition.Value).Value;
				var valueList = value.GetType().GetProperties().Select(x => x.GetValue(value, null)).ToList();
				var newConditionCollection = new ConditionCollection();
				// Swap the operator if it's NotEquals
				var op = newCondition.Operator;
				if (op == SqlOperator.NotEquals)
				{
					op = SqlOperator.Equals;
					newConditionCollection.Not = true;
				}
				for (var i = 0; i < fields.Count; i++)
				{
					newConditionCollection.Add(new Condition(fields[i], op, valueList[i]));
				}
				this.Stack.Push(newConditionCollection);
			}
			else
			{
				this.Stack.Push(newCondition);
			}

			return expression;
		}

		private Expression VisitBinaryOperation(BinaryExpression expression)
		{
			var newBinary = new BinaryOperation();
			Visit(expression.Left);
			newBinary.Left = (SourceExpression)this.Stack.Pop();

			switch (expression.NodeType)
			{
				case ExpressionType.Add:
				{
					newBinary.Operator = BinaryOperator.Add;
					break;
				}
				case ExpressionType.Subtract:
				{
					newBinary.Operator = BinaryOperator.Subtract;
					break;
				}
				case ExpressionType.Multiply:
				{
					newBinary.Operator = BinaryOperator.Multiply;
					break;
				}
				case ExpressionType.Divide:
				{
					newBinary.Operator = BinaryOperator.Divide;
					break;
				}
				case ExpressionType.Modulo:
				{
					newBinary.Operator = BinaryOperator.Remainder;
					break;
				}
				case ExpressionType.LeftShift:
				{
					newBinary.Operator = BinaryOperator.LeftShift;
					break;
				}
				case ExpressionType.RightShift:
				{
					newBinary.Operator = BinaryOperator.RightShift;
					break;
				}
				case ExpressionType.And:
				case ExpressionType.AndAlso:
				{
					newBinary.Operator = BinaryOperator.BitwiseAnd;
					break;
				}
				case ExpressionType.Or:
				case ExpressionType.OrElse:
				{
					newBinary.Operator = BinaryOperator.BitwiseOr;
					break;
				}
				case ExpressionType.ExclusiveOr:
				{
					newBinary.Operator = BinaryOperator.ExclusiveOr;
					break;
				}
				default:
				{
					// TODO: Throw that as a better exception
					throw new NotSupportedException("Unhandled NodeType: " + expression.NodeType);
				}
			}

			Visit(expression.Right);
			newBinary.Right = (SourceExpression)this.Stack.Pop();
			this.Stack.Push(newBinary);

			return expression;
		}

		protected override Expression VisitConditional(ConditionalExpression node)
		{
			var newConditionalCase = new ConditionalCase();
			Visit(node.Test);
			newConditionalCase.Test = this.Stack.Pop();
			Visit(node.IfTrue);
			newConditionalCase.IfTrue = this.Stack.Pop();
			Visit(node.IfFalse);
			newConditionalCase.IfFalse = this.Stack.Pop();
			this.Stack.Push(newConditionalCase);
			return node;
		}

		protected override Expression VisitConstant(ConstantExpression expression)
		{
			if (expression.Value == null)
			{
				this.Stack.Push(new ConstantPart(expression.Value));
			}
			else if (this.Configuration.ShouldMapType(expression.Type))
			{
				var primaryKeyName = this.Configuration.GetPrimaryKeyColumnName(expression.Type);
				var property = expression.Value.GetType().GetProperty(primaryKeyName);
				var value = property.GetValue(expression.Value);
				this.Stack.Push(new ConstantPart(value));
			}
			else if (TypeHelper.IsGenericType(expression.Type, typeof(IQueryable<>)))
			{
				var queryType = expression.Value.GetType().GetGenericArguments()[0];
				var tableName = this.Configuration.GetTableName(queryType);
				this.Stack.Push(new Table(tableName));
			}
			else
			{
				this.Stack.Push(new ConstantPart(expression.Value));
			}
			return expression;
		}

		protected override Expression VisitMember(MemberExpression expression)
		{
			if (expression.Member.DeclaringType == typeof(string))
			{
				switch (expression.Member.Name)
				{
					case "Length":
					{
						var newFunction = new StringLengthFunction();
						Visit(expression.Expression);
						newFunction.Argument = this.Stack.Pop();
						this.Stack.Push(newFunction);
						return expression;
					}
				}
			}
			else if (expression.Member.DeclaringType == typeof(DateTime) || expression.Member.DeclaringType == typeof(DateTimeOffset))
			{
				switch (expression.Member.Name)
				{
					case "Date":
					{
						var newFunction = new DatePartFunction(DatePart.Date);
						Visit(expression.Expression);
						newFunction.Argument = this.Stack.Pop();
						this.Stack.Push(newFunction);
						return expression;
					}
					case "Day":
					{
						var newFunction = new DatePartFunction(DatePart.Day);
						Visit(expression.Expression);
						newFunction.Argument = this.Stack.Pop();
						this.Stack.Push(newFunction);
						return expression;
					}
					case "Month":
					{
						var newFunction = new DatePartFunction(DatePart.Month);
						Visit(expression.Expression);
						newFunction.Argument = this.Stack.Pop();
						this.Stack.Push(newFunction);
						return expression;
					}
					case "Year":
					{
						var newFunction = new DatePartFunction(DatePart.Year);
						Visit(expression.Expression);
						newFunction.Argument = this.Stack.Pop();
						this.Stack.Push(newFunction);
						return expression;
					}
					case "Hour":
					{
						var newFunction = new DatePartFunction(DatePart.Hour);
						Visit(expression.Expression);
						newFunction.Argument = this.Stack.Pop();
						this.Stack.Push(newFunction);
						return expression;
					}
					case "Minute":
					{
						var newFunction = new DatePartFunction(DatePart.Minute);
						Visit(expression.Expression);
						newFunction.Argument = this.Stack.Pop();
						this.Stack.Push(newFunction);
						return expression;
					}
					case "Second":
					{
						var newFunction = new DatePartFunction(DatePart.Second);
						Visit(expression.Expression);
						newFunction.Argument = this.Stack.Pop();
						this.Stack.Push(newFunction);
						return expression;
					}
					case "Millisecond":
					{
						var newFunction = new DatePartFunction(DatePart.Millisecond);
						Visit(expression.Expression);
						newFunction.Argument = this.Stack.Pop();
						this.Stack.Push(newFunction);
						return expression;
					}
					case "DayOfWeek":
					{
						var newFunction = new DatePartFunction(DatePart.DayOfWeek);
						Visit(expression.Expression);
						newFunction.Argument = this.Stack.Pop();
						this.Stack.Push(newFunction);
						return expression;
					}
					case "DayOfYear":
					{
						var newFunction = new DatePartFunction(DatePart.DayOfYear);
						Visit(expression.Expression);
						newFunction.Argument = this.Stack.Pop();
						this.Stack.Push(newFunction);
						return expression;
					}
				}
			}
			else if (expression.Member.MemberType == MemberTypes.Property)
			{
				string tableName;
				if (this.AliasTables)
				{
					if (expression.Expression is UnaryExpression unaryExpression)
					{
						var source = (QuerySourceReferenceExpression)unaryExpression.Operand;
						tableName = source.ReferencedQuerySource.ItemName.Replace("<generated>", "g");
					}
					else if (expression.Expression is MemberExpression memberExpression)
					{
						var source = (QuerySourceReferenceExpression)memberExpression.Expression;
						tableName = source.ReferencedQuerySource.ItemName.Replace("<generated>", "g");
					}
					else
					{
						var source = (QuerySourceReferenceExpression)expression.Expression;
						tableName = source.ReferencedQuerySource.ItemName.Replace("<generated>", "g");
					}
				}
				else
				{
					// The property may be declared on a base type, so we can't just get DeclaringType
					// Instead, we get the type from the expression that was used to reference it
					var propertyType = expression.Expression.Type;

					// HACK: Replace interfaces with actual tables
					//	There has to be a way of intercepting the QueryModel creation??
					if (propertyType.IsInterface)
					{
						propertyType = this.QueryModel.MainFromClause.ItemType;
					}

					tableName = this.Configuration.GetTableName(propertyType);
				}

				var property = (PropertyInfo)expression.Member;
				var columnName = this.Configuration.GetColumnName(property);
				if (this.Configuration.IsRelatedItem(property))
				{
					// TODO: Should this be done here, or when converting the statement to SQL?
					columnName = this.Configuration.GetForeignKeyColumnName(property);
				}
				var newColumn = new Column(tableName, columnName) { PropertyType = property.PropertyType };
				this.Stack.Push(newColumn);
				return expression;
			}

			throw new NotSupportedException($"The member access '{expression.Member}' is not supported");
		}

		protected override Expression VisitMethodCall(MethodCallExpression expression)
		{
			var handled = false;

			if (expression.Method.DeclaringType == typeof(string))
			{
				handled = VisitStringMethodCall(expression);
			}
			else if (expression.Method.DeclaringType == typeof(DateTime))
			{
				handled = VisitDateTimeMethodCall(expression);
			}
			else if (expression.Method.DeclaringType == typeof(decimal))
			{
				handled = VisitDecimalMethodCall(expression);
			}
			else if (expression.Method.DeclaringType == typeof(Math))
			{
				handled = VisitMathMethodCall(expression);
			}

			if (!handled)
			{
				if (expression.Method.Name == "ToString")
				{
					handled = VisitToStringMethodCall(expression);
				}
				else if (expression.Method.Name == "Equals")
				{
					handled = VisitEqualsMethodCall(expression);
				}
				else if (!expression.Method.IsStatic && expression.Method.Name == "CompareTo" && expression.Method.ReturnType == typeof(int) && expression.Arguments.Count == 1)
				{
					handled = VisitCompareToMethodCall(expression);
				}
				else if (expression.Method.IsStatic && expression.Method.Name == "Compare" && expression.Method.ReturnType == typeof(int) && expression.Arguments.Count == 2)
				{
					handled = VisitCompareMethodCall(expression);
				}
			}

			return handled ? expression : base.VisitMethodCall(expression);
		}

		private bool VisitStringMethodCall(MethodCallExpression expression)
		{
			switch (expression.Method.Name)
			{
				case "StartsWith":
				{
					var newCondition = new Condition();
					newCondition.Operator = SqlOperator.StartsWith;
					this.Visit(expression.Object);
					newCondition.Field = this.Stack.Pop();
					this.Visit(expression.Arguments[0]);
					newCondition.Value = this.Stack.Pop();
					this.Stack.Push(newCondition);
					return true;
				}
				case "EndsWith":
				{
					var newCondition = new Condition();
					newCondition.Operator = SqlOperator.EndsWith;
					this.Visit(expression.Object);
					newCondition.Field = this.Stack.Pop();
					this.Visit(expression.Arguments[0]);
					newCondition.Value = this.Stack.Pop();
					this.Stack.Push(newCondition);
					return true;
				}
				case "Contains":
				{
					var newCondition = new Condition();
					newCondition.Operator = SqlOperator.Contains;
					this.Visit(expression.Object);
					newCondition.Field = this.Stack.Pop();
					this.Visit(expression.Arguments[0]);
					newCondition.Value = this.Stack.Pop();
					this.Stack.Push(newCondition);
					return true;
				}
				case "Concat":
				{
					var newFunction = new StringConcatenateFunction();
					IList<Expression> args = expression.Arguments;
					if (args.Count == 1 && args[0].NodeType == ExpressionType.NewArrayInit)
					{
						args = ((NewArrayExpression)args[0]).Expressions;
					}
					for (var i = 0; i < args.Count; i++)
					{
						this.Visit(args[i]);
						newFunction.Arguments.Add(this.Stack.Pop());
					}
					this.Stack.Push(newFunction);
					return true;
				}
				case "IsNullOrEmpty":
				{
					var newCondition = new ConditionCollection();

					var isNullCondition = new Condition();
					this.Visit(expression.Arguments[0]);
					isNullCondition.Field = this.Stack.Pop();
					isNullCondition.Operator = SqlOperator.Equals;
					isNullCondition.Value = new ConstantPart(null);
					newCondition.Add(isNullCondition);

					var notEqualsCondition = new Condition();
					notEqualsCondition.Relationship = ConditionRelationship.Or;
					this.Visit(expression.Arguments[0]);
					notEqualsCondition.Field = this.Stack.Pop();
					notEqualsCondition.Operator = SqlOperator.Equals;
					notEqualsCondition.Value = new ConstantPart("");
					newCondition.Add(notEqualsCondition);

					this.Stack.Push(newCondition);
					return true;
				}
				case "ToUpper":
				case "ToUpperInvariant":
				{
					var newFunction = new StringToUpperFunction();
					this.Visit(expression.Object);
					newFunction.Argument = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "ToLower":
				case "ToLowerInvariant":
				{
					var newFunction = new StringToLowerFunction();
					this.Visit(expression.Object);
					newFunction.Argument = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "Replace":
				{
					var newFunction = new StringReplaceFunction();
					this.Visit(expression.Object);
					newFunction.Argument = this.Stack.Pop();
					this.Visit(expression.Arguments[0]);
					newFunction.OldValue = this.Stack.Pop();
					this.Visit(expression.Arguments[1]);
					newFunction.NewValue = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "Substring":
				{
					var newFunction = new SubstringFunction();
					this.Visit(expression.Object);
					newFunction.Argument = this.Stack.Pop();
					this.Visit(expression.Arguments[0]);
					newFunction.StartIndex = this.Stack.Pop();
					if (expression.Arguments.Count > 1)
					{
						this.Visit(expression.Arguments[1]);
						newFunction.Length = this.Stack.Pop();
					}
					else
					{
						newFunction.Length = new ConstantPart(8000);
					}
					this.Stack.Push(newFunction);
					return true;
				}
				case "Remove":
				{
					var newFunction = new StringRemoveFunction();
					this.Visit(expression.Object);
					newFunction.Argument = this.Stack.Pop();
					this.Visit(expression.Arguments[0]);
					newFunction.StartIndex = this.Stack.Pop();
					if (expression.Arguments.Count > 1)
					{
						this.Visit(expression.Arguments[1]);
						newFunction.Length = this.Stack.Pop();
					}
					else
					{
						newFunction.Length = new ConstantPart(8000);
					}
					this.Stack.Push(newFunction);
					return true;
				}
				case "IndexOf":
				{
					var newFunction = new StringIndexFunction();
					this.Visit(expression.Object);
					newFunction.Argument = this.Stack.Pop();
					this.Visit(expression.Arguments[0]);
					newFunction.StringToFind = this.Stack.Pop();
					if (expression.Arguments.Count == 2 && expression.Arguments[1].Type == typeof(int))
					{
						this.Visit(expression.Arguments[1]);
						newFunction.StartIndex = this.Stack.Pop();
					}
					this.Stack.Push(newFunction);
					return true;
				}
				case "Trim":
				{
					var newFunction = new StringTrimFunction();
					this.Visit(expression.Object);
					newFunction.Argument = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
			}

			return false;
		}

		private bool VisitDateTimeMethodCall(MethodCallExpression expression)
		{
			switch (expression.Method.Name)
			{
				case "op_Subtract":
				{
					if (expression.Arguments[1].Type == typeof(DateTime))
					{
						var newFunction = new DateDifferenceFunction();
						this.Visit(expression.Arguments[0]);
						newFunction.Date1 = this.Stack.Pop();
						this.Visit(expression.Arguments[1]);
						newFunction.Date2 = this.Stack.Pop();
						this.Stack.Push(newFunction);
						return true;
					}
					break;
				}
				case "AddDays":
				{
					var newFunction = new DateAddFunction(DatePart.Day);
					this.Visit(expression.Object);
					newFunction.Argument = this.Stack.Pop();
					this.Visit(expression.Arguments[0]);
					newFunction.Number = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "AddMonths":
				{
					var newFunction = new DateAddFunction(DatePart.Month);
					this.Visit(expression.Object);
					newFunction.Argument = this.Stack.Pop();
					this.Visit(expression.Arguments[0]);
					newFunction.Number = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "AddYears":
				{
					var newFunction = new DateAddFunction(DatePart.Year);
					this.Visit(expression.Object);
					newFunction.Argument = this.Stack.Pop();
					this.Visit(expression.Arguments[0]);
					newFunction.Number = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "AddHours":
				{
					var newFunction = new DateAddFunction(DatePart.Hour);
					this.Visit(expression.Object);
					newFunction.Argument = this.Stack.Pop();
					this.Visit(expression.Arguments[0]);
					newFunction.Number = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "AddMinutes":
				{
					var newFunction = new DateAddFunction(DatePart.Minute);
					this.Visit(expression.Object);
					newFunction.Argument = this.Stack.Pop();
					this.Visit(expression.Arguments[0]);
					newFunction.Number = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "AddSeconds":
				{
					var newFunction = new DateAddFunction(DatePart.Second);
					this.Visit(expression.Object);
					newFunction.Argument = this.Stack.Pop();
					this.Visit(expression.Arguments[0]);
					newFunction.Number = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "AddMilliseconds":
				{
					var newFunction = new DateAddFunction(DatePart.Millisecond);
					this.Visit(expression.Object);
					newFunction.Argument = this.Stack.Pop();
					this.Visit(expression.Arguments[0]);
					newFunction.Number = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
			}

			return false;
		}

		private bool VisitDecimalMethodCall(MethodCallExpression expression)
		{
			switch (expression.Method.Name)
			{
				case "Add":
				case "Subtract":
				case "Multiply":
				case "Divide":
				case "Remainder":
				{
					var newOperation = new BinaryOperation();
					this.Visit(expression.Arguments[0]);
					newOperation.Left = (SourceExpression)this.Stack.Pop();
					newOperation.Operator = (BinaryOperator)Enum.Parse(typeof(BinaryOperator), expression.Method.Name);
					this.Visit(expression.Arguments[1]);
					newOperation.Right = (SourceExpression)this.Stack.Pop();
					this.Stack.Push(newOperation);
					return true;
				}
				case "Negate":
				{
					var newFunction = new NumberNegateFunction();
					this.Visit(expression.Arguments[0]);
					newFunction.Argument = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "Ceiling":
				{
					var newFunction = new NumberCeilingFunction();
					this.Visit(expression.Arguments[0]);
					newFunction.Argument = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "Floor":
				{
					var newFunction = new NumberFloorFunction();
					this.Visit(expression.Arguments[0]);
					newFunction.Argument = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "Round":
				{
					var newFunction = new NumberRoundFunction();
					this.Visit(expression.Arguments[0]);
					newFunction.Argument = this.Stack.Pop();
					if (expression.Arguments.Count == 2 && expression.Arguments[1].Type == typeof(int))
					{
						this.Visit(expression.Arguments[1]);
						newFunction.Precision = this.Stack.Pop();
					}
					else
					{
						// TODO: Make it consistent where these are set
						// should they be defaults here, or in the function class, or when making the sql
						// probably when making the sql, because the appropriate default will differ between platforms
						newFunction.Precision = new ConstantPart(0);
					}
					this.Stack.Push(newFunction);
					return true;
				}
				case "Truncate":
				{
					var newFunction = new NumberTruncateFunction();
					this.Visit(expression.Arguments[0]);
					newFunction.Argument = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "Compare":
				{
					this.Visit(Expression.Condition(
						Expression.Equal(expression.Arguments[0], expression.Arguments[1]),
						Expression.Constant(0),
						Expression.Condition(
							Expression.LessThan(expression.Arguments[0], expression.Arguments[1]),
							Expression.Constant(-1),
							Expression.Constant(1)
							)));
					return true;
				}
			}

			return false;
		}

		private bool VisitMathMethodCall(MethodCallExpression expression)
		{
			switch (expression.Method.Name)
			{
				case "Log":
				{
					var newFunction = new NumberLogFunction();
					this.Visit(expression.Arguments[0]);
					newFunction.Argument = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "Log10":
				{
					var newFunction = new NumberLog10Function();
					this.Visit(expression.Arguments[0]);
					newFunction.Argument = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "Sign":
				{
					var newFunction = new NumberSignFunction();
					this.Visit(expression.Arguments[0]);
					newFunction.Argument = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "Exp":
				{
					var newFunction = new NumberExponentialFunction();
					this.Visit(expression.Arguments[0]);
					newFunction.Argument = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "Sqrt":
				{
					var newFunction = new NumberRootFunction();
					this.Visit(expression.Arguments[0]);
					newFunction.Argument = this.Stack.Pop();
					newFunction.Root = new ConstantPart(2);
					this.Stack.Push(newFunction);
					return true;
				}
				case "Pow":
				{
					var newFunction = new NumberPowerFunction();
					this.Visit(expression.Arguments[0]);
					newFunction.Argument = this.Stack.Pop();
					this.Visit(expression.Arguments[1]);
					newFunction.Power = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "Abs":
				{
					var newFunction = new NumberAbsoluteFunction();
					this.Visit(expression.Arguments[0]);
					newFunction.Argument = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "Ceiling":
				{
					var newFunction = new NumberCeilingFunction();
					this.Visit(expression.Arguments[0]);
					newFunction.Argument = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "Floor":
				{
					var newFunction = new NumberFloorFunction();
					this.Visit(expression.Arguments[0]);
					newFunction.Argument = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "Round":
				{
					var newFunction = new NumberRoundFunction();
					this.Visit(expression.Arguments[0]);
					newFunction.Argument = this.Stack.Pop();
					if (expression.Arguments.Count == 2 && expression.Arguments[1].Type == typeof(int))
					{
						this.Visit(expression.Arguments[1]);
						newFunction.Precision = this.Stack.Pop();
					}
					else
					{
						// TODO: Make it consistent where these are set
						// should they be defaults here, or in the function class, or when making the sql
						// probably when making the sql, because the appropriate default will differ between platforms
						newFunction.Precision = new ConstantPart(0);
					}
					this.Stack.Push(newFunction);
					return true;
				}
				case "Truncate":
				{
					var newFunction = new NumberTruncateFunction();
					this.Visit(expression.Arguments[0]);
					newFunction.Argument = this.Stack.Pop();
					this.Stack.Push(newFunction);
					return true;
				}
				case "Sin":
				case "Cos":
				case "Tan":
				case "Acos":
				case "Asin":
				case "Atan":
				case "Atan2":
				{
					var newFunction = new NumberTrigFunction();
					this.Visit(expression.Arguments[0]);
					newFunction.Argument = this.Stack.Pop();
					if (expression.Arguments.Count > 1)
					{
						this.Visit(expression.Arguments[1]);
						newFunction.Argument2 = this.Stack.Pop();
					}
					newFunction.Function = (TrigFunction)Enum.Parse(typeof(TrigFunction), expression.Method.Name);
					this.Stack.Push(newFunction);
					return true;
				}
			}

			return false;
		}

		private bool VisitToStringMethodCall(MethodCallExpression expression)
		{
			if (expression.Object.Type == typeof(string))
			{
				this.Visit(expression.Object);
			}
			else
			{
				var newFunction = new ConvertFunction();
				this.Visit(expression.Arguments[0]);
				newFunction.Expression = (SourceExpression)this.Stack.Pop();
				this.Stack.Push(newFunction);
			}
			return true;
		}

		private bool VisitEqualsMethodCall(MethodCallExpression expression)
		{
			var condition = new Condition();
			condition.Operator = SqlOperator.Equals;
			if (expression.Method.IsStatic && expression.Method.DeclaringType == typeof(object))
			{
				this.Visit(expression.Arguments[0]);
				condition.Field = this.Stack.Pop();
				this.Visit(expression.Arguments[1]);
				condition.Value = this.Stack.Pop();
			}
			else if (!expression.Method.IsStatic && expression.Arguments.Count > 0 && expression.Arguments[0].Type == expression.Object.Type)
			{
				// TODO: Get the other arguments, most importantly StringComparison
				this.Visit(expression.Object);
				condition.Field = this.Stack.Pop();
				this.Visit(expression.Arguments[0]);
				condition.Value = this.Stack.Pop();
			}
			this.Stack.Push(condition);
			return true;
		}

		private bool VisitCompareToMethodCall(MethodCallExpression expression)
		{
			var newFunction = new StringCompareFunction();
			this.Visit(expression.Object);
			newFunction.Argument = this.Stack.Pop();
			this.Visit(expression.Arguments[0]);
			newFunction.Other = this.Stack.Pop();
			this.Stack.Push(newFunction);
			return true;
		}

		private bool VisitCompareMethodCall(MethodCallExpression expression)
		{
			var newFunction = new StringCompareFunction();
			this.Visit(expression.Arguments[0]);
			newFunction.Argument = this.Stack.Pop();
			this.Visit(expression.Arguments[1]);
			newFunction.Other = this.Stack.Pop();
			this.Stack.Push(newFunction);
			return true;
		}

		protected override Expression VisitNew(NewExpression expression)
		{
			if (expression.Type == typeof(DateTime))
			{
				// It's a date, so put its arguments into a DateNewFunction
				var function = new DateNewFunction();
				if (expression.Arguments.Count == 3)
				{
					this.Visit(expression.Arguments[0]);
					function.Year = this.Stack.Pop();
					this.Visit(expression.Arguments[1]);
					function.Month = this.Stack.Pop();
					this.Visit(expression.Arguments[2]);
					function.Day = this.Stack.Pop();
				}
				else if (expression.Arguments.Count == 6)
				{
					this.Visit(expression.Arguments[0]);
					function.Year = this.Stack.Pop();
					this.Visit(expression.Arguments[1]);
					function.Month = this.Stack.Pop();
					this.Visit(expression.Arguments[2]);
					function.Day = this.Stack.Pop();
					this.Visit(expression.Arguments[3]);
					function.Hour = this.Stack.Pop();
					this.Visit(expression.Arguments[4]);
					function.Minute = this.Stack.Pop();
					this.Visit(expression.Arguments[5]);
					function.Second = this.Stack.Pop();
				}
				this.Stack.Push(function);
				return expression;
			}
			else if (expression.Arguments.Count > 0)
			{
				// It's a new anonymous object, so get its properties as columns
				var fields = new FieldCollection();
				foreach (var argument in expression.Arguments)
				{
					this.Visit(argument);
					fields.Add((SourceExpression)this.Stack.Pop());
				}
				this.Stack.Push(fields);
				return expression;
			}

			return base.VisitNew(expression);
		}

		protected override Expression VisitUnary(UnaryExpression expression)
		{
			switch (expression.NodeType)
			{
				case ExpressionType.Not:
				{
					var newOperation = new UnaryOperation();
					newOperation.Operator = UnaryOperator.Not;
					Visit(expression.Operand);

					newOperation.Expression = this.Stack.Pop();
					// Push the condition onto the stack instead
					if (newOperation.Expression is Condition newCondition)
					{
						newCondition.Not = true;
						this.Stack.Push(newCondition);
					}
					else
					{
						this.Stack.Push(newOperation);
					}
					return expression;
				}
				case ExpressionType.Negate:
				case ExpressionType.NegateChecked:
				{
					var newOperation = new UnaryOperation();
					newOperation.Operator = UnaryOperator.Negate;
					Visit(expression.Operand);
					newOperation.Expression = this.Stack.Pop();
					this.Stack.Push(newOperation);
					return expression;
				}
				case ExpressionType.UnaryPlus:
				{
					Visit(expression.Operand);
					return expression;
				}
				case ExpressionType.Convert:
				{
					// Ignore conversions for now
					Visit(expression.Operand);
					return expression;
				}
				default:
				{
					throw new NotSupportedException($"The unary operator '{expression.NodeType}' is not supported");
				}
			}
		}

		protected override Expression VisitQuerySourceReference(QuerySourceReferenceExpression expression)
		{
			var tableName = expression.ReferencedQuerySource.ItemName.Replace("<generated>", "g");
			var columnName = this.Configuration.GetPrimaryKeyColumnName(expression.Type);
			var newColumn = new Column(tableName, columnName);
			this.Stack.Push(newColumn);

			return base.VisitQuerySourceReference(expression);
		}

		protected override Expression VisitSubQuery(SubQueryExpression expression)
		{
			if (expression.QueryModel.ResultOperators.Count > 0 &&
				expression.QueryModel.ResultOperators[0] is Remotion.Linq.Clauses.ResultOperators.ContainsResultOperator contains)
			{
				// It's an Array.Contains, so we need to convert the subquery into a condition
				var newCondition = new Condition();
				newCondition.Operator = SqlOperator.IsIn;

				Visit(contains.Item);
				newCondition.Field = this.Stack.Pop();

				if (TypeHelper.IsGenericType(expression.QueryModel.MainFromClause.FromExpression.Type, typeof(IQueryable<>)))
				{
					// Create the sub-select statement
					var subselect = StatementCreator.Visit(expression.QueryModel, this.Configuration, true);
					subselect.IsContains = false;
					if (subselect.SourceFields.Count == 0)
					{
						var subselectField = expression.QueryModel.SelectClause.Selector;
						Visit(subselectField);
						subselect.SourceFields.Add((SourceExpression)this.Stack.Pop());
					}
					newCondition.Value = subselect;
				}
				else
				{
					// Just check in the array that was passed
					Visit(expression.QueryModel.MainFromClause.FromExpression);
					newCondition.Value = this.Stack.Pop();
				}

				this.Stack.Push(newCondition);
			}

			return base.VisitSubQuery(expression);
		}

#if DEBUG

		// NOTE: I got sick of re-adding these everytime I wanted to figure out what was going on, so
		// I'm leaving them here in debug only

		protected override Expression VisitBlock(BlockExpression node)
		{
			BreakpointHook();
			return base.VisitBlock(node);
		}

		protected override CatchBlock VisitCatchBlock(CatchBlock node)
		{
			BreakpointHook();
			return base.VisitCatchBlock(node);
		}

		protected override Expression VisitDebugInfo(DebugInfoExpression node)
		{
			BreakpointHook();
			return base.VisitDebugInfo(node);
		}

		protected override Expression VisitDefault(DefaultExpression node)
		{
			BreakpointHook();
			return base.VisitDefault(node);
		}

		protected override Expression VisitDynamic(DynamicExpression node)
		{
			BreakpointHook();
			return base.VisitDynamic(node);
		}

		protected override ElementInit VisitElementInit(ElementInit node)
		{
			BreakpointHook();
			return base.VisitElementInit(node);
		}

		protected override Expression VisitExtension(Expression node)
		{
			BreakpointHook();
			return base.VisitExtension(node);
		}

		protected override Expression VisitGoto(GotoExpression node)
		{
			BreakpointHook();
			return base.VisitGoto(node);
		}

		protected override Expression VisitIndex(IndexExpression node)
		{
			BreakpointHook();
			return base.VisitIndex(node);
		}

		protected override Expression VisitInvocation(InvocationExpression node)
		{
			BreakpointHook();
			return base.VisitInvocation(node);
		}

		protected override Expression VisitLabel(LabelExpression node)
		{
			BreakpointHook();
			return base.VisitLabel(node);
		}

		protected override LabelTarget VisitLabelTarget(LabelTarget node)
		{
			BreakpointHook();
			return base.VisitLabelTarget(node);
		}

		protected override Expression VisitLambda<T>(Expression<T> node)
		{
			BreakpointHook();
			return base.VisitLambda(node);
		}

		protected override Expression VisitListInit(ListInitExpression node)
		{
			BreakpointHook();
			return base.VisitListInit(node);
		}

		protected override Expression VisitLoop(LoopExpression node)
		{
			BreakpointHook();
			return base.VisitLoop(node);
		}

		protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
		{
			BreakpointHook();
			return base.VisitMemberAssignment(node);
		}

		protected override MemberBinding VisitMemberBinding(MemberBinding node)
		{
			BreakpointHook();
			return base.VisitMemberBinding(node);
		}

		protected override Expression VisitMemberInit(MemberInitExpression node)
		{
			BreakpointHook();
			return base.VisitMemberInit(node);
		}

		protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
		{
			BreakpointHook();
			return base.VisitMemberListBinding(node);
		}

		protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
		{
			BreakpointHook();
			return base.VisitMemberMemberBinding(node);
		}

		protected override Expression VisitNewArray(NewArrayExpression node)
		{
			BreakpointHook();
			return base.VisitNewArray(node);
		}

		protected override Expression VisitParameter(ParameterExpression node)
		{
			BreakpointHook();
			return base.VisitParameter(node);
		}

		protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
		{
			BreakpointHook();
			return base.VisitRuntimeVariables(node);
		}

		protected override Expression VisitSwitch(SwitchExpression node)
		{
			BreakpointHook();
			return base.VisitSwitch(node);
		}

		protected override SwitchCase VisitSwitchCase(SwitchCase node)
		{
			BreakpointHook();
			return base.VisitSwitchCase(node);
		}

		protected override Expression VisitTry(TryExpression node)
		{
			BreakpointHook();
			return base.VisitTry(node);
		}

		protected override Expression VisitTypeBinary(TypeBinaryExpression node)
		{
			BreakpointHook();
			return base.VisitTypeBinary(node);
		}

		// When creating statement parts, put a breakpoint here if you would like to debug
		protected void BreakpointHook()
		{
		}
#endif
	}
}
