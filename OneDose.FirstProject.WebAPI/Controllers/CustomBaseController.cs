using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneDose.FirstProject.WebAPI.Model;
using System.Net;

namespace OneDose.FirstProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        [NonAction] //bu bi endpoint değil
        public IActionResult CreateActionResult<t>(CustomResponseDTO<t> response)
        {
            if (response.StatusCode == (int)HttpStatusCode.NoContent)
                return new ObjectResult(null)
                {
                    StatusCode = response.StatusCode,
                };

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode,
            };
        }
    }
}
