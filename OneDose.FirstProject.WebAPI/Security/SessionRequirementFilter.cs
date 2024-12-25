using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OneDose.FirstProject.WebAPI.Security
{
    public class SessionRequirementFilter : IAuthorizationFilter
    {
        private readonly SessionValidationService _sessionValidationService;

        public SessionRequirementFilter(SessionValidationService sessionValidationService)
        {
            _sessionValidationService = sessionValidationService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!_sessionValidationService.ContainsCustomHeader(context, "X-Session-Id")|| !_sessionValidationService.ContainsTokenHeader(context,"Token"))
            {
                context.Result = new UnauthorizedObjectResult( "UnAuthorized request");
                return;
            }
        }
    }
}
