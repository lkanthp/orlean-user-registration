using GrainInterfaces;
using Orleans;
using Orleans.Runtime;

namespace Grains
{
    public class UserGrain : Grain, IUserGrain
    {
        private readonly IPersistentState<UserState> _userState;

        public UserGrain([PersistentState("userState", "MySqlStore")] IPersistentState<UserState> userState)
        {
            Console.WriteLine("UserGrain constructor called");
            _userState = userState;
        }

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

            _userState.State.Users[user.Id] = user;

            try
            {
                await _userState.WriteStateAsync();
            }
            catch (Exception ex)
            {
                // Log the exception, rethrow it, or handle it in some other way
                Console.WriteLine($"An error occurred while writing state: {ex}");
            }

            Console.WriteLine("userId: " + user.Id);
            
            return user.Id;
        }

        public async Task<User> GetUserById(string id)
        {
            _userState.State.Users.TryGetValue(id, out var user);
            return user;
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            var user = _userState.State.Users.Values.FirstOrDefault(u => u.UserName == userName);
            return user;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return _userState.State.Users.Values.ToList();
        }
    }
}