using GraphQL.Types;
using msusersgraphql.Models.Dtos;
using msusersgraphql.Services.User;

namespace msusersgraphql.GraphQL.Types
{
    public sealed class ContactType : ObjectGraphType<ContactDto>
    {
        public ContactType()
        {
            Name = "Contact";
            Field(x => x.IdPerson).Description("The person ID of the contact");
            Field(x => x.Email).Description("The ID of the contact");
            Field(x => x.Phone).Description("The person ID of the contact");
            Field(x => x.Address).Description("The user ID of the contact");
            Field(x => x.CellPhone).Description("The business email of the contact");
            Field(x => x.Country).Description("The business phone of the contact");
            Field(x => x.Department, nullable: true).Description("The position ID of the contact");
            Field(x => x.Locality, nullable: true).Description("The branch ID of the contact");
            Field(x => x.Neighborhood, nullable: true).Description("The branch ID of the contact");
            Field(x => x.City, nullable: true).Description("The branch ID of the contact");
        }
    }
}