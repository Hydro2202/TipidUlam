import React, { useState, useEffect } from 'react';
import { useTipidUlam } from '../context/TipidUlamContext';
import { recipeService, ingredientService } from '../services/api';
import './HomePage.css';

const HomePage = () => {
  const {
    budget,
    familySize,
    pantryIngredients,
    loading,
    error,
    allIngredients,
    setBudget,
    setFamilySize,
    toggleIngredientInPantry,
    clearPantry,
    setSearchResults,
    setLoading,
    setError,
    setAllIngredients,
  } = useTipidUlam();

  const [pantryOpen, setPantryOpen] = useState(false);
  const [localBudget, setLocalBudget] = useState(budget);
  const [localFamilySize, setLocalFamilySize] = useState(familySize);

  useEffect(() => {
    const loadIngredients = async () => {
      try {
        const ingredients = await ingredientService.getAllIngredients();
        setAllIngredients(ingredients);
      } catch (err) {
        console.error('Failed to load ingredients:', err);
        setError('Could not load pantry items. Try again in a moment.');
      }
    };

    loadIngredients();
  }, [setAllIngredients, setError]);

  useEffect(() => {
    setBudget(localBudget);
  }, [localBudget, setBudget]);

  useEffect(() => {
    setFamilySize(localFamilySize);
  }, [localFamilySize, setFamilySize]);

  const handleSearch = async () => {
    if (!localBudget || localBudget <= 0) {
      setError('Enter a budget greater than zero.');
      return;
    }

    setLoading(true);
    setError(null);

    try {
      const result = await recipeService.searchByBudget(
        localBudget,
        localFamilySize,
        pantryIngredients
      );
      setSearchResults(result.meals || result.recipes || []);
    } catch (err) {
      setError(err.message || 'Search failed. Check your connection and try again.');
      setSearchResults([]);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    handleSearch();
  };

  const ingredientsByCategory = allIngredients.reduce((acc, ingredient) => {
    const category = ingredient.category || 'other';
    const key = category.charAt(0).toUpperCase() + category.slice(1);
    if (!acc[key]) acc[key] = [];
    acc[key].push(ingredient);
    return acc;
  }, {});

  return (
    <section className="planner">
      <header className="planner-intro">
        <h1>Plan this week&apos;s ulam</h1>
        <p>
          Enter what you can spend and how many mouths to feed. We scale recipes and
          subtract pantry items you already own.
        </p>
      </header>

      <form className="planner-form panel" onSubmit={handleSubmit}>
        {error && (
          <div className="form-banner form-banner--error" role="alert">
            {error}
          </div>
        )}

        <div className="planner-fields">
          <div className="field">
            <label htmlFor="budget">Weekly food budget</label>
            <div className="input-with-prefix">
              <span className="input-prefix" aria-hidden="true">
                PHP
              </span>
              <input
                id="budget"
                type="number"
                min="0.01"
                step="any"
                value={localBudget}
                onChange={(e) => {
                  const value = e.target.value;
                  setLocalBudget(value === '' ? '' : parseFloat(value));
                }}
                placeholder="500"
              />
            </div>
            <span className="field-hint">Total amount for one meal plan run</span>
          </div>

          <div className="field">
            <label htmlFor="familySize">Household size</label>
            <select
              id="familySize"
              value={localFamilySize}
              onChange={(e) => setLocalFamilySize(parseInt(e.target.value, 10))}
            >
              {[1, 2, 3, 4, 5, 6, 7, 8, 9, 10].map((size) => (
                <option key={size} value={size}>
                  {size} {size === 1 ? 'person' : 'people'}
                </option>
              ))}
            </select>
          </div>
        </div>

        <div className="pantry-block">
          <button
            type="button"
            className={`pantry-toggle ${pantryOpen ? 'is-open' : ''}`}
            onClick={() => setPantryOpen(!pantryOpen)}
            aria-expanded={pantryOpen}
          >
            <span className="pantry-toggle-label">Pantry</span>
            <span className="pantry-toggle-meta">
              {pantryIngredients.length === 0
                ? 'Nothing selected'
                : `${pantryIngredients.length} item${pantryIngredients.length === 1 ? '' : 's'} on hand`}
            </span>
          </button>

          {pantryOpen && (
            <div className="pantry-panel">
              {Object.keys(ingredientsByCategory).length === 0 ? (
                <p className="pantry-empty">Loading ingredients…</p>
              ) : (
                Object.entries(ingredientsByCategory).map(([category, items]) => (
                  <div key={category} className="pantry-group">
                    <h3 className="pantry-group-title">{category}</h3>
                    <ul className="pantry-list">
                      {items.map((ingredient) => (
                        <li key={ingredient.id}>
                          <label className="pantry-item">
                            <input
                              type="checkbox"
                              checked={pantryIngredients.includes(ingredient.id)}
                              onChange={() => toggleIngredientInPantry(ingredient.id)}
                            />
                            <span className="pantry-item-name">{ingredient.name}</span>
                            <span className="pantry-item-price">
                              {ingredient.pricePerUnit}/{ingredient.unitOfMeasure}
                            </span>
                          </label>
                        </li>
                      ))}
                    </ul>
                  </div>
                ))
              )}
              {pantryIngredients.length > 0 && (
                <button type="button" className="btn btn-ghost btn-sm pantry-clear" onClick={clearPantry}>
                  Clear pantry
                </button>
              )}
            </div>
          )}
        </div>

        <button type="submit" className="btn btn-primary planner-submit" disabled={loading}>
          {loading ? (
            <>
              <span className="spinner-inline" aria-hidden="true" />
              Searching…
            </>
          ) : (
            'Find meals within budget'
          )}
        </button>
      </form>
    </section>
  );
};

export default HomePage;
