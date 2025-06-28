# Altura.BidManagement

## Overview
- Altura.BidManagement is a modular .NET 8 Web API for managing bids, built with clean architecture principles. The main entry point is the Altura.BidManagement.WebApi project, which exposes endpoints for creating, retrieving, updating, deleting, and transitioning bids.
### Prerequisites
•	.NET 8 SDK
•	(Optional) Visual Studio 2022 or later

#### Solution Structure (Clean Architecture)
 - Altura.BidManagement.WebApi: Main Web API project (entry point)
 - Altura.BidManagement.Application: Application logic, MediatR commands/queries
 - Altura.BidManagement.Infrastructure: data access, EF Core and SQLite persistence
 - Altura.BidManagement.Domain: Domain models and enums
 - Altura.BidManagement.WebApi.UnitTests: Unit tests

### Setup Instructions
1. Clone the repository:
   ```bash
   git clone
   cd Altura.BidManagement
   
2. Restore NuGet packages:
   ```bash
    dotnet restore
    ```
   
3. Run the application:
   ```bash
   dotnet run --project Altura.BidManagement.WebApi
   ```
4. Access the API at `http://localhost:5264/api/bids`

### Accessing the API
5.	Swagger UI
Navigate to /swagger (e.g., https://localhost:7116/swagger) to explore and test the API endpoints.

### BidsController Endpoints
- POST /api/bids – Create a new bid
- GET /api/bids – Get all bids
- GET /api/bids/{bidId} – Get a bid by ID
- PUT /api/bids/{id} – Update a bid
- DELETE /api/bids/resourceId?id={id} – Delete a bid
- POST /api/bids/{id}/transition – Transition a bid to a new state


6. Run the tests:
   ```bash
   dotnet test Altura.BidManagement.WebApi.UnitTests
   ```