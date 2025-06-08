using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Repositories.Contact
{
    public interface IContactRepository
    {
        Task<ContactDto> GetContactByIdPersonAsync(string id);
    }
}