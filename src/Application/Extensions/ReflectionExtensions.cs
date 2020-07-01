using System;

namespace Application.Extensions
{
    static class ReflectionExtensions
    {
        public static bool IsSubclassOfGeneric(this Type type, Type genericTypeDefinition)
        {
            if (!genericTypeDefinition.IsGenericTypeDefinition || type.BaseType == null)
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
