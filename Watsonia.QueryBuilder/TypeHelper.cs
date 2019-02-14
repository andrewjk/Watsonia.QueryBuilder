using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// Contains helper methods for dealing with types.
	/// </summary>
	public static class TypeHelper
	{
		///// <summary>
		///// Finds any interfaces of type IEnumerable on a type.
		///// </summary>
		///// <param name="sequenceType">The type to search for IEnumerable.</param>
		///// <returns></returns>
		//public static Type FindIEnumerable(Type sequenceType)
		//{
		//	if (sequenceType == null || sequenceType == typeof(string))
		//	{
		//		return null;
		//	}

		//	if (sequenceType.IsArray)
		//	{
		//		return typeof(IEnumerable<>).MakeGenericType(sequenceType.GetElementType());
		//	}

		//	if (sequenceType.IsGenericType)
		//	{
		//		foreach (Type arg in sequenceType.GetGenericArguments())
		//		{
		//			Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
		//			if (ienum.IsAssignableFrom(sequenceType))
		//			{
		//				return ienum;
		//			}
		//		}
		//	}

		//	Type[] interfaces = sequenceType.GetInterfaces();
		//	if (interfaces != null && interfaces.Length > 0)
		//	{
		//		foreach (Type iface in interfaces)
		//		{
		//			Type ienum = FindIEnumerable(iface);
		//			if (ienum != null)
		//				return ienum;
		//		}
		//	}

		//	if (sequenceType.BaseType != null && sequenceType.BaseType != typeof(object))
		//	{
		//		return FindIEnumerable(sequenceType.BaseType);
		//	}

		//	return null;
		//}

		///// <summary>
		///// Gets the type of element contained in a sequence.
		///// </summary>
		///// <param name="sequenceType">The type of the sequence, which must implement an IEnumerable interface.</param>
		///// <returns></returns>
		//public static Type GetElementType(Type sequenceType)
		//{
		//	Type enumerableType = FindIEnumerable(sequenceType);
		//	if (enumerableType == null)
		//	{
		//		return sequenceType;
		//	}
		//	else
		//	{
		//		return enumerableType.GetGenericArguments()[0];
		//	}
		//}

		/// <summary>
		/// Determines whether the specified type is nullable.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if the specified type is nullable; otherwise, <c>false</c>.</returns>
		public static bool IsNullableType(Type type)
		{
			return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		/// <summary>
		/// Gets a non-nullable version of the supplied type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static Type GetNonNullableType(Type type)
		{
			if (IsNullableType(type))
			{
				return type.GetGenericArguments()[0];
			}
			return type;
		}

		///// <summary>
		///// Determines whether the specified type is boolean.
		///// </summary>
		///// <param name="type">The type.</param>
		///// <returns>
		/////   <c>true</c> if the specified type is boolean; otherwise, <c>false</c>.
		///// </returns>
		//public static bool IsBoolean(Type type)
		//{
		//	return Type.GetTypeCode(type) == TypeCode.Boolean;
		//}

		//public static bool IsInteger(Type type)
		//{
		//	Type nnType = GetNonNullableType(type);
		//	switch (Type.GetTypeCode(type))
		//	{
		//		case TypeCode.SByte:
		//		case TypeCode.Int16:
		//		case TypeCode.Int32:
		//		case TypeCode.Int64:
		//		case TypeCode.Byte:
		//		case TypeCode.UInt16:
		//		case TypeCode.UInt32:
		//		case TypeCode.UInt64:
		//		{
		//			return true;
		//		}
		//		default:
		//		{
		//			return false;
		//		}
		//	}
		//}

		///// <summary>
		///// Determines whether the specified type is numeric.
		///// </summary>
		///// <param name="type">The type.</param>
		///// <returns>
		/////   <c>true</c> if the specified type is numeric; otherwise, <c>false</c>.
		///// </returns>
		//public static bool IsNumeric(Type type)
		//{
		//	switch (Type.GetTypeCode(type))
		//	{
		//		case TypeCode.Byte:
		//		case TypeCode.Decimal:
		//		case TypeCode.Double:
		//		case TypeCode.Int16:
		//		case TypeCode.Int32:
		//		case TypeCode.Int64:
		//		case TypeCode.SByte:
		//		case TypeCode.Single:
		//		case TypeCode.UInt16:
		//		case TypeCode.UInt32:
		//		case TypeCode.UInt64:
		//		{
		//			return true;
		//		}
		//		default:
		//		{
		//			return false;
		//		}
		//	}
		//}

		//public static bool IsAnonymous(Type type)
		//{
		//	// From http://stackoverflow.com/questions/2483023/how-to-test-if-a-type-is-anonymous
		//	// HACK: The only way to detect anonymous types right now.
		//	return
		//		Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false) &&
		//		type.IsGenericType &&
		//		type.Name.Contains("AnonymousType") &&
		//		(type.Name.StartsWith("<>") || type.Name.StartsWith("VB$")) &&
		//		(type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
		//}

		//public static bool IsGenericType(Type type, Type genericType)
		//{
		//	// From http://stackoverflow.com/questions/982487/testing-if-object-is-of-generic-type-in-c-sharp
		//	while (type != null)
		//	{
		//		if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
		//		{
		//			return true;
		//		}
		//		if (genericType.IsInterface && IsAssignableToGenericType(type, genericType))
		//		{
		//			return true;
		//		}
		//		type = type.BaseType;
		//	}
		//	return false;
		//}

		//public static bool IsAssignableToGenericType(Type type, Type genericType)
		//{
		//	// From http://stackoverflow.com/questions/5461295/using-isassignablefrom-with-generics
		//	var interfaceTypes = type.GetInterfaces();

		//	foreach (var it in interfaceTypes)
		//	{
		//		if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
		//		{
		//			return true;
		//		}
		//	}

		//	if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
		//	{
		//		return true;
		//	}

		//	Type baseType = type.BaseType;
		//	if (baseType == null)
		//	{
		//		return false;
		//	}

		//	return IsAssignableToGenericType(baseType, genericType);
		//}

		///// <summary>
		///// Returns an object of the specified type and whose value is equivalent to the specified object.
		///// </summary>
		///// <param name="value">An object that implements the System.IConvertible interface.</param>
		///// <param name="conversionType">The type of object to return.</param>
		///// <returns>
		///// An object whose type is conversionType and whose value is equivalent to value.-or-A
		///// null reference (Nothing in Visual Basic), if value is null and conversionType
		///// is not a value type.
		///// </returns>
		//public static object ChangeType(object value, Type conversionType)
		//{
		//	if (value == null || value == DBNull.Value)
		//	{
		//		// TODO: Maybe not...
		//		// It would be better to make this generic and pass in the default value
		//		// But that would involve changing emitted code
		//		return null;
		//	}

		//	Type safeType = Nullable.GetUnderlyingType(conversionType) ?? conversionType;
		//	if (safeType.IsEnum)
		//	{
		//		return Enum.ToObject(safeType, value);
		//	}
		//	else
		//	{
		//		return Convert.ChangeType(value, safeType);
		//	}
		//}
	}
}
