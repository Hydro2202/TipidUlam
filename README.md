# Capstone - Full Stack Project Management Application

A professional full-stack web application built with **ASP.NET Core 8**, **React 18**, **TypeScript**, and **PostgreSQL**.

## 📋 Project Overview

This is a complete project management platform featuring:

- **Backend**: ASP.NET Core 8 with Entity Framework Core
- **Frontend**: React 18 with TypeScript and Bootstrap 5
- **Database**: PostgreSQL
- **Architecture**: RESTful API with clean separation of concerns

## 🏗 Project Structure

```
Capstone/
├── backend/
│   └── CapstoneAPI/
│       ├── Controllers/          # API endpoints
│       ├── Services/             # Business logic
│       ├── Models/               # Data models
│       ├── DTOs/                 # Data Transfer Objects
│       ├── Data/                 # Database context
│       ├── Mappings/             # AutoMapper configurations
│       ├── CapstoneAPI.csproj    # Project file
│       ├── Program.cs            # Entry point
│       └── appsettings.json      # Configuration
├── frontend/
│   └── capstone-app/
│       ├── src/
│       │   ├── components/       # React components
│       │   ├── pages/            # Page components
│       │   ├── services/         # API client & context
│       │   ├── types/            # TypeScript types
│       │   ├── App.tsx           # Main app component
│       │   └── main.tsx          # Entry point
│       ├── package.json          # Dependencies
│       ├── vite.config.ts        # Vite configuration
│       └── index.html            # HTML template
├── docker-compose.yml            # PostgreSQL setup
└── README.md
```

## 🚀 Quick Start

### Prerequisites

- .NET 8 SDK: [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- Node.js 18+: [Download](https://nodejs.org/)
- Docker & Docker Compose: [Download](https://www.docker.com/)
- PostgreSQL (optional, use Docker instead)

### 1️⃣ Set Up Database

**Option A: Using Docker (Recommended)**

```bash
docker-compose up -d
```

**Option B: Using Local PostgreSQL**

Update connection string in `backend/CapstoneAPI/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "User ID=postgres;Password=your_password;Host=localhost;Port=5432;Database=capstone_db;"
  }
}
```

### 2️⃣ Set Up Backend

```bash
cd backend/CapstoneAPI

# Restore packages
dotnet restore

# Apply database migrations
dotnet ef database update

# Run the backend server
dotnet run
```

Backend runs on: `https://localhost:5000`
Swagger API docs: `https://localhost:5000/swagger`

### 3️⃣ Set Up Frontend

```bash
cd frontend/capstone-app

# Install dependencies
npm install

# Start development server
npm run dev
```

Frontend runs on: `http://localhost:3000`

## 🔐 Demo Login Credentials

Use these credentials to test the application after setup:

- **Email**: `john@example.com`
- **Password**: `password123`

Or

- **Email**: `jane@example.com`
- **Password**: `password123`

## 📚 API Endpoints

### Users
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create user
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user
- `POST /api/users/login` - Login user

### Projects
- `GET /api/projects` - Get all projects
- `GET /api/projects/{id}` - Get project details
- `GET /api/projects/user/{userId}` - Get user's projects
- `POST /api/projects` - Create project
- `PUT /api/projects/{id}` - Update project
- `DELETE /api/projects/{id}` - Delete project

### Tasks
- `GET /api/projects/{projectId}/tasks` - Get project tasks
- `GET /api/projects/{projectId}/tasks/{id}` - Get task details
- `POST /api/projects/{projectId}/tasks` - Create task
- `PUT /api/projects/{projectId}/tasks/{id}` - Update task
- `DELETE /api/projects/{projectId}/tasks/{id}` - Delete task

## 🛠 Technology Stack

### Backend
- **Framework**: ASP.NET Core 8
- **ORM**: Entity Framework Core 8
- **Database**: PostgreSQL
- **API Docs**: Swagger/OpenAPI
- **Mapping**: AutoMapper
- **Security**: BCrypt for password hashing
- **Authentication**: JWT tokens ready

### Frontend
- **Framework**: React 18
- **Language**: TypeScript
- **Build Tool**: Vite
- **Routing**: React Router v6
- **HTTP Client**: Axios
- **UI Framework**: Bootstrap 5
- **State Management**: React Context API

## 📝 Development Commands

### Backend
```bash
# Restore dependencies
dotnet restore

# Build project
dotnet build

# Run development server
dotnet run

# Create migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Run tests (when added)
dotnet test
```

### Frontend
```bash
# Install dependencies
npm install

# Start development server
npm run dev

# Build for production
npm run build

# Preview production build
npm run preview

# Lint code
npm run lint
```

## 🐳 Docker Commands

```bash
# Start PostgreSQL
docker-compose up -d

# Stop PostgreSQL
docker-compose down

# View logs
docker-compose logs postgres

# Access PostgreSQL CLI
docker exec -it capstone_db psql -U postgres -d capstone_db
```

## 🔍 Project Features

### Backend Features
✅ RESTful API with proper HTTP methods
✅ Comprehensive error handling
✅ Logging with ILogger
✅ Dependency Injection
✅ AutoMapper for DTO mapping
✅ Entity Framework Core with migrations
✅ CORS configuration
✅ Swagger/OpenAPI documentation
✅ Sample data seeding

### Frontend Features
✅ Responsive Bootstrap 5 design
✅ React Router for navigation
✅ TypeScript for type safety
✅ Axios API client
✅ Auth context for state management
✅ Protected routes
✅ Clean component structure
✅ Professional UI/UX

## 🚢 Deployment

### Backend (ASP.NET)
- Deploy to Azure App Service
- Deploy to AWS Elastic Beanstalk
- Deploy to Heroku with buildpack
- Docker containerization ready

### Frontend (React)
- Build: `npm run build`
- Deploy to Vercel, Netlify, GitHub Pages
- Deploy to Azure Static Web Apps
- Docker containerization ready

## 📖 Environment Variables

### Backend (.env or appsettings)
```
ConnectionStrings__DefaultConnection=...
ASPNETCORE_ENVIRONMENT=Development
```

### Frontend (.env.local)
```
VITE_API_URL=http://localhost:5000/api
```

## 🐛 Troubleshooting

### Database Connection Issues
- Verify PostgreSQL is running: `docker ps`
- Check connection string in `appsettings.Development.json`
- Ensure firewall allows port 5432

### API CORS Issues
- Verify CORS is configured in `Program.cs`
- Check that frontend URL matches allowed origin

### Frontend API Connection
- Check that backend is running on `https://localhost:5000`
- Verify proxy configuration in `vite.config.ts`
- Check browser console for error messages

## 📞 Support

For issues or questions, refer to:
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [React Documentation](https://react.dev/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)

---

**Version**: 1.0.0
**Last Updated**: 2026
