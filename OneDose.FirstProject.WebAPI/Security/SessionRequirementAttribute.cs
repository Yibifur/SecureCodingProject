using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OneDose.FirstProject.WebAPI.Security
{
    public class SessionRequirementAttribute : TypeFilterAttribute
    {
        public SessionRequirementAttribute():
            base(typeof(SessionRequirementFilter))
        {
            
        }
    }
}
