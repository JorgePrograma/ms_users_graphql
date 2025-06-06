using msusersgraphql.Models.Dtos;
using msusersgraphql.Repositories.User;

namespace msusersgraphql.Services.User
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

        public async Task<EmployeeListDto> GetEmployeesAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Page number and page size must be greater than 0");
            }

            return await _employeeRepository.GetEmployeesAsync(pageNumber, pageSize);
        }
    }
}