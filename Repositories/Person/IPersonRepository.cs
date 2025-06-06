using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Repositories.User
{
    public interface IPersonRepository
    {
        Task<PersonDto> GetPersonByIdAsync(string id);
        Task<PersonListDto> GetPersonsAsync(int pageNumber = 1, int pageSize = 10);
    }
}