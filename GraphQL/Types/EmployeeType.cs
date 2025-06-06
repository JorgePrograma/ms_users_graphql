using GraphQL.Types;
using msusersgraphql.Models.Dtos;
using msusersgraphql.Services.User;

namespace msusersgraphql.GraphQL.Types
{
    public sealed class EmployeeType : ObjectGraphType<EmployeeDto>
    {
        public EmployeeType()
        {
            Name = "Employee";
            Field(x => x.Id).Description("The ID of the employee");
            Field(x => x.IdPerson).Description("The person ID of the employee");
            Field(x => x.IdUser).Description("The user ID of the employee");
            Field(x => x.BussinesEmail).Description("The business email of the employee");
            Field(x => x.BussinesPhone).Description("The business phone of the employee");
            Field(x => x.IdPosition, nullable: true).Description("The position ID of the employee");
            Field(x => x.IdBranch, nullable: true).Description("The branch ID of the employee");

            // Campo relacionado: Persona
            Field<PersonType>("person")
                .ResolveAsync(async context =>
                {
                    var personService = context.RequestServices!.GetRequiredService<IPersonService>();
                    var employee = context.Source;

                    try
                    {
                        if (employee.IdPerson != null)
                        {
                            return await personService.GetPersonByIdAsync(employee.IdPerson);
                        }
                        return null;
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Description("Person data for this employee");

            Field<ContactType>("contact")
           .ResolveAsync(async context =>
           {
               var contactService = context.RequestServices!.GetRequiredService<IContactService>();
               var contact = context.Source;

               try
               {
                   if (contact.IdPerson != null)
                   {
                       return await contactService.GetContactByIdPersonAsync(contact.IdPerson);
                   }
                   return null;
               }
               catch
               {
                   return null;
               }
           })
           .Description("Person data for this employee");
        }
    }
}