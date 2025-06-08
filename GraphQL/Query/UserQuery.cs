using GraphQL;
using GraphQL.Types;
using msusersgraphql.GraphQL.Types;
using msusersgraphql.Services.User;
using Microsoft.Extensions.DependencyInjection;
using msusersgraphql.Services.Employee;
using msusersgraphql.Services.Person;
using msusersgraphql.Services.Contact;

namespace msusersgraphql.Models.GraphQL
{
    public class UserQuery : ObjectGraphType
    {
        public UserQuery()
        {
            // User queries
            Field<UserType>("user")
                .Argument<StringGraphType>("id")
                .ResolveAsync(async context => 
                {
                    var userService = context.RequestServices!.GetRequiredService<IUserService>();
                    var id = context.GetArgument<string>("id");
                    return await userService.GetUserByIdAsync(id);
                })
                .Description("Get a user by ID");

            Field<UserListType>("users")
                .Argument<IntGraphType>("pageNumber")
                .Argument<IntGraphType>("pageSize")
                .ResolveAsync(async context => 
                {
                    var userService = context.RequestServices!.GetRequiredService<IUserService>();
                    var pageNumber = context.GetArgument<int?>("pageNumber") ?? 1;
                    var pageSize = context.GetArgument<int?>("pageSize") ?? 10;
                    return await userService.GetUsersAsync(pageNumber, pageSize);
                })
                .Description("Get a paginated list of users");

            // Employee queries
            Field<EmployeeType>("employee")
                .Argument<StringGraphType>("id")
                .ResolveAsync(async context => 
                {
                    var employeeService = context.RequestServices!.GetRequiredService<IEmployeeService>();
                    var id = context.GetArgument<string>("id");
                    return await employeeService.GetEmployeeByIdAsync(id);
                })
                .Description("Get an employee by ID");

            // Person queries
            Field<PersonType>("person")
                .Argument<StringGraphType>("id")
                .ResolveAsync(async context => 
                {
                    var personService = context.RequestServices!.GetRequiredService<IPersonService>();
                    var id = context.GetArgument<string>("id");
                    return await personService.GetPersonByIdAsync(id);
                })
                .Description("Get a person by ID");

                  // Person queries
            Field<ContactType>("contact")
                .Argument<StringGraphType>("id")
                .ResolveAsync(async context => 
                {
                    var contactService = context.RequestServices!.GetRequiredService<IContactService>();
                    var id = context.GetArgument<string>("id");
                    return await contactService.GetContactByIdPersonAsync(id);
                })
                .Description("Get a contact by ID");

        }
    }
}
