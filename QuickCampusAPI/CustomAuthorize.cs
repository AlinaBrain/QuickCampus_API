//using Microsoft.AspNetCore.Authorization;
//using System.Net;
//using System.Web.Http.Controllers;

//namespace QuickCampusAPI
//{
//    public class Authorize : AuthorizeAttribute
//    {
//        protected override void Authorize(HttpActionContext actionContext)
//        {
//            actionContext.Response = new HttpResponseMessage
//            {
//                StatusCode = HttpStatusCode.Forbidden,
//                Content = new StringContent("You are unauthorized to access this resource")
//            };
//        }
//    }
//}
