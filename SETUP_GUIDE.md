# TipidUlam - Complete Setup Guide

## 📋 Table of Contents
1. [Project Overview](#project-overview)
2. [System Requirements](#system-requirements)
3. [Backend Setup](#backend-setup)
4. [Frontend Setup](#frontend-setup)
5. [Verify Installation](#verify-installation)
6. [Running the Application](#running-the-application)
7. [Troubleshooting](#troubleshooting)

---

## Project Overview

**TipidUlam** is a full-stack budget meal planning application with:
- **Backend**: C# ASP.NET Core 6 Web API
- **Frontend**: ReactJS 18 Single-Page Application
- **Database**: PostgreSQL 12+
- **Authentication**: JWT (JSON Web Tokens)

### Folder Structure
```
TipidUlam/
├── TipidUlam.Backend/          # ASP.NET Core API
├── TipidUlam.Frontend/          # React App
├── README.md                    # Main documentation
├── QUICK_REFERENCE.md           # Commands cheat sheet
└── .gitignore                   # Git ignore rules
```

---

## System Requirements

### Required Software

| Software | Version | Purpose |
|----------|---------|---------|
| .NET SDK | 6.0+ | Backend development |
| Node.js | 14+ LTS | Frontend development |
| PostgreSQL | 12+ | Database server |
| Git | Any recent | Version control |
| VS Code / Visual Studio | Any recent | IDE |

### Hardware
- **RAM**: 4GB minimum (8GB recommended)
- **Disk**: 2GB free space
- **Processor**: Dual-core minimum

### System Paths (Verify These)
```bash
# Check .NET installation
dotnet --version

# Check Node.js installation
node --version
npm --version

# Check PostgreSQL installation
psql --version
```

---

## Backend Setup

### Step 1: Install .NET 6 SDK

**Windows**:
1. Download from [dot.net](https://dotnet.microsoft.com/download)
2. Run installer
3. Follow on-screen instructions
4. Restart your terminal

**macOS**:
```bash
brew install dotnet-sdk
```

**Linux (Ubuntu)**:
```bash
wget https://dot.microsoft.com/dotnet/release-metadata/releases-index.json -O releases-index.json
sudo apt-get install dotnet-sdk-6.0
```

### Step 2: Install PostgreSQL

**Windows**:
1. Download from [postgresql.org](https://www.postgresql.org/download/windows/)
2. Run installer, note the password for `postgres` user
3. Accept default port 5432
4. Add PostgreSQL bin folder to PATH (usually `C:\Program Files\PostgreSQL\15\bin`)

**macOS**:
```bash
brew install postgresql@15
brew services start postgresql@15
```

**Linux (Ubuntu)**:
```bash
sudo apt update
sudo apt install postgresql postgresql-contrib
sudo service postgresql start
```

### Step 3: Create PostgreSQL Database

```bash
# Connect to PostgreSQL (default user: postgres)
psql -U postgres

# In psql prompt:
CREATE DATABASE tipidulam_db;

# Verify creation
\l

# Exit psql
\q
```

### Step 4: Initialize Database Schema

```bash
# Navigate to backend folder
cd c:\TipidUlam\TipidUlam.Backend

# Run schema
psql -U postgres -d tipidulam_db -f database-schema.sql

# (Optional) Load sample data
psql -U postgres -d tipidulam_db -f sample-data.sql
```

### Step 5: Configure Backend

**Edit `appsettings.json`**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=tipidulam_db;Username=postgres;Password=YOUR_PASSWORD"
  },
  "JwtSettings": {
    "SecretKey": "your-very-long-secret-key-minimum-32-characters-for-HS256-encryption",
    "Issuer": "TipidUlam",
    "Audience": "TipidUlamUsers",
    "ExpiryMinutes": 60
  }
}
```

**Important**: 
- Replace `YOUR_PASSWORD` with the PostgreSQL password you set during installation
- Keep `SecretKey` long and secure (use a password generator)
- Do NOT commit passwords to Git

### Step 6: Restore & Build Backend

```bash
cd c:\TipidUlam\TipidUlam.Backend

# Restore NuGet packages
dotnet restore

# Build project
dotnet build

# Output: Should show "Build succeeded"
```

### Step 7: Run Backend

```bash
# From TipidUlam.Backend folder
dotnet run

# Output should show:
# Application startup. Application started. 
# Press Ctrl+C to shut down.
# Listening on: http://localhost:5000 https://localhost:5001
```

**Backend is now running** ✅

---

## Frontend Setup

### Step 1: Install Node.js & npm

**Windows**:
1. Download from [nodejs.org](https://nodejs.org)
2. Choose LTS version (14+)
3. Run installer
4. Accept default settings
5. Restart terminal

**macOS**:
```bash
brew install node
```

**Linux (Ubuntu)**:
```bash
sudo apt install nodejs npm
```

### Step 2: Verify Installation

```bash
node --version   # Should be 14.0.0+
npm --version    # Should be 6.0.0+
```

### Step 3: Install Frontend Dependencies

```bash
cd c:\TipidUlam\TipidUlam.Frontend

# Install all npm packages
npm install

# This creates node_modules/ folder (may take 2-3 minutes)
```

### Step 4: Configure Frontend Environment

**Edit `.env`**:
```
REACT_APP_API_URL=http://localhost:5000/api
PORT=3000
```

### Step 5: Start Frontend Development Server

```bash
# From TipidUlam.Frontend folder
npm start

# This should:
# 1. Compile React app
# 2. Open browser to http://localhost:3000
# 3. Show "Compiled successfully!"
```

**Frontend is now running** ✅

---

## Verify Installation

### Checklist

- [ ] **Backend Running**: 
  - Terminal shows "Application started"
  - Listening on `http://localhost:5000`

- [ ] **Frontend Running**: 
  - Terminal shows "Compiled successfully!"
  - Browser opened to `http://localhost:3000`

- [ ] **Database Connected**: 
  - PostgreSQL service is running
  - Database `tipidulam_db` exists

### Test API Endpoint

**Open browser and visit**:
```
http://localhost:5000/api/recipes
```

**Expected response**: JSON array of recipes (or empty array if no sample data)

### Test Frontend

1. Open `http://localhost:3000`
2. Should see TipidUlam homepage with:
   - Budget input box
   - Family size dropdown
   - "Find Recipes" button
3. Enter ₱500 budget, select 4 people, click button
4. Should return filtered recipes

---

## Running the Application

### Daily Development Workflow

**Terminal 1 - Backend**:
```bash
cd c:\TipidUlam\TipidUlam.Backend
dotnet run

# Or with auto-reload:
dotnet watch run
```

**Terminal 2 - Frontend**:
```bash
cd c:\TipidUlam\TipidUlam.Frontend
npm start

# Or with custom port:
PORT=3000 npm start
```

**Terminal 3 - Database** (Optional - if not auto-starting):
```bash
# Windows
pg_ctl -D "C:\Program Files\PostgreSQL\15\data" start

# macOS
brew services start postgresql

# Linux
sudo service postgresql start
```

### Access Points

| Service | URL | Purpose |
|---------|-----|---------|
| Frontend | `http://localhost:3000` | User interface |
| Backend API | `http://localhost:5000` | API endpoints |
| API Docs (Swagger) | `http://localhost:5000/swagger` | API documentation |
| Database | `localhost:5432` | PostgreSQL server |

---

## Project Structure Quick Reference

### Backend Files You'll Edit

```
TipidUlam.Backend/
├── Models/
│   ├── Recipe.cs              ← Add recipe properties
│   ├── Ingredient.cs          ← Add ingredient fields
│   └── User.cs                ← Add user roles
├── Services/
│   ├── BudgetMatchingService.cs  ← Core algorithm (DON'T MODIFY)
│   └── IngredientService.cs     ← Business logic
├── Controllers/
│   ├── RecipesController.cs    ← Add API endpoints
│   └── IngredientsController.cs ← Add admin endpoints
├── appsettings.json           ← Database & JWT config
└── Startup.cs                 ← Register services
```

### Frontend Files You'll Edit

```
TipidUlam.Frontend/
├── src/
│   ├── components/
│   │   ├── HomePage.jsx       ← Budget search form
│   │   └── RecipeResults.jsx  ← Recipe display
│   ├── context/
│   │   └── TipidUlamContext.js ← Global state
│   ├── services/
│   │   └── api.js             ← API calls
│   └── App.jsx                ← Main component
├── .env                       ← API URL config
└── package.json               ← Dependencies
```

---

## Common Issues & Solutions

### Issue: "npm not found"

**Solution**:
```bash
# Reinstall Node.js
# Make sure to restart terminal after installation
node --version  # Should work now
```

### Issue: "Cannot connect to database"

**Solution**:
```bash
# Check PostgreSQL is running
# Windows: Check Services (services.msc)
# macOS: brew services list
# Linux: sudo systemctl status postgresql

# Verify connection string in appsettings.json
# Test connection:
psql -U postgres -d tipidulam_db
```

### Issue: Port 5000/3000 already in use

**Solution**:
```bash
# For Backend - change port in Startup.cs
# Or use environment variable:
set ASPNETCORE_URLS=http://localhost:5005
dotnet run

# For Frontend:
PORT=3001 npm start
```

### Issue: "React app not loading"

**Solution**:
1. Check backend is running: `http://localhost:5000/api/recipes`
2. Check .env has correct API_URL
3. Clear browser cache (Ctrl+Shift+Delete)
4. Restart npm: `Ctrl+C` then `npm start`

### Issue: CORS error in browser console

**Solution**:
```csharp
// In Startup.cs, verify CORS is configured:
services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder
            .WithOrigins("http://localhost:3000", "https://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

app.UseCors("AllowReactApp");
```

---

## Development Best Practices

### Code Style

**C# Backend**:
```csharp
// Use PascalCase for classes, methods, properties
public class RecipeService 
{
    public async Task<Recipe> GetRecipeByIdAsync(int id) { }
}
```

**JavaScript Frontend**:
```javascript
// Use camelCase for variables, functions
const getRecipeById = async (id) => { }
```

### Git Workflow

```bash
# Create feature branch
git checkout -b feature/add-recipes

# Commit changes
git add .
git commit -m "feat: add recipe search functionality"

# Push to remote
git push origin feature/add-recipes

# Create Pull Request on GitHub
```

### Testing

```bash
# Backend unit tests
dotnet test

# Frontend component tests
npm test

# E2E testing (Cypress - optional)
npm run cypress
```

---

## Next Steps

### After Setup Complete

1. **Explore the code**:
   - Read [Backend README.md](TipidUlam.Backend/README.md)
   - Read [Frontend README.md](TipidUlam.Frontend/README.md)

2. **Test functionality**:
   - Add sample data
   - Create test recipes
   - Test budget search

3. **Customize**:
   - Add more recipes to database
   - Modify UI colors/fonts
   - Add new features

4. **Deployment**:
   - See deployment sections in respective READMEs
   - Deploy backend to Heroku/Azure
   - Deploy frontend to Vercel/Netlify

---

## Support & Resources

### Official Documentation
- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [React Documentation](https://react.dev)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)

### Community
- [Stack Overflow](https://stackoverflow.com)
- [GitHub Issues](https://github.com/issues)
- [Discord Communities](https://discord.com)

### Local Resources
- Backend README: `TipidUlam.Backend/README.md`
- Frontend README: `TipidUlam.Frontend/README.md`
- Quick Reference: `QUICK_REFERENCE.md`

---

## Verification Checklist

After completing this guide:

- [ ] PostgreSQL installed and running
- [ ] Database `tipidulam_db` created
- [ ] Backend running at `http://localhost:5000`
- [ ] Frontend running at `http://localhost:3000`
- [ ] Can visit `http://localhost:5000/api/recipes`
- [ ] Can see TipidUlam homepage
- [ ] Can enter budget and search
- [ ] Git initialized and .gitignore in place
- [ ] Project added to IDE
- [ ] Both terminals can run without errors

**Once all items checked** ✅ **You're ready to develop!**

---

**Estimated Setup Time**: 45-60 minutes

**Need Help?**
1. Check [Troubleshooting](#troubleshooting) section
2. Review specific README files
3. Create GitHub issue with error details
4. Search Stack Overflow for error message

---

**Last Updated**: May 25, 2026
**Version**: 1.0.0
