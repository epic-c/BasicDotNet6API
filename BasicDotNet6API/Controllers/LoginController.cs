using BasicDotNet6API.Helpers;
using BasicDotNet6API.Models.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BasicDotNet6API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly JwtHelpers _jwtHelpers;

        public LoginController(ILogger<LoginController> logger, JwtHelpers jwtHelpers) {
            _logger = logger;
            _jwtHelpers = jwtHelpers;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<Token> Login() {
            var authorization =  HttpContext.Request.Headers["Authorization"];

            var userValue = Encoding.UTF8.GetString(Convert.FromBase64String( authorization.ToString()[6..])).Split(":");

            
            return Ok(new Token() {JwtToken = _jwtHelpers.GenerateToken(userValue[0]) });
        }
    }
}
