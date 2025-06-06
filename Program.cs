using GraphQL;
using msusersgraphql.Extensions;
using msusersgraphql.Models.GraphQL;
using myapp.Models.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// Configuración de GraphQL
builder.Services.AddGraphQL(builder => {
    builder.AddSystemTextJson();
    builder.AddSchema<UserEmployeeSchema>();
    builder.AddGraphTypes();
});

// Configuración de servicios personalizados
builder.Services.ConfigureServices();

// Configuración del puerto
var port = Environment.GetEnvironmentVariable("PORT") ?? "3010";
builder.WebHost.UseUrls($"http://localhost:{port}");

var app = builder.Build();

// Solo el middleware de GraphQL
app.UseGraphQL<UserEmployeeSchema>();
app.Run();
