using myapp.Models.Dtos;
using System.Threading.Tasks;

namespace myapp.Services
{
    public interface IUserService
    {
        Task<EmployeeDto> GetEmployeeById(int pageNumber, int pageSize);
        Task<UserListDto> GetUsers(int pageNumber=20, int pageSize=20);
    }
}
