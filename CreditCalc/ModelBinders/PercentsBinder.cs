using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CreditCalc.ModelBinders
{
    /// <summary>
    /// Binder of double value from string. Strings with '%' symbol parsed as percents, else parsed as fraction
    /// </summary>
    public class PercentsBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var modelName = bindingContext.ModelName;

            // Try to fetch the value of the argument by name
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var val = valueProviderResult.FirstValue.Trim();
            // Check if the argument value is null or empty
            if (string.IsNullOrEmpty(val))
                return Task.CompletedTask;

            //If contains '%' symbol, remove it
            var withPercent = val.Last() == '%';
            if (withPercent)
                val = val.Remove(val.Length - 1);

            //Parse value
            if (!double.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsed))
            {
                bindingContext.ModelState.TryAddModelError(modelName,
                    "Percents must be a float value with or without '%' symbol at the end.");
                return Task.CompletedTask;
            }

            //Divide it by 100
            if (withPercent)
                parsed /= 100;

            bindingContext.Result = ModelBindingResult.Success(parsed);
            return Task.CompletedTask;
        }
    }
}
