# TipidUlam - Frontend (ReactJS)

## Overview
TipidUlam Frontend is a modern, mobile-first ReactJS single-page application that connects to the ASP.NET Core budget meal planning API.

### Key Features
- **Beautiful, Minimal UI**: Emerald green theme, mobile-optimized design
- **Real-time Budget Search**: Instant recipe filtering based on budget and family size
- **Pantry Checklist**: Select ingredients you already own to reduce recipe costs
- **Global State Management**: Context API with custom hooks
- **Responsive Design**: Works perfectly on mobile, tablet, and desktop

---

## Technical Stack
- **Framework**: ReactJS 18.2+
- **State Management**: Context API with Hooks
- **Styling**: CSS3 with CSS Variables (modern design system)
- **HTTP Client**: Fetch API
- **Build Tool**: Create React App with Webpack
- **Node.js**: 14+ LTS

---

## Project Structure

```
TipidUlam.Frontend/
├── public/
│   └── index.html              # Entry HTML file
├── src/
│   ├── components/
│   │   ├── HomePage.jsx        # Main search component
│   │   ├── HomePage.css
│   │   ├── RecipeResults.jsx   # Results display component
│   │   └── RecipeResults.css
│   ├── context/
│   │   └── TipidUlamContext.js # Global state management
│   ├── services/
│   │   └── api.js              # API service layer
│   ├── styles/
│   │   └── variables.css       # Design system & variables
│   ├── App.jsx                 # Main app component
│   ├── App.css
│   └── index.js                # React entry point
├── .env                        # Development environment variables
├── .env.production             # Production environment variables
├── package.json
└── README.md
```

---

## Getting Started

### Prerequisites
- Node.js 14+ LTS
- npm or yarn package manager
- Backend API running at `http://localhost:5000`

### 1. Install Dependencies

```bash
cd TipidUlam.Frontend
npm install
```

### 2. Configure Environment

Edit `.env`:
```
REACT_APP_API_URL=http://localhost:5000/api
```

### 3. Start Development Server

```bash
npm start
```

The app will open at `http://localhost:3000`

### 4. Build for Production

```bash
npm run build
```

Output in `build/` directory

---

## Application Architecture

### Global State (Context API)

**File**: `src/context/TipidUlamContext.js`

**Managed State**:
```javascript
{
  budget: number,                    // Current budget in ₱
  familySize: number,                // Number of people
  pantryIngredients: number[],        // Ingredient IDs owned
  searchResults: Recipe[],            // Filtered recipes
  allIngredients: Ingredient[],       // All available ingredients
  loading: boolean,
  error: string | null
}
```

**Actions**:
- `setBudget(amount)` - Update budget
- `setFamilySize(size)` - Update family size
- `toggleIngredientInPantry(id)` - Add/remove from pantry
- `clearPantry()` - Reset pantry selection
- `setSearchResults(recipes)` - Set filtered results
- `setLoading(bool)` - Set loading state
- `setError(message)` - Set error message

### API Service

**File**: `src/services/api.js`

**Exported Functions**:

```javascript
// Recipe endpoints
recipeService.searchByBudget(maxBudget, familySize, pantryIds)
recipeService.getAllRecipes()
recipeService.getRecipeById(id)

// Ingredient endpoints
ingredientService.getAllIngredients()
ingredientService.getByCategory(category)
ingredientService.getIngredientById(id)
ingredientService.create(data, token)    // Admin
ingredientService.update(id, data, token) // Admin
ingredientService.delete(id, token)       // Admin
```

### Component Hierarchy

```
App
└── TipidUlamProvider (Context)
    └── AppContent
        ├── HomePage
        │   ├── Budget Input
        │   ├── Family Size Selector
        │   └── Pantry Checklist
        └── RecipeResults (Conditional)
            └── Recipe Cards Grid
```

---

## UI/UX Design System

### Color Palette

| Variable | Color | Usage |
|----------|-------|-------|
| `--color-primary` | #10B981 (Emerald) | Primary buttons, headings |
| `--color-primary-light` | #D1FAE5 | Hover states, backgrounds |
| `--color-primary-dark` | #059669 | Active states, gradients |
| `--color-white` | #FFFFFF | Cards, overlays |
| `--color-gray-100` | #F9FAFB | Page background |
| `--color-gray-500` | #9CA3AF | Secondary text |
| `--color-gray-900` | #1F2937 | Primary text |

### Typography

```css
Body: 16px (1rem) - Roboto, system fonts
Headings: 2.25rem (4xl) - Semibold/Bold
Small: 0.875rem (sm) - Secondary info
```

### Spacing Scale

- `--spacing-base`: 1rem (16px)
- `--spacing-lg`: 1.5rem (24px)
- `--spacing-xl`: 2rem (32px)
- `--spacing-2xl`: 2.5rem (40px)

### Shadows & Borders

```css
--shadow-base: 0 1px 3px rgba(0,0,0,0.1)
--shadow-lg: 0 10px 15px rgba(0,0,0,0.1)
--radius-md: 0.75rem (12px)
--radius-lg: 1rem (16px)
```

---

## Key Components

### HomePage Component

**Props**: None (uses Context)

**Features**:
- Budget input with currency symbol (₱)
- Family size dropdown (1-10 people)
- Expandable pantry checklist organized by category
- Search button with loading indicator
- Error display
- Enter key support on budget input

**Workflow**:
1. User enters budget
2. User selects family size
3. (Optional) User selects pantry ingredients
4. User clicks "Find Recipes"
5. Component calls `searchByBudget()` API
6. Results displayed in RecipeResults component

### RecipeResults Component

**Props**:
- `recipes`: Array of recipe objects
- `loading`: Boolean
- `error`: Error message or null
- `budget`: Current budget
- `familySize`: Current family size

**Features**:
- Responsive grid layout (1→2→3 columns)
- Recipe cards with metadata
- Ingredient list with scaled quantities and costs
- Total cost calculation display
- Loading spinner animation
- Empty state messaging

---

## Responsive Breakpoints

```css
Mobile (< 480px)
  - Single column grid
  - Larger touch targets
  - Simplified layouts

Tablet (480px - 768px)
  - 2-column grid
  - Comfortable spacing

Desktop (> 768px)
  - 2-3 column grid
  - Optimized spacing
  - Hover effects
```

---

## API Integration

### Example: Budget Search

```javascript
// HomePage.jsx
const handleSearch = async () => {
  setLoading(true);
  
  try {
    const result = await recipeService.searchByBudget(
      budget,           // e.g., 500
      familySize,       // e.g., 4
      pantryIngredients // e.g., [1, 2, 3]
    );
    setSearchResults(result.recipes);
  } catch (err) {
    setError('Failed to search recipes');
  } finally {
    setLoading(false);
  }
};
```

### Error Handling

```javascript
// All API calls include error handling
try {
  const data = await recipeService.searchByBudget(...);
} catch (error) {
  console.error('API Error:', error);
  setError('User-friendly error message');
}
```

---

## Environment Variables

### Development (.env)
```
REACT_APP_API_URL=http://localhost:5000/api
```

### Production (.env.production)
```
REACT_APP_API_URL=https://api.tipidulam.com/api
```

---

## Performance Optimizations

1. **Context API**: Efficient state management for small/medium apps
2. **CSS Variables**: Reduced CSS file size
3. **Lazy Loading**: Recipe results load on demand
4. **Memoization**: Components only re-render when state changes

---

## Testing

```bash
# Run tests
npm test

# Run tests with coverage
npm test -- --coverage

# Run tests in watch mode
npm test -- --watch
```

---

## Building & Deployment

### Build Optimization
```bash
npm run build
# Creates optimized production build in ./build
```

### Deploy to Vercel
```bash
npm i -g vercel
vercel
```

### Deploy to Netlify
```bash
npm run build
# Drag ./build folder to Netlify
```

### Docker
```dockerfile
FROM node:18-alpine AS build
WORKDIR /app
COPY package.json .
RUN npm install
COPY . .
RUN npm run build

FROM node:18-alpine
WORKDIR /app
RUN npm install -g serve
COPY --from=build /app/build ./build
EXPOSE 3000
CMD ["serve", "-s", "build", "-l", "3000"]
```

---

## Browser Support

- Chrome/Edge: Latest 2 versions
- Firefox: Latest 2 versions
- Safari: Latest 2 versions
- Mobile browsers: iOS Safari 12+, Chrome Android

---

## Accessibility

- Semantic HTML elements
- ARIA labels on form inputs
- Keyboard navigation support
- Color contrast ratios meet WCAG AA
- Focus indicators on interactive elements

---

## License
MIT License - See LICENSE file

---

## Support
For issues or questions, please create an issue in the repository.
