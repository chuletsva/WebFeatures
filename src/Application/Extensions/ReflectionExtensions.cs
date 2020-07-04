using System;

namespace Application.Extensions
{
    static class ReflectionExtensions
    {
        public static bool IsSubclassOfGeneric(this Type type, Type genericTypeDefinition)
        {
            if (type.BaseType == null || !genericTypeDefinition.IsGenericTypeDefinition)
            {
                return false;
            }

            if (type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == genericTypeDefinition)
            {
                return true;
            }

            return type.BaseType.IsSubclassOfGeneric(genericTypeDefinition);
        }
    }
}
