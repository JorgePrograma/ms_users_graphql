using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Services.Contact
{
    public interface IContactService
    {
        Task<ContactDto> GetContactByIdPersonAsync(string id);
    }
}