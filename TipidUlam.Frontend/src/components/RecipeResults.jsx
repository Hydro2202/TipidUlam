import React from 'react';
import { useTipidUlam } from '../context/TipidUlamContext';
import RecipeMealCard from './RecipeMealCard';
import './RecipeResults.css';

const formatPeso = (amount) =>
  new Intl.NumberFormat('en-PH', {
    style: 'currency',
    currency: 'PHP',
    minimumFractionDigits: 2,
  }).format(amount);

const RecipeResults = ({ recipes, loading, error, budget, familySize }) => {
  const { pantryIngredients, setSearchResults } = useTipidUlam();

  const handleMealUpdated = (updatedMeal) => {
    setSearchResults((prev) =>
      prev.map((m) => (m.id === updatedMeal.id ? updatedMeal : m))
    );
  };

  if (loading) {
    return (
      <section className="results" aria-live="polite">
        <div className="results-state">
          <div className="boot-spinner" aria-hidden="true" />
          <p>Matching recipes to your budget…</p>
        </div>
      </section>
    );
  }

  if (error) {
    return (
      <section className="results">
        <div className="results-state results-state--error">
          <p>{error}</p>
        </div>
      </section>
    );
  }

  if (!recipes?.length) {
    return (
      <section className="results">
        <div className="results-state">
          <p className="results-state-title">No meals fit this budget</p>
          <p className="results-state-hint">
            Try a higher amount, fewer people, or add pantry items you already have.
          </p>
        </div>
      </section>
    );
  }

  const withinBudget = recipes.filter(
    (m) => m.fitsBudget !== false && (m.totalCost ?? m.totalCostForFamily ?? 0) <= budget
  );
  const overBudget = recipes.filter(
    (m) => !withinBudget.includes(m)
  );

  return (
    <section className="results">
      <header className="results-header">
        <div>
          <h2>
            {withinBudget.length} meal{withinBudget.length !== 1 ? 's' : ''} within budget
          </h2>
          <p className="results-meta">
            {formatPeso(budget)} · {familySize} {familySize === 1 ? 'person' : 'people'}
          </p>
        </div>
      </header>

      {withinBudget.length > 0 && (
        <ul className="results-list">
          {withinBudget.map((meal) => (
            <RecipeMealCard
              key={meal.id}
              meal={meal}
              budget={budget}
              familySize={familySize}
              pantryIngredientIds={pantryIngredients}
              onMealUpdated={handleMealUpdated}
            />
          ))}
        </ul>
      )}

      {overBudget.length > 0 && (
        <>
          <h3 className="results-subheading">Adjusted over budget</h3>
          <p className="results-subhint">
            These meals exceeded your limit after you edited amounts or prices.
          </p>
          <ul className="results-list">
            {overBudget.map((meal) => (
              <RecipeMealCard
                key={meal.id}
                meal={meal}
                budget={budget}
                familySize={familySize}
                pantryIngredientIds={pantryIngredients}
                onMealUpdated={handleMealUpdated}
              />
            ))}
          </ul>
        </>
      )}
    </section>
  );
};

export default RecipeResults;
