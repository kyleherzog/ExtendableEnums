using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExtendableEnums.Microsoft.AspNetCore
{
    /// <summary>
    /// The <see cref="IModelBinderProvider"/> that provides model binding for ExtendableEnums.
    /// </summary>
    public class ExtendableEnumModelBinderProvider : IModelBinderProvider
    {
        /// <summary>
        /// Creates a IModelBinder based on ModelBinderProviderContext.
        /// </summary>
        /// <param name="context">The current <see cref="ModelBinderProviderContext"/>.</param>
        /// <returns>An <see cref="ExtendableEnumBinder"/> if the model type derives from <see cref="ExtendableEnumBase{TEnumeration, TValue}"/>, otherwise null.</returns>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType.IsExtendableEnum())
            {
                return new ExtendableEnumBinder();
            }

            return null;
        }
    }
}
