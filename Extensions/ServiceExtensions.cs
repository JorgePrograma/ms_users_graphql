using GraphQL;
using Microsoft.Extensions.DependencyInjection;
using msusersgraphql.Models.GraphQL;
using msusersgraphql.Repositories.User;
using msusersgraphql.Services.User;
using System.Net.Http.Headers;

namespace msusersgraphql.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();

            // GraphQL - Solo una configuración
            services.AddSingleton<UserSchema>();
            services.AddSingleton<UserQuery>();

            // Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IPersonService, PersonService>();

            // HttpClient for Users API con Bearer Token CORREGIDO
            services.AddHttpClient("UsersAPI", client =>
            {
                client.BaseAddress = new Uri("https://ib3m6t7bp7sjmglwvvpg3xrmzu.apigateway.sa-bogota-1.oci.customer-oci.com/api/v1/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                
                var token = "eyJ4NXQjUzI1NiI6InBHbGg3RTdpSndmRGhNbnJDU3RNV0ZpelZkNTBXcTFwZFBZdVNzcWhvM1EiLCJ4NXQiOiI0MEIxWUhCc1dJOVhBZmhaSDNrcEtNQ2lveEEiLCJraWQiOiJTSUdOSU5HX0tFWSIsImFsZyI6IlJTMjU2In0.eyJjbGllbnRfb2NpZCI6Im9jaWQxLmRvbWFpbmFwcC5vYzEuc2EtYm9nb3RhLTEuYW1hYWFhYWFoZ3BvaHNxYXlqZ2ZnNGliZ3NmbGJtcWptbmFkYjdqaW41Yms3bnEzdno1ZzV1bjYyNXRhIiwidXNlcl90eiI6IkFtZXJpY2EvQ2hpY2FnbyIsInN1YiI6InNzdWFyZXpAaW5mb2RvYy5jb20uY28iLCJ1c2VyX2xvY2FsZSI6ImVuIiwic2lkbGUiOjQ4MCwidXNlci50ZW5hbnQubmFtZSI6ImlkY3MtYWUwNGM2YzMzODVhNGQ0Mzk1MjdiZmZlYzVmZjg3MzciLCJpc3MiOiJodHRwczovL2lkZW50aXR5Lm9yYWNsZWNsb3VkLmNvbS8iLCJkb21haW5faG9tZSI6InNhLWJvZ290YS0xIiwiY2Ffb2NpZCI6Im9jaWQxLnRlbmFuY3kub2MxLi5hYWFhYWFhYXE1cW9kZDZ1NXhhdDZodDZ6cjU0M2UyN2U3aHhncjUyZTZkbTRpZWFvMnRxNnQzMnRuZGEiLCJ1c2VyX3RlbmFudG5hbWUiOiJpZGNzLWFlMDRjNmMzMzg1YTRkNDM5NTI3YmZmZWM1ZmY4NzM3IiwiY2xpZW50X2lkIjoiZDQ1YzQyMDJlYTViNGE4ZDg1ZmU2OWUzNTIwMzI2N2UiLCJkb21haW5faWQiOiJvY2lkMS5kb21haW4ub2MxLi5hYWFhYWFhYTUydGJ4ZnVrNTZiZHVnYWxqNmdxNmRxY3diYXZxbXpkZnJzaGJ5emlhbzUyb3hrcGpsNXEiLCJzdWJfdHlwZSI6InVzZXIiLCJzY29wZSI6ImdhdGV3YXkuYWNjZXNzZnVsbCBvZmZsaW5lX2FjY2VzcyIsInVzZXJfb2NpZCI6Im9jaWQxLnVzZXIub2MxLi5hYWFhYWFhYWNrdHJtbzY1M3Y3c3liY3J6b295aHFpbXk1NWJpYnJqcXlhdGVveW11am9iZGh2eDR3Z3EiLCJjbGllbnRfdGVuYW50bmFtZSI6ImlkY3MtYWUwNGM2YzMzODVhNGQ0Mzk1MjdiZmZlYzVmZjg3MzciLCJyZWdpb25fbmFtZSI6InNhLWJvZ290YS1pZGNzLTEiLCJ1c2VyX2xhbmciOiJlbiIsImV4cCI6MTc0OTIzOTE5NSwiaWF0IjoxNzQ5MjM1NTk1LCJjbGllbnRfZ3VpZCI6IjU0MGJhZTIzN2Y0MTQxYTE4MDMxZmRhMDMzY2ExZDEzIiwiY2xpZW50X25hbWUiOiJhcGlnYXRld2F5X3Jlc291cmNlX2NsaWVudCIsInRlbmFudCI6ImlkY3MtYWUwNGM2YzMzODVhNGQ0Mzk1MjdiZmZlYzVmZjg3MzciLCJqdGkiOiJjOGMzMDE1ZmEyY2E0ZGJkOTgxMzRkODEzNjE3ODM4MiIsImd0cCI6InJvIiwidXNlcl9kaXNwbGF5bmFtZSI6IlNlYmFzdGlhbiBBbmRyZXMgU3VhcmV6IEd1em1hbiIsIm9wYyI6ZmFsc2UsInN1Yl9tYXBwaW5nYXR0ciI6InVzZXJOYW1lIiwicHJpbVRlbmFudCI6ZmFsc2UsInRva190eXBlIjoiQVQiLCJhdWQiOiJhcGktZ2F0ZXdheSIsImNhX25hbWUiOiJpZHNvcG9ydGUiLCJ1c2VyX2lkIjoiYWViY2E0MWE4ZTRkNGNkNTgyNjI1OTYzZTIzM2RmNDciLCJydF9qdGkiOiI2OGM0YTEwZjkyNjY0YzFkYWRiN2RlYzlkZjc3M2QyNCIsImRvbWFpbiI6IlNHREVBX0RPTUFJTiIsInRlbmFudF9pc3MiOiJodHRwczovL2lkY3MtYWUwNGM2YzMzODVhNGQ0Mzk1MjdiZmZlYzVmZjg3MzcuaWRlbnRpdHkub3JhY2xlY2xvdWQuY29tOjQ0MyIsInJlc291cmNlX2FwcF9pZCI6ImM1M2Y0NmMyOWQyNTQyMzNhOWM4NmFhOGRhZGVkMDkwIn0.euQQuooNT1n09274u7R0HlC5FwNJDw2z8rzNdmzUD22tvTrFqyRPJEetVvDiohQRGFoOz_mKmN_GL_gBHOEF490S0NDAkoP49F9NYIN9V8tF7ekBEfYIsWioBGAK4xAc53zPq2XRkwdHEiuwuk43oP2Kj7QodLZIepm5U-poI5d8Ug0Og5AJ76U-9DSXbzaJ5ITB8I2PYb4WSkOUduAhQW-w4ErTRca2VUr60AZF7T5zUfQ6oTZVTlR_-xUhwLJDiGxI3l-IOxfwWV9BS3bLIwlpgYGWiUYb45JmuPPnePyFYZTmajkdpIevxDez-9U858tvA5RpQ2asWqII7lqtzQ";
                // MÉTODO CORRECTO: Usar AuthenticationHeaderValue
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                Console.WriteLine($"Token configurado correctamente");
            });
        }
    }
}
