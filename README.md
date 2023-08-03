# Squabble Server

The Squabble Server (API) is the backend interface that acts as the middle person between the 
Squabble Web App and the database.

## Useful links

| Description  | URL |
| ------------ | --- |
| Squabble-Server (this project)  | https://github.com/RMIT-COSC2650-SP3-2021-Team-3/Squabble-Server  |
| Squabble-Client                 | https://github.com/RMIT-COSC2650-SP3-2021-Team-3/Squabble-Client  |
| Azure Portal*                   | https://portal.azure.com |

\* Azure hosts the database, the production deployments and the file/blob storage.

## Development

### Requirements

- C#
- .NET 5

### Running the code

1. Install dependencies with `nuget install`
2. Start the development server with `dotnet run`

The API server is now running. If you also want to run the Squabble Web App follow the instructions
in the `Squabble-Client` README.

### Coding conventions

TODO: Talk about source code structure etc.

### Testing

TODO: Ask Vin about running the unit tests.

## Deploying the code

The code can be deployed to any number of hosts, options are AWS, DigitalOcean, Azure and more!

We chose Azure App Service to deploy our code and the instructions to follow our set up can be
found in the Azure documentation portal
[here](https://docs.microsoft.com/en-us/azure/app-service/quickstart-dotnetcore).

### Azure services

We rely on a number of Azure services to run the Squabble application. The guides to deploy each
are listed here.

| Description  | URL |
| ------------ | --- |
| SQL Server and Database | https://docs.microsoft.com/en-us/azure/azure-sql/database/single-database-create-quickstart?tabs=azure-portal |
| Blob Storage            | https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal |
| Communication Services  | https://docs.microsoft.com/en-us/azure/communication-services/quickstarts/create-communication-resource?tabs=windows&pivots=platform-azp |

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

To generate the changelog run `git log --format='- %s (Commit: %h) [Author: %aN]' > CHANGELOG.md`.

## Known issues/bugs

- Bug #1 TBA
- Bug #2 TBA

## License

The source code is licensed under Creative Commons Attribution-NonCommercial-NoDerivatives 4.0
International.

This license allows reusers to copy and distribute the material in any medium or format in
unadapted form only, for noncommercial purposes only, and only so long as attribution is given to
the creator.

See [LICENSE.md](LICENSE.md) for the full text.
