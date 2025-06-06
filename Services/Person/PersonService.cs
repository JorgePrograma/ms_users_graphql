using msusersgraphql.Models.Dtos;
using msusersgraphql.Repositories.User;

namespace msusersgraphql.Services.User
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonDto> GetPersonByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Person ID cannot be empty");
            }

            return await _personRepository.GetPersonByIdAsync(id);
        }

        public async Task<PersonListDto> GetPersonsAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Page number and page size must be greater than 0");
            }

            return await _personRepository.GetPersonsAsync(pageNumber, pageSize);
        }
    }
}