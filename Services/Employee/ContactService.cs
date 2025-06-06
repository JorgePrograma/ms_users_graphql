using msusersgraphql.Models.Dtos;
using msusersgraphql.Repositories.User;

namespace msusersgraphql.Services.User
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<ContactDto> GetContactByIdPersonAsync(string id)
        {
             if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Contact ID cannot be empty");
            }

            return await _contactRepository.GetContactByIdPersonAsync(id);
        }

    }
}