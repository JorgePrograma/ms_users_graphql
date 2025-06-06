using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Services.User
{
    public interface IPersonService
    {
        Task<PersonDto> GetPersonByIdAsync(string id);
        Task<PersonListDto> GetPersonsAsync(int pageNumber = 1, int pageSize = 10);
    }
}