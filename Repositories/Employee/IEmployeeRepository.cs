using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Repositories.Employee
{
    public interface IEmployeeRepository
    {
        Task<EmployeeDto> GetEmployeeByIdAsync(string id);
    }
}