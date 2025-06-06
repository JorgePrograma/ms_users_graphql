using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Repositories.User
{
    public interface IUserRepository
    {
        Task<UserDto> GetUserByIdAsync(string id);
        Task<UserListDto> GetUsersAsync(int pageNumber = 1, int pageSize = 10);
    }
}