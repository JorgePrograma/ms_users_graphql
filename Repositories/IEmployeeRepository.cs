using myapp.Models.Dtos;
using System.Threading.Tasks;

namespace myapp.Repositories
{
    public interface IEmployeeRepository
    {
        Task<EmployeeDto> GetEmployeeFromApiByUserId(string userId);
    }
}
