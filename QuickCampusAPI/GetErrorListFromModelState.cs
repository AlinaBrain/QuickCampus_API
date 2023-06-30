using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace QuickCampusAPI
{
    public class GetErrorListFromModelState
    {
        public static string GetErrorList(ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.FirstOrDefault();
            return errorList;
        }
    }
}
