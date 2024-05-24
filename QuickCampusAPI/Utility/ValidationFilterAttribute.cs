using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;

namespace QuickCampusAPI.Utility
{
    public class ValidationFilterAttribute: IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            IGeneralResult<dynamic> result = new GeneralResult<dynamic>();
            //context.ModelState.AddModelError(result.Success, bool.FalseString);
            context.Result = new UnprocessableEntityObjectResult(context.ModelState);
        }
    }
    public void OnActionExecuted(ActionExecutedContext context) { }
}
}
