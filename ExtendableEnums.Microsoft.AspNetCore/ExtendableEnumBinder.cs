using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExtendableEnums.Microsoft.AspNetCore;

/// <summary>
/// Defines an interface for ExtendableEnum model binders.
/// </summary>
public class ExtendableEnumBinder : IModelBinder
{
    /// <summary>
    /// Attempts to bind a model.
    /// </summary>
    /// <param name="bindingContext">The <see cref="ModelBindingContext"/>.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext is null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None
            || string.IsNullOrEmpty(valueProviderResult.FirstValue))
        {
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        var converter = TypeDescriptor.GetConverter(bindingContext.ModelType);
        var result = converter.ConvertFromString(valueProviderResult.FirstValue);

        bindingContext.Result = ModelBindingResult.Success(result);

        return Task.CompletedTask;
    }
}