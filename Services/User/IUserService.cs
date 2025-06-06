using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Services.User
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(string id);
        Task<UserListDto> GetUsersAsync(int pageNumber = 1, int pageSize = 10);
    }
}