# TipidUlam - Budget Meal Planner for Filipino Families

TipidUlam is a full-stack application designed to help Filipino families plan meals within their budget. It combines recipe management, ingredient pricing, pantry inventory tracking, and budget-aware recipe suggestions to help users stretch their grocery dollars and reduce food waste.

## What This System Does

- Stores recipes and recipe ingredients with detailed quantity information.
- Tracks ingredient price history and uses it to calculate meal costs.
- Maintains a user pantry so users can see what they already have and avoid duplicate purchases.
- Provides budget-based recipe suggestions to help users choose affordable meals.
- Offers authentication for secure user-specific pantry and recipe management.

## Key Features

- User sign-up, login, and token-based authentication.
- Recipe search and filtering based on pantry contents.
- Recipe cost calculation based on current ingredient prices.
- Pantry item management with quantities and stock status.
- Support for updating ingredient prices and tracking historical cost changes.

## Tech Stack

- Backend: ASP.NET Core 10
- Frontend: React with JSX
- Database: Entity Framework Core with relational persistence
- API: RESTful controllers for authentication, recipes, ingredients, and pantry operations
- Project Structure:
  - `TipidUlam.Backend/` for the server-side API and business logic
  - `TipidUlam.Frontend/` for the client-side React application

## Architecture

The backend implements domain models for users, recipes, ingredients, pantry items, and price history. Repository and service layers isolate database access and business rules, while controllers expose REST endpoints for frontend consumption.

The frontend is a single-page React app that interacts with the backend API, manages authentication state, and provides pages for browsing recipes, managing the pantry, and viewing user profile details.

## Getting Started

1. Open the solution in Visual Studio or your preferred IDE.
2. Restore NuGet packages and install frontend dependencies.
3. Run the backend API and the React frontend concurrently.
4. Use the application to sign in, add pantry items, and discover budget-friendly recipes.

## Folder Layout

- `TipidUlam.Backend/`
  - `Controllers/` - API controllers
  - `Services/` - Business logic services
  - `Repositories/` - Data access layer
  - `Data/` - Entity Framework context and initialization
  - `DTOs/` - Data transfer objects for API requests/responses
  - `Models/` - Domain entities
  - `Migrations/` - Database schema migrations
- `TipidUlam.Frontend/`
  - `src/` - React application source files
  - `src/components/` - UI components
  - `src/context/` - React context providers
  - `src/services/` - API client utilities

## Why TipidUlam

The app is tailored for Filipino households who want to plan meals thoughtfully while keeping costs low. By combining pantry tracking with recipe cost analysis, TipidUlam helps users make smarter grocery decisions and reduce waste.

