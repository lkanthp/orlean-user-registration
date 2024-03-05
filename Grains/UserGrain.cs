using GrainInterfaces;
using Orleans;
using Orleans.Runtime;

namespace Grains
{

    public class UserState
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
    }


    public class UserGrain : Grain, IUserGrain
    {
        private Dictionary<string, User> users = new Dictionary<string, User>();
        // private readonly IPersistentState<UserState> _userState;

        // public UserGrain([PersistentState("userState", "MySqlStore")] IPersistentState<UserState> userState)
        // {
        //     Console.WriteLine("UserGrain constructor called");
        //     _userState = userState;
        // }

        public async Task<string> RegisterUser(string userName, string email, string postalCode, string phoneNumber)
        {
             Console.WriteLine("RegisterUser called while writing state:");
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = userName,
                Email = email,
                PostalCode = postalCode,
                PhoneNumber = phoneNumber
            };

            users[user.Id] = user;

            Console.WriteLine("userId: " + user.Id);

            // try
            // {
            //     await _userState.WriteStateAsync();
            // }
            // catch (Exception ex)
            // {
            //     // Log the exception, rethrow it, or handle it in some other way
            //     Console.WriteLine($"An error occurred while writing state: {ex}");
            // }
            return user.Id;
        }

        public async Task<User> GetUserById(string id)
        {
            Console.WriteLine("GetUserById: "+id+" called while reading state");
            users.TryGetValue(id, out var user);
            return user;
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            var user = users.Values.FirstOrDefault(u => u.UserName == userName);
            return user;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return users.Values.ToList();
        }
    }
}