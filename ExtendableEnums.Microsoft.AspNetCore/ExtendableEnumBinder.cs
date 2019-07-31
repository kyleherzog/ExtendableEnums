using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace ExtendableEnums.Microsoft.AspNetCore
{
    /// <summary>
    /// Defines an interface for ExtendableEnum model binders.
    /// </summary>
    public class ExtendableEnumBinder : IModelBinder
    {
        /// <summary>
        /// Attempts to bind a model.
        /// </summary>
        /// <param name="bindingContext">The <see cref="ModelBindingContext"/>.</param>
        /// <returns>
        ///     A <see cref="Task"/> which will complete when the model binding process completes.
        ///
        ///     If model binding was successful, IsMOdelSet is set to <c>true</c>.
        /// </returns>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelName = bindingContext.ModelName;

            // Try to fetch the value of the argument by name
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            var result = JsonConvert.DeserializeObject($"'{valueProviderResult.FirstValue}'", bindingContext.ModelType);

            bindingContext.Result = ModelBindingResult.Success(result);

            return Task.CompletedTask;
        }
    }
}
