using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Services.User
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> GetEmployeeByIdAsync(string id);
        Task<EmployeeListDto> GetEmployeesAsync(int pageNumber = 1, int pageSize = 10);
    }
}