using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace E_Commerce.API.Factories
{
    public class ApiResponceFactory
    {
        public static IActionResult CustonValidationErrorResponse(ActionContext context)
        {
            //context=> errors,key [field]   
            //context.ModelState ==> <string , ModelStateEntry>
            //string ==> name of the field
            //ModelStateEntry ==> Errors ==> Error Messages
            //Ienumerable<Validationerror> ==> ErrorMessage
            var errors = context.ModelState.Where(e => e.Value?.Errors.Any()==true)
                .Select(e => new ValidationErrors
                {
                    Field = e.Key,
                    Errors = e.Value?.Errors.Select(er => er.ErrorMessage)?? new List<string>()
                });
            var response = new ValidationErrorResponse
                {
                StatusCode = StatusCodes.Status400BadRequest,
                ErrorMessage = "One or more validation errors occurred.",
                Errors = errors
            };
            return new BadRequestObjectResult(response);
        }
    }
}
