# TipidUlam Development Quick Reference

## 🚀 Quick Commands

### Backend (ASP.NET Core)

```bash
# Install packages
dotnet restore

# Build project
dotnet build

# Run in development
dotnet run

# Watch mode (auto-reload on changes)
dotnet watch run

# Create migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# View migrations
dotnet ef migrations list

# Revert last migration
dotnet ef migrations remove

# Run tests
dotnet test

# Publish for production
dotnet publish -c Release
```

### Frontend (React)

```bash
# Install dependencies
npm install

# Start development server
npm start

# Build for production
npm run build

# Run tests
npm test

# Test coverage
npm test -- --coverage

# Eject configuration (irreversible!)
npm run eject

# Format code
npm run format (if configured)

# Lint code
npm run lint (if configured)
```

### Database (PostgreSQL)

```bash
# Connect to PostgreSQL
psql -U postgres

# Create database
CREATE DATABASE tipidulam_db;

# Run schema
\c tipidulam_db
\i database-schema.sql

# Run sample data
\i sample-data.sql

# List tables
\dt

# Describe table
\d table_name

# Drop database
DROP DATABASE tipidulam_db;
```

---

## 📝 File Templates

### Adding a New Entity (Backend)

**1. Create Model** (`Models/NewEntity.cs`)
```csharp
using System;
namespace TipidUlam.Backend.Models
{
    public class NewEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
```

**2. Add DbSet to DbContext**
```csharp
public DbSet<NewEntity> NewEntities { get; set; }
```

**3. Create Repository Interface** (`Repositories/INewEntityRepository.cs`)
```csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using TipidUlam.Backend.Models;

public interface INewEntityRepository
{
    Task<NewEntity> GetByIdAsync(int id);
    Task<IEnumerable<NewEntity>> GetAllAsync();
    Task<NewEntity> CreateAsync(NewEntity entity);
    Task<NewEntity> UpdateAsync(NewEntity entity);
    Task<bool> DeleteAsync(int id);
    Task SaveChangesAsync();
}
```

**4. Create Repository Implementation**
```csharp
public class NewEntityRepository : INewEntityRepository
{
    private readonly TipidUlamDbContext _context;
    
    public NewEntityRepository(TipidUlamDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<NewEntity> GetByIdAsync(int id)
    {
        return await _context.NewEntities.FindAsync(id);
    }
    
    // ... implement other methods
}
```

**5. Register in Startup.cs**
```csharp
services.AddScoped<INewEntityRepository, NewEntityRepository>();
```

---

## 🔑 Environment Variables Checklist

### Backend

- [ ] `ConnectionStrings__DefaultConnection` - PostgreSQL connection string
- [ ] `JwtSettings__SecretKey` - Min 32 chars for HS256
- [ ] `JwtSettings__Issuer` - Token issuer
- [ ] `JwtSettings__Audience` - Token audience
- [ ] `JwtSettings__ExpiryMinutes` - Token expiry
- [ ] `ASPNETCORE_ENVIRONMENT` - Development/Production
- [ ] `ASPNETCORE_URLS` - API listening URLs

### Frontend

- [ ] `REACT_APP_API_URL` - Backend API base URL
- [ ] `PORT` - Development server port (default: 3000)

---

## 🐛 Debugging Tips

### Backend

**Check logs in appsettings.json**:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Information"
    }
  }
}
```

**Use breakpoints**:
- VS Code: Ctrl+Shift+D
- Visual Studio: F5

**View SQL queries**:
```csharp
// In DbContext
logger.LogDebug("SQL Query: {query}");
```

### Frontend

**Browser DevTools**:
- F12 or Ctrl+Shift+I
- Network tab to inspect API calls
- Console tab for errors
- React DevTools extension

**Debug state**:
```javascript
// In component
const context = useTipidUlam();
console.log('Current state:', context);
```

---

## 📊 Testing Workflows

### Manual API Testing (Postman/Thunder Client)

**1. Search Recipes**
```
GET http://localhost:5000/api/recipes/search?maxBudget=500&familySize=4
```

**2. Get All Ingredients**
```
GET http://localhost:5000/api/ingredients
```

**3. Create Ingredient (Admin)**
```
POST http://localhost:5000/api/ingredients
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "name": "Chicken",
  "unitOfMeasure": "kg",
  "pricePerUnit": 150,
  "category": "protein"
}
```

### Browser Testing

1. Open `http://localhost:3000`
2. Enter budget ₱500
3. Select family size 4
4. Click "Find Recipes"
5. Verify results display

---

## 🎨 CSS Class Naming Convention

**BEM (Block Element Modifier)**:
```css
/* Block */
.search-card { }

/* Element */
.search-card__title { }

/* Modifier */
.search-card--loading { }
.search-card--error { }
```

**Component pattern**:
```css
.recipe-card { }
  .recipe-card__header { }
  .recipe-card__body { }
  .recipe-card__footer { }
  .recipe-card--featured { }
```

---

## 🚢 Deployment Checklist

### Before Going to Production

- [ ] Environment variables set securely
- [ ] Database backups configured
- [ ] HTTPS enabled
- [ ] CORS properly configured
- [ ] Rate limiting implemented
- [ ] Error logging enabled
- [ ] Performance profiling done
- [ ] Security headers set
- [ ] Database migrations tested
- [ ] Load tested (minimum 1000 users)

### Docker Build

```bash
# Backend
docker build -t tipidulam-backend .
docker run -p 5000:80 tipidulam-backend

# Frontend
docker build -t tipidulam-frontend .
docker run -p 3000:3000 tipidulam-frontend
```

---

## 📚 Useful Resources

### C# / ASP.NET Core
- [Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [JWT Authentication](https://jwt.io/)

### React
- [React Docs](https://react.dev)
- [Context API](https://react.dev/reference/react/useContext)
- [Hooks API](https://react.dev/reference/react/hooks)

### PostgreSQL
- [PostgreSQL Docs](https://www.postgresql.org/docs/)
- [PostSQL Tutorial](https://www.postgresqltutorial.com/)

### General
- [HTTP Status Codes](https://httpwg.org/specs/rfc9110.html#status.codes)
- [REST API Best Practices](https://restfulapi.net/)
- [CSS Best Practices](https://developer.mozilla.org/en-US/docs/Web/CSS)

---

## 🤝 Git Workflow

```bash
# Create feature branch
git checkout -b feature/feature-name

# Make changes
git add .
git commit -m "feat: add feature description"

# Push to remote
git push origin feature/feature-name

# Create Pull Request on GitHub

# After approval, merge to develop
git checkout develop
git merge feature/feature-name
git push origin develop

# For production releases
git checkout main
git merge develop
git tag -a v1.0.0 -m "Release version 1.0.0"
git push origin main --tags
```

---

## 🆘 Common Issues & Solutions

### Issue: CORS Error
**Solution**: Update `Startup.cs`:
```csharp
services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader());
});
```

### Issue: 404 Not Found on API
**Solution**: Check route attributes:
```csharp
[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
```

### Issue: React state not updating
**Solution**: Check Context usage:
```javascript
const { budget, setBudget } = useTipidUlam();
// Make sure setBudget is called within component scope
```

### Issue: Database connection failed
**Solution**: Verify connection string in `appsettings.json` and PostgreSQL is running

---

## 📞 Support Contacts

- **Backend Issues**: Check backend README.md
- **Frontend Issues**: Check frontend README.md
- **Database Issues**: PostgreSQL docs
- **General**: Create GitHub issue

---

**Last Updated**: May 25, 2026
