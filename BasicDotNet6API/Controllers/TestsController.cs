using BasicDotNet6API.Exceptions;
using BasicDotNet6API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BasicDotNet6API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        IMongoCollection<User> client;


        public TestsController()
        {
            client = new MongoClient("mongodb://127.0.0.1:27017").GetDatabase("Hello").GetCollection<User>("User");
        }
        // Query string

        //Body
        [HttpPost]
        public async Task<ActionResult> TestBody(Body body)
        {
            await client.InsertOneAsync(new User { Name = "epic" });
            return Ok(body);
        }

        // ex: https://localhost:44322/api/Tests?id=123
        [HttpGet]
        public async Task<ActionResult> Test()
        {
            var result = await client.Find(i => i.Name == "epik").FirstOrDefaultAsync();

           return Ok(result);
            //throw new YourBussException("this is test");
            //return Ok(); 
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



        // TODO header body, basic auth, A&A(jwt), exceptionHandleFactory, mongo client, 
        // TODO postman 整合測試(測試腳本)
    }

    public class Body
    {
        public string Name { get; set; }
    }
}
