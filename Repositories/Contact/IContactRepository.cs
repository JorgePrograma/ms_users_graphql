using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Repositories.User
{
    public interface IContactRepository
    {
        Task<ContactDto> GetContactByIdPersonAsync(string id);
    }
}