using msusersgraphql.Models.Dtos;
using msusersgraphql.Repositories.Person;

namespace msusersgraphql.Services.Person
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
    }
}