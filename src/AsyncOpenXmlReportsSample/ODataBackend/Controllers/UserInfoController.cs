
namespace IIS.AsyncOpenXmlReportsSample.Controllers
{
    using System.Configuration;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    [ApiController]
    [Route("api/[controller]")]
    public class UserInfoController : ControllerBase
    {
        private ICSSoft.Services.CurrentUserService.IUser _userService;

        public UserInfoController(ICSSoft.Services.CurrentUserService.IUser service)
        {
            _userService = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var x = _userService.Login;
            return Ok(HttpContext.User.Claims.Select(x => new { x.Type, x.Value }));
        }
    }
}