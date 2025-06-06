using myapp.Models.Dtos;
using System.Threading.Tasks;

namespace myapp.Repositories
{
    public interface IUserRepository
    {
        Task<UserListDto> GetUsersFromApi(int pageNumber, int pageSize);
    }
}