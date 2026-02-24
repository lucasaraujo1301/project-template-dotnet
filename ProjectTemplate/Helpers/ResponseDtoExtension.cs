using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections;
using ProjectTemplate.Application.Dto;

namespace ProjectTemplate.Helpers;

public static class ResponseViewModelExtension
{
    public static IActionResult ToActionResult<T, K>(this ResponseDto<T, K> result, ControllerBase controller)
    {
        if (result.IsSuccess)
            return controller.StatusCode(result.StatusCode, result.Data);

        if (result.Errors is null)
            return controller.StatusCode(result.StatusCode);

        var modelState = new ModelStateDictionary();

        foreach (var kv in result.Errors)
        {
            if (kv.Value is string singleError)
            {
                modelState.AddModelError(kv.Key, singleError);
            }
            else if (kv.Value is IEnumerable enumerableErrors)
            {
                foreach (var error in enumerableErrors)
                {
                    modelState.AddModelError(kv.Key, error?.ToString() ?? "");
                }
            }
            else if (kv.Value != null)
            {
                // For any other type, just add its string representation
                modelState.AddModelError(kv.Key, kv.Value.ToString() ?? "");
            }
        }

        var problemDetails = new ValidationProblemDetails(modelState)
        {
            Status = result.StatusCode,
            Title = "Request failed"
        };

        return controller.StatusCode(result.StatusCode, problemDetails);
    }
}