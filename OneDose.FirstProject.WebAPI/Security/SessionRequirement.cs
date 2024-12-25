using Microsoft.AspNetCore.Authorization;

namespace OneDose.FirstProject.WebAPI.Security
{
    public class SessionRequirement:IAuthorizationRequirement
    {
        public string? SessionName { get; set; }
        public string  Token { get; set; }
        
        public SessionRequirement(string? sessionName, string token)
        {
            SessionName = sessionName;
            Token = token;
        }
    }
}
