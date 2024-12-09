using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExtendableEnums.Microsoft.AspNetCore;

/// <summary>
/// Defines an interface for ExtendableEnum model binders.
/// </summary>
public class ExtendableEnumBinder : IModelBinder
{
    private JsonSerializerOptions jsonSerializerOptions;

    public ExtendableEnumBinder(JsonSerializerOptions jsonSerializerOptions)
    {
        this.jsonSerializerOptions = jsonSerializerOptions;
    }

    /// <summary>
    /// Attempts to bind a model.
    /// </summary>
    /// <param name="bindingContext">The <see cref="ModelBindingContext"/>.</param>
    /// <returns>
    ///     A <see cref="Task"/> which will complete when the model binding process completes.
    ///
    ///     If model binding was successful, IsModelSet is set to <c>true</c>.
    /// </returns>
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext is null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var modelName = bindingContext.ModelName;

        // Try to fetch the value of the argument by name
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None || string.IsNullOrEmpty(valueProviderResult.FirstValue))
        {
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        var json = $"{{\"value\":\"{valueProviderResult.FirstValue}\"}}";
        var result = JsonSerializer.Deserialize(json, returnType: bindingContext.ModelType, options: this.jsonSerializerOptions);

        bindingContext.Result = ModelBindingResult.Success(result);

        return Task.CompletedTask;
    }
}