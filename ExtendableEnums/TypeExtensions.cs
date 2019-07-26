using System;

namespace ExtendableEnums
{
    /// <summary>
    /// Provides extension method on <see cref="Type"/> that relate to ExtendableEnums.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Checks to see if the the given type is derived from <see cref="ExtendableEnumBase{TEnumeration, TValue}" />.
        /// </summary>
        /// <param name="type">The <see cref="Type" /> to check to see if it is an <see cref="ExtendableEnumBase{TEnumeration, TValue}"/> decendant.</param>
        /// <returns><c>true</c> if the type inherits from <see cref="ExtendableEnumBase{TEnumeration, TValue}"/>, otherwise <c>false</c>.</returns>
        public static bool IsExtendableEnum(this Type type)
        {
            return IsTypeDerivedFromGenericType(type, typeof(ExtendableEnumBase<,>));
        }

        private static bool IsTypeDerivedFromGenericType(Type typeToCheck, Type genericType)
        {
            if (typeToCheck == typeof(object))
            {
                return false;
            }
            else if (typeToCheck == null)
            {
                return false;
            }
            else if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
            else
            {
                return IsTypeDerivedFromGenericType(typeToCheck.BaseType, genericType);
            }
        }
    }
}
