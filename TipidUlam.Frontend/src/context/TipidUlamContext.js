import React, { createContext, useContext, useState, useCallback } from 'react';

/**
 * Global State Context for TipidUlam
 * Manages:
 * - Current budget
 * - Family size
 * - Selected pantry ingredients
 * - Search results (filtered recipes)
 * - Loading and error states
 */

const TipidUlamContext = createContext(null);

export const TipidUlamProvider = ({ children }) => {
  const [budget, setBudget] = useState(500); // Default ₱500
  const [familySize, setFamilySize] = useState(4); // Default 4 people
  const [pantryIngredients, setPantryIngredients] = useState([]); // Array of ingredient IDs
  const [searchResults, setSearchResults] = useState([]); // Array of recipes
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [allIngredients, setAllIngredients] = useState([]); // All available ingredients for pantry

  const addIngredientToPantry = useCallback((ingredientId) => {
    setPantryIngredients(prev =>
      prev.includes(ingredientId) ? prev : [...prev, ingredientId]
    );
  }, []);

  const removeIngredientFromPantry = useCallback((ingredientId) => {
    setPantryIngredients(prev =>
      prev.filter(id => id !== ingredientId)
    );
  }, []);

  const toggleIngredientInPantry = useCallback((ingredientId) => {
    setPantryIngredients(prev =>
      prev.includes(ingredientId)
        ? prev.filter(id => id !== ingredientId)
        : [...prev, ingredientId]
    );
  }, []);

  const clearPantry = useCallback(() => {
    setPantryIngredients([]);
  }, []);

  const updateBudget = useCallback((newBudget) => {
    setBudget(Math.max(0, newBudget));
  }, []);

  const updateFamilySize = useCallback((newSize) => {
    setFamilySize(Math.max(1, newSize));
  }, []);

  const value = {
    // State
    budget,
    familySize,
    pantryIngredients,
    searchResults,
    loading,
    error,
    allIngredients,

    // Actions
    setBudget: updateBudget,
    setFamilySize: updateFamilySize,
    addIngredientToPantry,
    removeIngredientFromPantry,
    toggleIngredientInPantry,
    clearPantry,
    setSearchResults, // supports functional updates: setSearchResults(prev => ...)
    setLoading,
    setError,
    setAllIngredients,
  };

  return (
    <TipidUlamContext.Provider value={value}>
      {children}
    </TipidUlamContext.Provider>
  );
};

export const useTipidUlam = () => {
  const context = useContext(TipidUlamContext);
  if (!context) {
    throw new Error('useTipidUlam must be used within TipidUlamProvider');
  }
  return context;
};
