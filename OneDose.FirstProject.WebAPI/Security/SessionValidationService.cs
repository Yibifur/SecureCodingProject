using Microsoft.AspNetCore.Mvc.Filters;

namespace OneDose.FirstProject.WebAPI.Security
{
    public class SessionValidationService
    {
        public bool ContainsCustomHeader(AuthorizationFilterContext context,string headerName)
            => context.HttpContext.Request.Headers[headerName].Any();
        public bool ContainsTokenHeader(AuthorizationFilterContext context, string Token)
           => context.HttpContext.Request.Headers[Token].Any();




    }
}
