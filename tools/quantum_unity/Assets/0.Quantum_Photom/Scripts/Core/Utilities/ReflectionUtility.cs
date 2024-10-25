namespace TowerRush.Core
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	public static class ReflectionUtils
	{
		// PUBLIC METHODS

		static public List<Type> GetDerivedTypes(Assembly[] assemblies, Type baseType, bool includeBase, bool includeAbstract)
		{
			if (baseType == null)
				return null;

			var result = new List<Type>(8);

			if (includeBase == true && (includeAbstract == true || baseType.IsAbstract == false))
				result.Add(baseType);

			if (assemblies == null)
			{
				GetDerivedTypes(baseType.Assembly, baseType, includeAbstract, result);
			}
			else
			{
				foreach (Assembly asm in assemblies)
				{
					GetDerivedTypes(asm, baseType, includeAbstract, result);
				}
			}

			return result;
		}

		public static bool HasAttribute<T>(Type type, bool inherited = false) where T : Attribute
		{
			var attribs = type.GetCustomAttributes(typeof(T), inherited);

			return attribs.Length > 0;
		}

		public static bool FieldHasAttribute<T>(Type parentType, string fieldName, bool inherited) where T : Attribute
		{
			if (parentType != null && string.IsNullOrEmpty(fieldName) != true)
			{
				var fieldInfo = parentType.GetField(fieldName);
				if (fieldInfo != null)
					return fieldInfo.HasAttribute<T>(inherited);
			}
			return false;
		}

		public static bool HasAttribute<T>(this FieldInfo fieldInfo, bool inherited) where T : Attribute
		{
			var attribs = fieldInfo.GetCustomAttributes(typeof(T), inherited);

			return attribs.Length > 0;
		}

		public static T GetFieldAttribute<T>(Type parentType, string fieldName, bool inherited) where T : Attribute
		{
			if (parentType != null && string.IsNullOrEmpty(fieldName) != true)
			{
				var fieldInfo = parentType.GetField(fieldName);
				if (fieldInfo != null)
					return fieldInfo.GetAttribute<T>(inherited);
			}
			return null;
		}

		public static T GetAttribute<T>(this FieldInfo fieldInfo, bool inherited) where T : Attribute
		{
			var attributes = fieldInfo.GetCustomAttributes(typeof(T), inherited);

			return attributes.Length > 0 ? attributes[0] as T : null;
		}

		public static void GetFieldsRecursive(this Type type, Type baseType, BindingFlags bindingFlags, IList<FieldInfo> fields)
		{
			if (type == null)
				return;

			if (bindingFlags.HasFlag(BindingFlags.Public) == true)
			{
				var customFlags = bindingFlags.RemoveFlag(BindingFlags.NonPublic);

				foreach (var fieldInfo in type.GetFields(customFlags))
				{
					fields.Add(fieldInfo);
				}
			}

			if (bindingFlags.HasFlag(BindingFlags.NonPublic) == true)
			{
				var customFlags = bindingFlags.RemoveFlag(BindingFlags.Public);
				do
				{
					foreach (var fieldInfo in type.GetFields(customFlags))
					{
						fields.Add(fieldInfo);
					}

					type = type.BaseType;
				}
				while (type != baseType && type != null);
			}
		}

		// PRIVATE METHODS

		private static void GetDerivedTypes(Assembly asm, Type baseType, bool includeAbstract, List<Type> subTypes)
		{
			foreach (var type in asm.GetTypes())
			{
				if (type != baseType && baseType.IsAssignableFrom(type) == true)
				{
					AddDerivedType(type, includeAbstract, subTypes);
				}
			}
		}

		private static void AddDerivedType(Type type, bool includeAbstract, List<Type> subTypes)
		{
			if (type.IsAbstract == false || includeAbstract == true)
			{
				subTypes.Add(type);
			}
		}
	}

	public static class BindingFlagsExtension
	{
		public static bool         HasFlag   (this BindingFlags flags, BindingFlags flag) { return ((int)flags & (int)flag) == (int)flag; }
		public static BindingFlags SetFlag   (this BindingFlags flags, BindingFlags flag) { return flags | flag; }
		public static BindingFlags RemoveFlag(this BindingFlags flags, BindingFlags flag) { return flags & ~flag; }
	}
}