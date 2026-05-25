const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';
const TOKEN_KEY = 'tipidulam_token';

export const getStoredToken = () => localStorage.getItem(TOKEN_KEY);

export const setStoredToken = (token) => {
  if (token) {
    localStorage.setItem(TOKEN_KEY, token);
  } else {
    localStorage.removeItem(TOKEN_KEY);
  }
};

async function parseError(response) {
  try {
    const data = await response.json();
    return data.message || data.title || `Request failed (${response.status})`;
  } catch {
    return `Request failed (${response.status})`;
  }
}

export async function apiFetch(path, options = {}) {
  const headers = {
    'Content-Type': 'application/json',
    ...options.headers,
  };

  const token = getStoredToken();
  if (token) {
    headers.Authorization = `Bearer ${token}`;
  }

  let response;
  try {
    response = await fetch(`${API_BASE_URL}${path}`, {
      ...options,
      headers,
    });
  } catch {
    throw new Error(
      `Cannot reach the API at ${API_BASE_URL}. Start the backend (dotnet run in TipidUlam.Backend) and use http://localhost:3000 in the browser.`
    );
  }

  if (!response.ok) {
    throw new Error(await parseError(response));
  }

  if (response.status === 204) {
    return null;
  }

  return response.json();
}

export const authService = {
  register: (username, email, password) =>
    apiFetch('/auth/register', {
      method: 'POST',
      body: JSON.stringify({ username, email, password }),
    }),

  login: (email, password) =>
    apiFetch('/auth/login', {
      method: 'POST',
      body: JSON.stringify({ email, password }),
    }),

  me: () => apiFetch('/auth/me'),
};

export const recipeService = {
  searchByBudget: (maxBudget, familySize, pantryIds = []) => {
    let url = `/recipes/search?maxBudget=${maxBudget}&familySize=${familySize}`;
    if (pantryIds.length > 0) {
      url += `&pantryIds=${pantryIds.join(',')}`;
    }
    return apiFetch(url);
  },

  searchByBudgetPost: (maxBudget, familySize, pantryIds = []) =>
    apiFetch('/recipes/search', {
      method: 'POST',
      body: JSON.stringify({
        maxBudget,
        familySize,
        pantryIngredientIds: pantryIds,
      }),
    }),

  calculateCost: (recipeId, {
    maxBudget,
    familySize,
    pantryIngredientIds = [],
    ingredientLines,
    anchorIngredientId,
    anchorQuantity,
  }) =>
    apiFetch(`/recipes/${recipeId}/calculate`, {
      method: 'POST',
      body: JSON.stringify({
        maxBudget,
        familySize,
        pantryIngredientIds,
        ingredientLines,
        anchorIngredientId,
        anchorQuantity,
      }),
    }),

  getAllRecipes: () => apiFetch('/recipes'),
  getRecipeById: (id) => apiFetch(`/recipes/${id}`),
};

export const ingredientService = {
  getAllIngredients: () => apiFetch('/ingredients'),
  getByCategory: (category) => apiFetch(`/ingredients/category/${category}`),
  getIngredientById: (id) => apiFetch(`/ingredients/${id}`),
};
