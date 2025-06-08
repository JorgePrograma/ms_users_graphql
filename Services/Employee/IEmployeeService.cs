using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Services.Employee
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> GetEmployeeByIdAsync(string id);
    }
}