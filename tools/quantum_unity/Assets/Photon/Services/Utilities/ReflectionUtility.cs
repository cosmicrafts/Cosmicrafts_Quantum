namespace Quantum.Services
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	public static partial class ReflectionUtility
	{
		public static List<Type> GetInheritedTypes(Type baseType, bool includeAbstract = true)
		{
			return GetInheritedTypes(baseType, baseType.Assembly, includeAbstract);
		}

		public static List<Type> GetInheritedTypes(Type baseType, Assembly assembly, bool includeAbstract = true)
		{
			List<Type> inheritedTypes = new List<Type>(16);

			foreach (Type type in assembly.GetTypes())
			{
				if (type.IsSubclassOf(baseType) == true)
				{
					if (includeAbstract == false && type.IsAbstract == true)
						continue;

					inheritedTypes.Add(type);
				}
			}

			return inheritedTypes;
		}

		public static List<Type> GetAssignableTypes(Type baseType, bool includeAbstract = true)
		{
			return GetAssignableTypes(baseType, baseType.Assembly, includeAbstract);
		}

		public static List<Type> GetAssignableTypes(Type baseType, Assembly assembly, bool includeAbstract = true)
		{
			List<Type> assignableTypes = new List<Type>(16);

			foreach (Type type in assembly.GetTypes())
			{
				if (baseType.IsAssignableFrom(type) == true)
				{
					if (includeAbstract == false && type.IsAbstract == true)
						continue;

					assignableTypes.Add(type);
				}
			}

			return assignableTypes;
		}
	}
}
