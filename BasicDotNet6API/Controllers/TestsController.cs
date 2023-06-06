using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicDotNet6API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        
        public TestsController()
        { 
        }
        // Query string
        // ex: https://localhost:44322/api/Tests?id=123
        [HttpGet]
        public ActionResult Test() {
            return Ok(); 
        }

        // path，優先於 query string
        // https://localhost:44322/api/Tests/123
        [HttpGet("{id}")]
        public ActionResult TestOfId(bool id)
        {
            return Ok(id);
        }

        // path
        // https://localhost:44322/api/Path/SubPath
        [HttpDelete("Path/SubPath")]
        public ActionResult TestSubPath()
        {
            return BadRequest();
        }

        //Body
        [HttpPost]
        public ActionResult TestBody(Body body)
        {
            return Ok(body);
        }

        // TODO header body, basic auth, A&A(jwt), exceptionHandleFactory, mongo client, 
        // TODO postman 整合測試
    }

   public  class Body
    {
        public string Name { get; set; }
    }
}
