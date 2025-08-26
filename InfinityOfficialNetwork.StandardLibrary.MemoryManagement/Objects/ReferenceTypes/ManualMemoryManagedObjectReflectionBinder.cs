using System.Reflection;

namespace InfinityOfficialNetwork.StandardLibrary.MemoryManagement.Objects.ReferenceTypes
{
	internal static class ManualMemoryManagedObjectReflectionBinder<T>
	{
		static ManualMemoryManagedObjectReflectionBinder()
		{
			Type type = typeof(T);
			Constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
		}

		private static ConstructorInfo[] Constructors { get; }

		public static class BindDefaultConstructor
		{
			static BindDefaultConstructor()
			{
				ConstructorInfo? constructor = Constructors.Where(i => i.GetParameters().Length == 0).FirstOrDefault();
				if (constructor != null)
				{

				}
				else
					throw new InvalidOperationException("Default constructor not found");
			}

			public static Action<object?, object?[]?> Constructor { get; }
		}
	}
}
