using System.Threading.Tasks;
using GrainInterfaces;
using Grains;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IClusterClient _client;

        public UserController(IClusterClient client)
        {
            _client = client;
        }

        [HttpPost]
        public async Task<string> RegisterUser(UserRegisterRequest request)
        {
            Console.WriteLine("RegisterUser on controller called while writing state:");
            var userGrain = _client.GetGrain<IUserGrain>("userGrain");
            return await userGrain.RegisterUser(request.UserName, request.Email, request.PostalCode, request.PhoneNumber);
        }

        [HttpGet("test/{test}")]
        public async Task<string> getTest(string test)
        {
            Console.WriteLine("GetTest called: "+test);
            return "test-result"+test;
        }

        [HttpGet("{id}")]
        public async Task<User> GetUserById(string id)
        {
            var userGrain = _client.GetGrain<IUserGrain>("userGrain");
            return await userGrain.GetUserById(id);
        }

        [HttpGet("username/{userName}")]
        public async Task<User> GetUserByUserName(string userName)
        {
            var userGrain = _client.GetGrain<IUserGrain>("userGrain");
            return await userGrain.GetUserByUserName(userName);
        }

        [HttpGet]
        public async Task<List<User>> GetAllUsers()
        {
            var userGrain = _client.GetGrain<IUserGrain>("userGrain");
            return await userGrain.GetAllUsers();
        }
    }
}