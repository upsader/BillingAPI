# BillingAPI

BillingAPI is a .NET 9.0-based project designed to handle billing operations, including order processing, payment gateway integration, and receipt management.

## Table of Contents
- [Prerequisites](#prerequisites)
- [Project Structure](#project-structure)
- [Setup Instructions](#setup-instructions)
  - [1. Clone the Repository](#1-clone-the-repository)
  - [2. Open the Solution](#2-open-the-solution)
  - [3. Restore Dependencies](#3-restore-dependencies)
  - [4. Configure the Database](#4-configure-the-database)
  - [5. Run the Application](#5-run-the-application)
  - [6. Run Tests](#6-run-tests)
  - [7. Generate Test Coverage Report (Optional)](#7-generate-test-coverage-report-optional)

## Prerequisites

Before setting up the project, ensure you have the following installed:

- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (if using a database)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or any IDE that supports .NET development
- Git (optional, for version control)

## Project Structure

The solution consists of the following projects:

| Project | Description |
|---------|-------------|
| **BillingAPI.API** | The main API project |
| **BillingAPI.Core** | Contains core models and shared logic |
| **BillingAPI.Services** | Business logic and service implementations |
| **BillingAPI.Infrastructure** | Handles database and external integrations |
| **BillingAPI.Tests** | Unit tests for the application |

## Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/upsader/BillingAPI.git
cd BillingAPI
```

### 2. Open the Solution
Open the BillingAPI.sln file in Visual Studio or your preferred IDE.

### 3. Restore Dependencies
Restore the NuGet packages for all projects:

```bash
dotnet restore
```

### 4. Configure the Database
If the project uses a database, configure the connection string in appsettings.json or appsettings.Development.json under the BillingAPI.API project:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=BillingAPI_Dev;Trusted_Connection=True;"
}
```

Run the Entity Framework migrations to set up the database schema:

```bash
dotnet ef database update --project BillingAPI.Infrastructure --startup-project BillingAPI.API
```

### 5. Run the Application
Start the API project:

```bash
dotnet run --project BillingAPI.API
```

The API will be available at: 
* https://localhost:5001
* http://localhost:5000

Swagger documentation can be accessed at: 
* https://localhost:5001/swagger

### 6. Run Tests
Run the unit tests to ensure everything is working correctly:

```bash
dotnet test
```

### 7. Generate Test Coverage Report (Optional)
To generate a test coverage report, use the following commands:

```bash
dotnet test --collect:"XPlat Code Coverage"
dotnet reportgenerator -reports:./TestResults/**/coverage.cobertura.xml -targetdir:./TestResults/CoverageReport -reporttypes:Html
```

The coverage report will be available in the BillingAPI.Tests/TestResults/CoverageReport directory.