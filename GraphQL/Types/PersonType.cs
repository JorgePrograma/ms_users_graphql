using GraphQL.Types;
using msusersgraphql.Models.Dtos;

namespace msusersgraphql.GraphQL.Types
{
    public sealed class PersonType : ObjectGraphType<PersonDto>
    {
        public PersonType()
        {
            Name = "Person";
            Field(x => x.Id).Description("The ID of the person");
            Field(x => x.DocumentType).Description("The document type of the person");
            Field(x => x.DocumentNumber).Description("The document number of the person");
            Field(x => x.FirstName).Description("The first name of the person");
            Field(x => x.MiddleName, nullable: true).Description("The middle name of the person");
            Field(x => x.LastName).Description("The last name of the person");
            Field(x => x.SecondLastName, nullable: true).Description("The second last name of the person");
        }
    }

    public sealed class PersonListType : ObjectGraphType<PersonListDto>
    {
        public PersonListType()
        {
            Name = "PersonList";
            Field(x => x.TotalCount).Description("Total count of persons");
            Field(x => x.PageNumber).Description("Current page number");
            Field(x => x.PageSize).Description("Page size");
            Field<ListGraphType<PersonType>>("data")
                .Description("List of persons")
                .Resolve(context => context.Source.Data);

        }
    }
}