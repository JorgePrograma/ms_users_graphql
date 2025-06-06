using GraphQL;
using msusersgraphql.Extensions;
using msusersgraphql.Models.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios personalizados
builder.Services.ConfigureServices();

// Configuración de GraphQL (una sola vez)
builder.Services.AddGraphQL(builder => {
    builder.AddSystemTextJson();
    builder.AddSchema<UserSchema>();
    builder.AddGraphTypes();
});

// Configuración del puerto
var port = Environment.GetEnvironmentVariable("PORT") ?? "3010";
builder.WebHost.UseUrls($"http://localhost:{port}");

var app = builder.Build();

// Configuración de GraphQL endpoint
app.UseRouting();
app.MapGraphQL("/graphql");

app.Run();

