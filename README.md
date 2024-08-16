# PersonService

Webservice for person specific operations. 
- Framework: dotnet 8.0.0 
- SDK: 8.0.303 
- Runtime: 8.0.7

# Project structure

```
PersonService/
├── Controllers/                # API controllers
│   ├── PersonController.cs     # Controller for Person related operations   
├── Data/                       # Application database related files
│   ├── Interceptors/           # For entity modification before save
├── Dtos/                       # Data transfer objects
│   ├── Person/                 # Data transfer objects for Person operations
├── Exceptions/                 # Custom exceptions used in application
├── Mappers/                    # Object mapper for mapping dtos to model and vice versa
├── Middlewares/                # For request and response logging
├── Models/                     # Database described entities
├── Services/                   # Service layer, that contains service interfaces
│   ├── Impl/                   # Implementation of service interfaces
├── Utilites/                   # Shared helper utilities
├── Validators/                 # Validators for http requests
│   ├── Person/                 # Person related validators
├── appsettings.json            # Settings file for running application
├── Program.cs                  # Application startup file
```


## Build

Prerequisities:
 - dotnet 8.0.0 or newer must be installed. https://dotnet.microsoft.com/en-us/download

Navigate to prpject root folder.
Run command:
`cmd> dotnet build`

## Run
Navigate to project PersonService/ folder.
Run command:
`cmd> dotnet run`

Local OpenApi (Swagger) url: http://localhost:5053/swagger/index.html

## Configuration
No configuration 
Uses Entity Framework Core In-Memory database. 

## Test
Navigate to prpject root folder.
Run command:
`cmd> dotnet test`

## Test Coverage
Prerequisities
- dotnet-reportgenerator-globaltool must be installed globally

Run command:

`cmd> dotnet tool install -g dotnet-reportgenerator-globaltool`
 
Run test with coverage:

`cmd> dotnet test /p:CollectCoverage=true --collect:"XPlat Code Coverage"`

Run report generator:

`cmd> reportgenerator -reports:".\PersonService.Tests\TestResults\{guid}\coverage.cobertura.xml" -targetdir:"coverageresults" -reporttypes:Html`

Report will be create inside:

`.\PersonService.Tests\TestResults\Coverage\index.html`

OR

Naviagte to root folder.
Run all at once:

`cmd> test-with-coverage.bat`

This will run all previous commands at once:
1. Install dotnet-reportgenerator-globaltool if not installed
2. Run tests with coverage
3. Generates report

Report will be create inside:

`.\PersonService.Tests\TestResults\Coverage\index.html`

