using msusersgraphql.Models.Dtos;
using msusersgraphql.Repositories.Employee;

namespace msusersgraphql.Services.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Employee ID cannot be empty");
            }

            return await _employeeRepository.GetEmployeeByIdAsync(id);
        }
    }
}