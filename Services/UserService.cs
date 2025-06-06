using myapp.Models.Dtos;
using myapp.Repositories; // Assuming you have a user repository
using System.Threading.Tasks;

namespace myapp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserListDto> GetUsers(int pageNumber, int pageSize)
        {
            // Here you would call your user repository or directly the API
            // For demonstration, let's simulate fetching data
            var users = await _userRepository.GetUsersFromApi(pageNumber, pageSize); // Implement this in your repository
            return users;
        }
    }
}
