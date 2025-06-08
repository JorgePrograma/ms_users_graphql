using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Services.Person
{
    public interface IPersonService
    {
        Task<PersonDto> GetPersonByIdAsync(string id);
    }
}