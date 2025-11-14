# Optivem eShop Monolith (.NET)

This is the .NET 8 monolith application for the Optivem eShop, migrated from the Java Spring Boot version.

## Technology Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- PostgreSQL
- C# 12

## Architecture

The application follows a layered architecture:

- **Controllers** - HTTP endpoints for API and static file serving
- **Core/Services** - Business logic layer including OrderService
- **Core/Repositories** - Data access layer with EF Core
- **Core/DTOs** - Data transfer objects for API contracts
- **Core/Entities** - Domain entities (Order)
- **Core/Exceptions** - Custom exception types
- **Core/Services/External** - HTTP clients for external APIs (ERP, Tax)

## Business Logic

### Order Placement
- Validates SKU, quantity, and country
- Fetches product price from ERP API
- Calculates discount rate (15% after 17:00, otherwise 0%)
- Fetches tax rate from Tax API
- Calculates final price with discounts and taxes
- Generates unique order number (ORD-{GUID})

### Order Retrieval
- Returns order details by order number
- Throws 404 if order doesn't exist

### Order Cancellation
- Changes order status to CANCELLED
- Blocks cancellation on December 31st between 22:00-23:00
- Throws 404 if order doesn't exist

## Configuration

Configuration is stored in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=eshop;Username=postgres;Password=postgres"
  },
  "ExternalApis": {
    "ErpApi": {
      "BaseUrl": "http://localhost:3000"
    },
    "TaxApi": {
      "BaseUrl": "http://localhost:3001"
    }
  }
}
```

## Running the Application

### Using .NET CLI

```bash
cd monolith
dotnet restore
dotnet run
```

The application will start on `http://localhost:8080`.

### Using Docker

```bash
docker build -f monolith/Dockerfile -t eshop-monolith .
docker run -p 8080:8080 eshop-monolith
```

## API Endpoints

- `GET /` - Home page
- `GET /shop.html` - Order creation page
- `GET /order-history.html` - Order history page
- `POST /api/orders` - Create new order
- `GET /api/orders/{orderNumber}` - Get order details
- `POST /api/orders/{orderNumber}/cancel` - Cancel order
- `GET /api/echo` - Health check endpoint

## Database Schema

The `orders` table contains:
- order_number (PK)
- order_timestamp
- country
- sku
- quantity
- unit_price
- original_price
- discount_rate
- discount_amount
- subtotal_price
- tax_rate
- tax_amount
- total_price
- status (PLACED/CANCELLED)

## Error Handling

- Returns 422 for validation errors (ValidationException)
- Returns 404 for non-existent resources (NotExistValidationException)
- Returns 500 for unhandled exceptions
- All errors return JSON response with error message

## Migration from Java

This application is a faithful migration from the Java Spring Boot version, preserving:
- All business logic (discount rates, tax calculations, cancellation rules)
- API contracts (camelCase JSON, same endpoints)
- Error handling behavior
- Database schema
- Static HTML pages with JavaScript
