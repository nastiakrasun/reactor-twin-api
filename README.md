# Reactor Twin API

A minimal ASP.NET Web API for managing reactor twin records, authentication, and user administration.

## Prerequisites

- .NET 10 SDK (matching the project's `TargetFramework` - `net10.0`). Verify with:

```bash
dotnet --version
```

- PostgreSQL (local or remote)
- `dotnet-ef` tool for applying Entity Framework Core migrations (optional but recommended)

On macOS you can install required tools via Homebrew or download installers from Microsoft/Postgres websites.

## Deployment / Setup

1. Clone the repository and change directory:

```bash
git clone <repo-url>
cd reactor-twin-api
```

2. Install the .NET 10 SDK if you don't have it. Download from https://dotnet.microsoft.com or use a package manager.

3. Install PostgreSQL (example with Homebrew):

```bash
# install
brew install postgresql
# start postgres service
brew services start postgresql
# create a database (adjust username/password as needed)
createdb `reactor_twin_db`
```

4. Configure the application connection string and secrets.

This project uses `UserSecrets` (a `UserSecretsId` is present in the project file). You should set at least the connection string and JWT settings as secrets rather than storing them in source-controlled files.

Example connection string (Postgres):

```
Host=localhost;Port=5432;Database=reactor_twin_db;Username=postgres;Password=yourpassword
```

Set required user-secrets from the project directory (the project already contains a `UserSecretsId`):

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=reactor_twin_db;Username=postgres;Password=yourpassword"
dotnet user-secrets set "Jwt:Key" "your-very-strong-secret-key"
dotnet user-secrets set "Jwt:Issuer" "ReactorTwinAPI"
dotnet user-secrets set "Jwt:Audience" "ReactorTwinAPI"
```

Note: `appsettings.Development.json` includes placeholders — using user-secrets will override or complement those values in development.

5. Install `dotnet-ef` (if you plan to apply migrations locally):

```bash
dotnet tool install --global dotnet-ef --version 10.0.0
```

6. Apply EF Core migrations to create/update the database schema:

```bash
dotnet ef database update
```

7. Build and run the application:

```bash
dotnet build
dotnet run
```

The app will start and (by default) expose Swagger in development at `https://localhost:<port>/swagger`.

## Quick Testing

- Use the `/api/auth/register` and `/api/auth/login` endpoints to create users and obtain JWT tokens.
- Use the JWT token in the `Authorization: Bearer <token>` header for protected endpoints.

## Functionality (brief)

Reactor Twin API provides user authentication (username/password registration and JWT-based login), user administration for superusers (view users, update permissions), and CRUD operations for reactor twin entities. The API uses Entity Framework Core with a PostgreSQL provider for persistence, AutoMapper for DTO mapping, and standard ASP.NET Core middleware for routing, authentication, and exception handling.

The project includes a global exception middleware that returns consistent JSON error responses for unhandled exceptions and centralizes logging. Controllers handle expected error cases (validation, unauthorized access, conflicts), while the middleware ensures a uniform HTTP 500 JSON payload for unexpected failures.

## Author

Project owner: `Anastasiia Buriakivska`
