import React, { createContext, useContext, useState, useCallback, useEffect } from 'react';
import { pantryService } from '../services/api';

/**
 * Global State Context for TipidUlam
 * Manages:
 * - Current budget
 * - Family size
 * - Selected pantry ingredients
 * - Search results (filtered recipes)
 * - Loading and error states
 * - User's pantry items (backend synced)
 */

const TipidUlamContext = createContext(null);

export const TipidUlamProvider = ({ children }) => {
  const [budget, setBudget] = useState(500); // Default ₱500
  const [familySize, setFamilySize] = useState(4); // Default 4 people
  const [pantryIngredients, setPantryIngredients] = useState([]); // Array of ingredient IDs (for quick filtering)
  const [pantryItems, setPantryItems] = useState([]); // Full pantry items from backend
  const [searchResults, setSearchResults] = useState([]); // Array of recipes
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [allIngredients, setAllIngredients] = useState([]); // All available ingredients for pantry

  // Sync pantryItems IDs with pantryIngredients when pantryItems changes
  useEffect(() => {
    const ingredientIds = pantryItems.map(item => item.ingredientId);
    setPantryIngredients(ingredientIds);
  }, [pantryItems]);

  // Load pantry from backend on mount
  useEffect(() => {
    const loadPantry = async () => {
      try {
        const items = await pantryService.getPantry();
        setPantryItems(items || []);
      } catch (err) {
        // Silently fail if not authenticated or pantry can't load
        setPantryItems([]);
      }
    };
    loadPantry();
  }, []);

  const addIngredientToPantry = useCallback((ingredientId, quantity = 1, notes = '') => {
    setPantryItems(prev => {
      if (prev.some(item => item.ingredientId === ingredientId)) {
        return prev; // Already exists
      }
      return [...prev, { ingredientId, quantity, notes, id: `temp-${Date.now()}` }];
    });
  }, []);

  const removeIngredientFromPantry = useCallback((ingredientId) => {
    setPantryItems(prev =>
      prev.filter(item => item.ingredientId !== ingredientId)
    );
  }, []);

  const toggleIngredientInPantry = useCallback((ingredientId) => {
    setPantryItems(prev => {
      if (prev.some(item => item.ingredientId === ingredientId)) {
        return prev.filter(item => item.ingredientId !== ingredientId);
      }
      return [...prev, { ingredientId, quantity: 1, notes: '', id: `temp-${Date.now()}` }];
    });
  }, []);

  const updatePantryItem = useCallback((pantryItemId, quantity, notes = '') => {
    setPantryItems(prev =>
      prev.map(item =>
        item.id === pantryItemId ? { ...item, quantity, notes } : item
      )
    );
  }, []);

  const clearPantry = useCallback(() => {
    setPantryItems([]);
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
    pantryItems,
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
    updatePantryItem,
    clearPantry,
    setPantryItems, // Direct update from API
    setSearchResults,
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
