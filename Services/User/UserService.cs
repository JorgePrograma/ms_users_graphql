using msusersgraphql.Models.Dtos;
using msusersgraphql.Repositories.User;

namespace msusersgraphql.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("User ID cannot be empty");
            }

            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<UserListDto> GetUsersAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Page number and page size must be greater than 0");
            }

            return await _userRepository.GetUsersAsync(pageNumber, pageSize);
        }
    }
}