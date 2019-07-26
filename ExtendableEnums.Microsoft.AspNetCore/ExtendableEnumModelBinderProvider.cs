using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExtendableEnums.Microsoft.AspNetCore
{
    public class ExtendableEnumModelBinderProvider : IModelBinderProvider
    {
        public static bool IsTypeDerivedFromGenericType(Type typeToCheck, Type genericType)
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

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (IsTypeDerivedFromGenericType(context.Metadata.ModelType, typeof(ExtendableEnumBase<,>)))
            {
                return new ExtendableEnumBinder();
            }

            return null;
        }
    }
}
