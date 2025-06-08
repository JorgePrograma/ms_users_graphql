using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Repositories.Person
{
    public interface IPersonRepository
    {
        Task<PersonDto> GetPersonByIdAsync(string id);
    }
}