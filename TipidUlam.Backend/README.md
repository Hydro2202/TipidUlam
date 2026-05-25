# TipidUlam - Backend (ASP.NET Core API)

## Overview
TipidUlam Backend is a C# ASP.NET Core Web API that implements the Budget Matching Engine for the TipidUlam meal planning application.

### Key Features
- **Budget Matching Engine**: Filters recipes based on budget and family size
- **Pantry Checklist**: Adjusts recipe costs based on ingredients user already owns
- **Admin Price Board**: JWT-protected CRUD operations for ingredient prices
- **PostgreSQL Database**: Relational database with EF Core ORM

---

## Technical Stack
- **Framework**: ASP.NET Core 6.0
- **Language**: C# (.NET 6+)
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core (EF Core)
- **Authentication**: JWT (JSON Web Tokens)
- **Documentation**: Swagger/OpenAPI

---

## Project Structure

```
TipidUlam.Backend/
├── Models/                      # Data models
│   ├── Recipe.cs
│   ├── Ingredient.cs
│   ├── RecipeIngredient.cs
│   ├── User.cs
│   └── IngredientPriceHistory.cs
├── Controllers/                 # API endpoints
│   ├── RecipesController.cs     # Recipe search endpoint
│   └── IngredientsController.cs # Ingredient CRUD (admin only)
├── Services/                    # Business logic
│   ├── BudgetMatchingService.cs # Core budget filtering algorithm
│   └── IngredientService.cs
├── Repositories/               # Data access layer
│   ├── IRecipeRepository.cs
│   ├── RecipeRepository.cs
│   ├── IIngredientRepository.cs
│   └── IngredientRepository.cs
├── Data/                       # Database context
│   └── TipidUlamDbContext.cs
├── DTOs/                       # Data transfer objects
│   ├── RecipeDetailDto.cs
│   ├── RecipeIngredientDetailDto.cs
│   ├── IngredientDto.cs
│   └── CreateUpdateIngredientDto.cs
├── database-schema.sql         # PostgreSQL schema
├── Startup.cs                  # Service configuration
├── Program.cs                  # Application entry point
├── appsettings.json           # Default configuration
└── appsettings.Development.json
```

---

## Getting Started

### Prerequisites
- .NET 6 SDK or later
- PostgreSQL 12 or later
- Visual Studio Code or Visual Studio

### 1. Setup PostgreSQL Database

```bash
# Connect to PostgreSQL
psql -U postgres

# Create database
CREATE DATABASE tipidulam_db;

# Run schema
\c tipidulam_db
\i database-schema.sql
```

### 2. Configure Connection String

Edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=tipidulam_db;Username=postgres;Password=your_password"
  }
}
```

### 3. Update JWT Settings

Edit `appsettings.json`:
```json
{
  "JwtSettings": {
    "SecretKey": "your-very-long-secret-key-minimum-32-characters-for-HS256",
    "Issuer": "TipidUlam",
    "Audience": "TipidUlamUsers",
    "ExpiryMinutes": 60
  }
}
```

### 4. Restore and Run

```bash
# Restore packages
dotnet restore

# Build project
dotnet build

# Run application
dotnet run
```

The API will be available at `https://localhost:5001` (HTTPS) or `http://localhost:5000` (HTTP).

---

## API Endpoints

### Public Endpoints (No Authentication Required)

#### 1. Get All Recipes
```
GET /api/recipes
Response: 200 OK
[
  {
    "id": 1,
    "name": "Adobo",
    "description": "Classic Filipino chicken adobo",
    ...
  }
]
```

#### 2. Search Recipes by Budget (Core Engine)
```
GET /api/recipes/search?maxBudget=500&familySize=4&pantryIds=1,2,3

Query Parameters:
- maxBudget (decimal): Maximum budget in Philippine Pesos
- familySize (int): Number of people to feed
- pantryIds (string, optional): Comma-separated ingredient IDs owned

Response: 200 OK
{
  "count": 5,
  "budget": 500,
  "familySize": 4,
  "recipes": [
    {
      "id": 1,
      "name": "Adobo",
      "totalCostForFamily": 245.50,
      "ingredients": [
        {
          "ingredientName": "Chicken",
          "quantityPerServing": 0.5,
          "unitOfMeasure": "kg",
          "pricePerUnit": 150
        }
      ]
    }
  ]
}
```

#### 3. Get Single Recipe
```
GET /api/recipes/{id}
Response: 200 OK
```

#### 4. Get All Ingredients
```
GET /api/ingredients
Response: 200 OK
```

### Admin Endpoints (JWT Required - Admin Role)

#### 5. Create Ingredient
```
POST /api/ingredients
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Chicken",
  "unitOfMeasure": "kg",
  "pricePerUnit": 150,
  "description": "Chicken meat",
  "category": "protein"
}

Response: 201 Created
```

#### 6. Update Ingredient (Price Changes)
```
PUT /api/ingredients/{id}
Authorization: Bearer {token}
Content-Type: application/json

Response: 200 OK
```

#### 7. Delete Ingredient
```
DELETE /api/ingredients/{id}
Authorization: Bearer {token}

Response: 204 No Content
```

---

## Budget Matching Algorithm

**Location**: `Services/BudgetMatchingService.cs`

### Algorithm Overview
```csharp
FOR EACH recipe IN database:
  scaleFactor = familySize / recipe.baseServings
  totalCost = 0
  
  FOR EACH ingredient IN recipe.ingredients:
    scaledQuantity = ingredient.quantityPerServing * scaleFactor
    ingredientCost = scaledQuantity * ingredient.pricePerUnit
    
    IF ingredient.id IN pantryIds:
      ingredientCost = 0  // User already owns this
    
    totalCost += ingredientCost
  
  IF totalCost <= maxBudget:
    ADD recipe to results

RETURN results SORTED BY totalCost ASC
```

### Example Calculation
**Recipe**: Adobo (Base: 2 servings)
**Family Size**: 4
**Scale Factor**: 4 / 2 = 2x

| Ingredient | Qty/Serving | Scaled Qty | Price/Unit | Cost |
|-----------|-----------|-----------|-----------|------|
| Chicken | 0.25 kg | 0.5 kg | ₱150 | ₱75 |
| Soy Sauce | 2 tbsp | 4 tbsp | ₱5 | ₱20 |
| **Total** | - | - | - | **₱95** |

---

## Database Schema

### Users Table
- Stores admin users for JWT authentication
- Role: 'user' or 'admin'

### Recipes Table
- Base serving size used for scaling
- Metadata: cuisine type, difficulty, cooking time

### Ingredients Table
- `pricePerUnit`: Live market price in Philippine Pesos
- Updated by admins via price board

### RecipeIngredients Table
- Junction table with quantities per serving
- Links recipes to ingredients

### IngredientPriceHistory Table
- Audit trail for price changes
- Tracks old price, new price, change timestamp, admin user

---

## Error Handling

All API responses follow standard HTTP status codes:

```
200 OK - Success
201 Created - Resource created
204 No Content - Successful deletion
400 Bad Request - Invalid input
401 Unauthorized - Missing JWT token
403 Forbidden - Insufficient permissions
404 Not Found - Resource not found
500 Internal Server Error - Server error
```

Example Error Response:
```json
{
  "message": "Budget cannot be negative.",
  "statusCode": 400
}
```

---

## Development

### Running Tests
```bash
dotnet test
```

### Database Migrations (EF Core)
```bash
# Add migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update
```

### View Swagger Documentation
Navigate to: `https://localhost:5001/swagger`

---

## Deployment

### Docker
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "TipidUlam.Backend.dll"]
```

### Environment Variables for Production
```
ConnectionStrings__DefaultConnection=Host=prod-db-host;Database=tipidulam_db;Username=postgres;Password=***
JwtSettings__SecretKey=***
JwtSettings__Issuer=TipidUlam
JwtSettings__Audience=TipidUlamUsers
ASPNETCORE_ENVIRONMENT=Production
```

---

## License
MIT License - See LICENSE file

---

## Support
For issues or questions, please create an issue in the repository.
