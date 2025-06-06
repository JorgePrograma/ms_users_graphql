# .NET API Service Starter

This is a minimal .NET API service starter based on [Google Cloud Run Quickstart](https://cloud.google.com/run/docs/quickstarts/build-and-deploy/deploy-dotnet-service).

## Getting Started

Server should run automatically when starting a workspace. To run manually, run:
```sh
dotnet watch --urls=http://localhost:3000
```
# listar la lista de usuarios
{
  "query": "{ users { data { id userName creationDate state roles { id name } } totalCount pageNumber pageSize } }"
}

