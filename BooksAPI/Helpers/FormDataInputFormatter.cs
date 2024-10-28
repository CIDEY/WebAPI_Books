using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

public class FormDataInputFormatter : InputFormatter
{
    public FormDataInputFormatter()
    {
        SupportedMediaTypes.Add("application/x-www-form-urlencoded");
    }

    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
    {
        var form = await context.HttpContext.Request.ReadFormAsync();
        var model = Activator.CreateInstance(context.ModelType);

        var properties = context.ModelType.GetProperties();
        foreach (var property in properties)
        {
            var fromFormAttribute = property.GetCustomAttribute<FromFormAttribute>();
            var key = fromFormAttribute?.Name ?? property.Name;

            if (form.TryGetValue(key, out var value))
            {
                property.SetValue(model, Convert.ChangeType(value, property.PropertyType));
            }
        }

        return InputFormatterResult.Success(model);
    }

    public override bool CanRead(InputFormatterContext context)
    {
        return context.HttpContext.Request.ContentType.StartsWith("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase);
    }
}