using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Repositories.User
{
    public interface IEmployeeRepository
    {
        Task<EmployeeDto> GetEmployeeByIdAsync(string id);
        Task<EmployeeListDto> GetEmployeesAsync(int pageNumber = 1, int pageSize = 10);
    }
}