using System.Reflection;

namespace Mundos {
    public static class Util {

        /// <summary>
        /// Retrieves an array of types that are subclasses of the specified type.
        /// </summary>
        /// <param name="MyType">The base type to check for subclasses.</param>
        /// <param name="Assembly">The assembly to search for subclasses. If null, the executing assembly is used.</param>
        /// <returns>An array of types that are subclasses of the specified type.</returns>
        public static Type[] GetInheritedClasses(Type MyType, Assembly? Assembly = null)
        {
            if (Assembly == null)
                Assembly = Assembly.GetExecutingAssembly();

            List<Type> types = new List<Type>();
            foreach (Type type in Assembly.GetTypes())
            {
                if (type.IsSubclassOf(MyType))
                    types.Add(type);
            }
            return types.ToArray();
        }
    }
}