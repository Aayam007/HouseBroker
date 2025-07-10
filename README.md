# HouseBroker

A clean-architecture real estate broker platform built with ASP.NET Core 8.0.

## Features

- User authentication (JWT, role-based)
- Property listing, search, and management
- Commission calculation logic
- Role-based access (Broker, Seeker)
- Swagger API documentation
- Automated database migrations
- Clean Architecture (Domain, Application, Infrastructure, API)

## Project Structure

```
src/
  HouseBroker.Domain/         # Core business entities and interfaces
  HouseBroker.Application/    # DTOs, use cases, business logic
  HouseBroker.Infrastructure/ # Data access, services, EF Core context
  HouseBroker.API/            # Web API, controllers, DI, startup
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (or LocalDB for development)

## Setup Instructions

### 1. Clone the Repository

```bash
git clone <your-repo-url>
cd Proshore
```

### 2. Database Setup

- The app uses a connection string in `src/HouseBroker.API/appsettings.json`:
  ```json
  "DefaultConnection": "Server=.;Database=HouseBrokerDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;Encrypt=false"
  ```
- **Automatic Migration:**  
  On first run, the app will automatically create and migrate the database. No manual migration is needed.
- **Custom Database:**  
  To use a different SQL Server instance, update the `DefaultConnection` string accordingly.

### 3. Build and Run the Application

```bash
dotnet build
dotnet run --project src/HouseBroker.API
```

- The API will be available at `https://localhost:<port>/`
- **Swagger UI:** [https://localhost:<port>/swagger](https://localhost:<port>/swagger)
- The root URL will redirect to `/swagger` for easy API exploration.

### 4. Running Tests

```bash
dotnet test
```

- Unit tests cover commission logic, property listing, and controller endpoints.
- See `src/HouseBroker.Tests/README.md` for details.

## Clean Architecture Overview

- **Domain**: Business rules, entities, interfaces
- **Application**: Use cases, DTOs, orchestrators
- **Infrastructure**: Data access, external services
- **API**: Controllers, DI, startup

## Comments & Documentation

- Each class and method is documented with XML comments.
- See code for detailed explanations of each layer’s responsibilities.

## Troubleshooting

- **Database Connection Issues:**
  - Ensure SQL Server or LocalDB is running.
  - Update the connection string if using a remote or named instance.
- **JWT Key Issues:**
  - Ensure the JWT key in `Program.cs` and `AuthController.cs` is at least 32 characters long.
- **SSL Certificate Issues:**
  - The connection string includes `TrustServerCertificate=true;Encrypt=false` for local development.

## Contact

For questions or support, please contact the project maintainer. 