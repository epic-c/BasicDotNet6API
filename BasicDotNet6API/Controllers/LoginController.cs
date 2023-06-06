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
        private readonly BasicAuthHelpers basicAuthHelpers;

        public LoginController(ILogger<LoginController> logger, JwtHelpers jwtHelpers, BasicAuthHelpers basicAuthHelpers) {
            _logger = logger;
            _jwtHelpers = jwtHelpers;
            this.basicAuthHelpers = basicAuthHelpers;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<Token> Login() {
            var (user, _) = basicAuthHelpers.Parse(HttpContext.Request);

            return Ok(new Token() {JwtToken = _jwtHelpers.GenerateToken(user) });
        }
    }
}
