using BasicDotNet6API.Exceptions;
using BasicDotNet6API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BasicDotNet6API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IMongoCollection<User> client;


        public UserController()
        {
            client = new MongoClient("mongodb://127.0.0.1:27017").GetDatabase("Hello").GetCollection<User>("User");
        }

        // Path
        [HttpGet("{name}")]
        public async Task<ActionResult<User>> GetUser(string name)
        {
            var result = await client.Find(i => i.Name == name).FirstOrDefaultAsync();

            if (result == null) 
                throw new YourBussException($"User: {name} not exist DB.");
           
            return Ok(result);
        }

        // Query string
        [HttpGet]
        public async Task<ActionResult<User>> GetUserOfId(string id)
        {
            var result = await client.Find(i => i.Id == id).FirstOrDefaultAsync();

            if (result == null)
                throw new YourBussException($"User: {id} not exist DB.");

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser(UserResquest user)
        {
            await client.InsertOneAsync(new User {
                Name  = user.Name,
                Password = user.Password,
                Description = user.Description
            });

            var result = await client.Find(i => i.Name == user.Name).FirstOrDefaultAsync();

            return Ok(result);
        }

        [HttpPut("{name}")]
        public async Task<ActionResult<User>> EditUser(string name, UserResquest user)
        {
            var getuser = await client.Find(i => i.Name == name).FirstOrDefaultAsync();

            var newUser = new User
            {
                Id = getuser.Id,
                Name = user.Name,
                Password = user.Password,
                Description = user.Description
            };

            var result = await client.ReplaceOneAsync(i => i.Id == getuser.Id, newUser);
            return Ok(newUser);
        }

        // path
        // https://localhost:44322/api/Path/SubPath
        [HttpDelete("{name}")]
        public async Task<ActionResult<string>> TestSubPath(string name)
        {
            var result = await client.DeleteOneAsync(i => i.Name == name);
            return Ok(name);
        }

        // TODO header body, basic auth, A&A(jwt), exceptionHandleFactory, mongo client, 
        // TODO postman 整合測試(測試腳本)
    }
}
