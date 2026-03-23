# ShopService - Shop Bounded Context Microservice

An independent ASP.NET Core 10 Web API microservice extracted from the QuickApp monolith. ShopService owns the **Shop bounded context**: Customers, Products, Product Categories, Orders, and Order Details.

## Architecture

```
ShopService/
├── ShopService.API/           # ASP.NET Web API (controllers, config, ViewModels)
│   ├── Controllers/
│   │   ├── CustomerController.cs   # CRUD for customers
│   │   ├── ProductController.cs    # Read operations for products
│   │   └── OrderController.cs      # Read operations + cashier-order check
│   ├── ViewModels/                 # DTOs for API responses
│   ├── Configuration/
│   │   └── MappingProfile.cs       # AutoMapper entity-to-VM mappings
│   ├── Services/
│   │   └── UserIdAccessor.cs       # Extracts user ID from JWT claims
│   ├── Program.cs                  # App bootstrap and DI configuration
│   └── appsettings.json            # Connection string, auth authority
├── ShopService.Core/          # Domain models, services, infrastructure
│   ├── Models/
│   │   ├── Customer.cs
│   │   ├── Product.cs
│   │   ├── ProductCategory.cs
│   │   ├── Order.cs                # CashierId is a plain string (no FK)
│   │   ├── OrderDetail.cs
│   │   ├── BaseEntity.cs           # Shared base with audit fields
│   │   └── IAuditableEntity.cs
│   ├── Services/
│   │   ├── ICustomerService.cs / CustomerService.cs
│   │   ├── IProductService.cs / ProductService.cs
│   │   └── IOrdersService.cs / OrdersService.cs
│   └── Infrastructure/
│       ├── ShopDbContext.cs         # EF Core DbContext for Shop entities
│       └── DatabaseSeeder.cs        # Seeds demo data on startup
├── ShopService.sln
└── Dockerfile
```

## Key Design Decisions

- **CashierId is an opaque string.** `Order.CashierId` stores the monolith's `ApplicationUser.Id` but has no foreign key or navigation property. The Shop service treats it as an external identifier.
- **JWT tokens are validated, not issued.** The monolith's OpenIddict server issues tokens; this service validates them via JWT Bearer authentication pointing to the monolith as the authority.
- **Same table prefix (`App`) as the monolith.** Entity tables use names like `AppCustomers`, `AppProducts`, etc. This allows the service to share the monolith's database initially or use its own database.
- **Audit fields are automatic.** `ShopDbContext` intercepts `SaveChanges` to populate `CreatedBy`, `UpdatedBy`, `CreatedDate`, `UpdatedDate` from the current JWT user.

## API Endpoints

All endpoints except the cashier-order check require a valid JWT Bearer token.

### Customers (`/api/customer`)

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/api/customer` | Yes | List all customers |
| GET | `/api/customer/{id}` | Yes | Get customer by ID |
| POST | `/api/customer` | Yes | Create customer |
| PUT | `/api/customer/{id}` | Yes | Update customer |
| DELETE | `/api/customer/{id}` | Yes | Delete customer |

### Products (`/api/product`)

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/api/product` | Yes | List all products |
| GET | `/api/product/{id}` | Yes | Get product by ID |

### Orders (`/api/order`)

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/api/order` | Yes | List all orders |
| GET | `/api/order/{id}` | Yes | Get order by ID |
| GET | `/api/order/by-cashier/{cashierId}/exists` | No | Check if a cashier has any orders (used by monolith for user deletion checks) |

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server (local, Docker, or remote)

### Run Locally

```bash
cd ShopService

# Restore packages
dotnet restore

# Generate initial EF migration (required before first run)
dotnet ef migrations add Init --project ShopService.Core --startup-project ShopService.API

# Update the connection string in ShopService.API/appsettings.json if needed

# Run the service
dotnet run --project ShopService.API
```

The service starts on `http://localhost:5001` by default. Swagger UI is available at `http://localhost:5001/swagger` in Development mode.

### Run with Docker

```bash
# From the ShopService/ directory
docker build -t shopservice .
docker run -p 5001:8080 \
  -e ConnectionStrings__DefaultConnection="Server=host.docker.internal;Database=QuickApp_ShopService;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=true" \
  -e Authentication__Authority="http://host.docker.internal:7085" \
  -e ASPNETCORE_ENVIRONMENT=Development \
  shopservice
```

### Run with docker-compose (full stack)

From the repository root:

```bash
docker-compose up
```

This starts SQL Server, the monolith (port 7085), and the Shop service (port 5001) together. See the root `docker-compose.yml` for configuration details.

## Configuration

### `appsettings.json`

| Key | Description | Default |
|-----|-------------|---------|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string | `Server=(local);Database=QuickApp_ShopService;...` |
| `Authentication:Authority` | URL of the monolith's OpenIddict server for JWT validation | `https://localhost:7085` |

### Environment Variable Overrides

Use `__` (double underscore) as a section separator:

```bash
ConnectionStrings__DefaultConnection="Server=..."
Authentication__Authority="http://monolith:8080"
```

## Inter-Service Communication

The monolith calls the Shop service's `GET /api/order/by-cashier/{cashierId}/exists` endpoint when checking if a user can be deleted (`UserAccountService.TestCanDeleteUserAsync`). This is the only inter-service call. The monolith uses `IShopServiceClient` / `ShopServiceHttpClient` for this, configured via `ShopService:BaseUrl` in the monolith's `appsettings.json`.

## Database Seeding

On startup, `DatabaseSeeder` automatically:
1. Applies pending EF migrations (`MigrateAsync`)
2. Seeds demo data if the database is empty: 4 customers, 1 product category, 2 products, 2 orders with order details

## Technology Stack

- **ASP.NET Core 10** - Web API framework
- **Entity Framework Core 10** - ORM with SQL Server provider
- **AutoMapper 13** - Entity-to-ViewModel mapping
- **JWT Bearer Authentication** - Token validation via Microsoft.AspNetCore.Authentication.JwtBearer
