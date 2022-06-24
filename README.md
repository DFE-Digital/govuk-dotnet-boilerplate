# govuk-dotnet-boilerplate
This is a clone of the Jayson Taylor's Clean Architecture Solution Template with the SPA front end replaced with an ASP.NET MVC Front End.
https://github.com/jasontaylordev/CleanArchitecture

This solution template creates an ASP.NET MVC Front End (using Scott Allan's Add Feature Folders https://odetocode.com/blogs/scott/archive/2016/11/29/addfeaturefolders-and-usenodemodules-on-nuget-for-asp-net-core.aspx ) and ASP.NET Core projects following the principles of Clean Architecture. 

## Technologies

* [C#](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
* [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
* [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)
* [MediatR](https://github.com/jbogard/MediatR)
* [AutoMapper](https://automapper.org/)
* [FluentValidation](https://fluentvalidation.net/)
* [NUnit](https://nunit.org/), [xUnit](https://xunit.net/), [FluentAssertions](https://fluentassertions.com/), [Moq](https://github.com/moq) & [Respawn](https://github.com/jbogard/Respawn)

### Getting Started
* Clone the repository on your local pc.
* Open the powershell console, then cd to the root folder of the cloned repository 
* Create a local nuget package with the command: dotnet pack .\nuget.csproj
* Install the nuget package with the command: dotnet new -i .\DfeCleanArchitectureTemplate.1.0.0.nupkg

This should have installed the template locally.

* Create a folder for your new solution and cd into it (the template will use it as project name)
* Run dotnet new ca-sln to create a new project
* Navigate to src/WebUI and launch the project using dotnet run

(For reference: https://medium.com/@stoyanshopov032/create-dotnet-new-template-with-multiple-projects-5df240ed81b4)

### Database Configuration

The template is configured to use an in-memory database by default. This ensures that all users will be able to run the solution without needing to set up additional infrastructure (e.g. SQL Server). The **WebUI/appsettings.json** is set as follows:

```json
  "UseInMemoryDatabase": true,
```

If you would like to use SQL Server, you will need to update **WebUI/appsettings.json** as follows:

```json
  "UseInMemoryDatabase": false,
```
```json
  "UseSqlServerDatabase": true,
```

If you would like to use Postgresql, you will need to update **WebUI/appsettings.json** as follows:

```json
  "UseInMemoryDatabase": false,
```
```json
  "UseInMemoryDatabase": false,
```

Verify that the **DefaultConnection** connection string within **appsettings.json** points to a valid database instance. 

When you run the application the database will be automatically created (if necessary) and the latest migrations will be applied.

### Database Migrations

To use `dotnet-ef` for your migrations first ensure that "UseInMemoryDatabase" is disabled, as described within previous section.
Then, add the following flags to your command (values assume you are executing from repository root)

* `--project src/Infrastructure` (optional if in this folder)
* `--startup-project src/WebUI`
* `--output-dir Persistence/Migrations`

For example, to add a new migration from the root folder:

 `dotnet ef migrations add "SampleMigration" --project src\Infrastructure --startup-project src\WebUI --output-dir Persistence\Migrations`

## Overview

### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Application

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### WebUI

This layer is a ASP.NET Core 6 MVC Front End. This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only *Startup.cs* should reference Infrastructure.

## License

This project is licensed with the [MIT license](LICENSE).