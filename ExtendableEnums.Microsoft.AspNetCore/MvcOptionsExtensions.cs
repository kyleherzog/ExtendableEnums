using System;
using Microsoft.AspNetCore.Mvc;

namespace ExtendableEnums.Microsoft.AspNetCore;

/// <summary>
/// Provides extention methods for adding ExtendableEnum support to <see cref="MvcOptions"/>.
/// </summary>
public static class MvcOptionsExtensions
{
    /// <summary>
    /// Adds the Asp.net MVC model binding providers for ExtendableEnum object.
    /// </summary>
    /// <param name="options">The <see cref="MvcOptions"/> for which to add the model binding provider.</param>
    public static void UseExtendableEnumModelBinding(this MvcOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        options.ModelBinderProviders.Insert(0, new ExtendableEnumModelBinderProvider());
    }
}