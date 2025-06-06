using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Services.User
{
    public interface IContactService
    {
        Task<ContactDto> GetContactByIdPersonAsync(string id);
    }
}