using Microsoft.Extensions.DependencyInjection;
using msusersgraphql.Repositories;
using msusersgraphql.Services;
using myapp.Repositories;
using myapp.Services;

namespace msusersgraphql.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IPokemonRepository, PokemonRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        
        // Services
        services.AddScoped<IPokemonService,PokemonService>();
        services.AddScoped<IUserService,UserService>();
        services.AddScoped<IEmployeeService,EmployeeService>();
        
        // HttpClient for Users API
        services.AddHttpClient("PokeAPI", client =>
        {
            client.BaseAddress = new Uri("https://ib3m6t7bp7sjmglwvvpg3xrmzu.apigateway.sa-bogota-1.oci.customer-oci.com/api/v1/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "msusersgraphql");
        });/*
        services.AddHttpClient("PokeAPI", client =>
        {
            client.BaseAddress = new Uri("https://api.example.com/users/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "msusersgraphql");
        });
        */
    }
}