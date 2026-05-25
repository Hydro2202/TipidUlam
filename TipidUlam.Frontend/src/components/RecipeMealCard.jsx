import React, { useCallback, useEffect, useRef, useState } from 'react';
import { recipeService } from '../services/api';

const formatPeso = (amount) =>
  new Intl.NumberFormat('en-PH', {
    style: 'currency',
    currency: 'PHP',
    minimumFractionDigits: 2,
  }).format(amount);

const mealTotal = (recipe) => recipe.totalCost ?? recipe.totalCostForFamily ?? 0;

const RecipeMealCard = ({
  meal,
  budget,
  familySize,
  pantryIngredientIds,
  onMealUpdated,
}) => {
  const [expanded, setExpanded] = useState(false);
  const [recalculating, setRecalculating] = useState(false);
  const [recalcError, setRecalcError] = useState(null);
  const [proportionalScale, setProportionalScale] = useState(false);
  const debounceRef = useRef(null);

  const [draftLines, setDraftLines] = useState(() =>
    buildDraftFromMeal(meal)
  );

  useEffect(() => {
    setDraftLines(buildDraftFromMeal(meal));
  }, [meal]);

  const runRecalculate = useCallback(
    async (lines, useAnchor = false, anchorIngredientId = null) => {
      setRecalculating(true);
      setRecalcError(null);

      try {
        const payload = {
          maxBudget: budget,
          familySize,
          pantryIngredientIds,
        };

        if (useAnchor && anchorIngredientId != null) {
          const anchor = lines.find((l) => l.ingredientId === anchorIngredientId);
          payload.anchorIngredientId = anchorIngredientId;
          payload.anchorQuantity = parseFloat(anchor.quantity) || 0;
        } else {
          payload.ingredientLines = lines.map((l) => ({
            ingredientId: l.ingredientId,
            requiredQuantity: parseFloat(l.quantity) || 0,
            pricePerBaseUnit: parseFloat(l.price) || 0,
          }));
        }

        const response = await recipeService.calculateCost(meal.id, payload);
        onMealUpdated(response.meal);
        setDraftLines(buildDraftFromMeal(response.meal));
      } catch (err) {
        setRecalcError(err.message || 'Could not update costs.');
      } finally {
        setRecalculating(false);
      }
    },
    [meal.id, budget, familySize, pantryIngredientIds, onMealUpdated]
  );

  const scheduleRecalculate = useCallback(
    (lines, anchorId = null) => {
      if (debounceRef.current) clearTimeout(debounceRef.current);
      debounceRef.current = setTimeout(() => {
        runRecalculate(lines, proportionalScale && anchorId != null, anchorId);
      }, 500);
    },
    [runRecalculate, proportionalScale]
  );

  const handleLineChange = (ingredientId, field, value) => {
    const next = draftLines.map((line) =>
      line.ingredientId === ingredientId ? { ...line, [field]: value } : line
    );
    setDraftLines(next);
    scheduleRecalculate(next, field === 'quantity' ? ingredientId : null);
  };

  const handleResetLines = () => {
    const reset = draftLines.map((line) => ({
      ...line,
      quantity: String(line.baselineQuantity),
      price: String(line.defaultPrice),
    }));
    setDraftLines(reset);
    runRecalculate(reset, false);
  };

  const total = mealTotal(meal);
  const budgetLeft = budget - total;
  const overBudget = budgetLeft < 0;

  return (
    <li
      className={`result-card panel ${overBudget ? 'result-card--over' : ''} ${!meal.fitsBudget && meal.fitsBudget !== undefined ? 'result-card--over' : ''}`}
    >
      <div className="result-card-top">
        <div className="result-card-heading">
          <h3>{meal.name}</h3>
          <div className="result-tags">
            {meal.cuisineType && <span>{meal.cuisineType}</span>}
            {meal.difficultyLevel && <span>{meal.difficultyLevel}</span>}
            {meal.cookingTimeMinutes && <span>{meal.cookingTimeMinutes} min</span>}
          </div>
        </div>
        <div className="result-cost">
          <span className="result-cost-label">Estimated total</span>
          <span className={`result-cost-value ${recalculating ? 'is-updating' : ''}`}>
            {formatPeso(total)}
          </span>
          <span className={`result-cost-remaining ${overBudget ? 'is-over' : ''}`}>
            {overBudget
              ? `${formatPeso(Math.abs(budgetLeft))} over budget`
              : `${formatPeso(budgetLeft)} left in budget`}
          </span>
        </div>
      </div>

      {meal.description && <p className="result-description">{meal.description}</p>}

      <button
        type="button"
        className={`result-expand ${expanded ? 'is-open' : ''}`}
        onClick={() => setExpanded(!expanded)}
        aria-expanded={expanded}
      >
        {expanded ? 'Hide breakdown' : 'Edit amounts & prices'}
      </button>

      {expanded && (
        <div className="result-details">
          <p className="edit-hint">
            Adjust quantity ({draftLines[0]?.unitOfMeasure || 'unit'}) and price per unit to match
            your palengke. Totals update automatically.
          </p>

          <label className="scale-option">
            <input
              type="checkbox"
              checked={proportionalScale}
              onChange={(e) => setProportionalScale(e.target.checked)}
            />
            <span>When I change one amount, scale other ingredients proportionally</span>
          </label>

          {recalcError && (
            <div className="form-banner form-banner--error" role="alert">
              {recalcError}
            </div>
          )}

          <table className="ingredient-table ingredient-table--editable">
            <thead>
              <tr>
                <th scope="col">Item</th>
                <th scope="col">Amount</th>
                <th scope="col">Price / unit</th>
                <th scope="col" className="num">
                  Line cost
                </th>
              </tr>
            </thead>
            <tbody>
              {draftLines.map((line) => {
                const liveIng = meal.ingredients?.find(
                  (i) => i.ingredientId === line.ingredientId
                );
                const lineCost = liveIng?.lineCost ?? 0;

                return (
                  <tr
                    key={line.ingredientId}
                    className={line.isPantryItem ? 'row-pantry' : ''}
                  >
                    <td>
                      {line.ingredientName}
                      {line.isPantryItem && (
                        <span className="pantry-badge">Pantry</span>
                      )}
                    </td>
                    <td>
                      <div className="cell-input-group">
                        <input
                          type="number"
                          min="0.001"
                          step="0.01"
                          className="cell-input"
                          value={line.quantity}
                          onChange={(e) =>
                            handleLineChange(line.ingredientId, 'quantity', e.target.value)
                          }
                          aria-label={`${line.ingredientName} amount`}
                        />
                        <span className="cell-unit">{line.unitOfMeasure}</span>
                      </div>
                    </td>
                    <td>
                      <div className="cell-input-group">
                        <span className="cell-prefix">₱</span>
                        <input
                          type="number"
                          min="0.01"
                          step="0.5"
                          className="cell-input"
                          value={line.price}
                          onChange={(e) =>
                            handleLineChange(line.ingredientId, 'price', e.target.value)
                          }
                          disabled={line.isPantryItem}
                          aria-label={`${line.ingredientName} price per ${line.unitOfMeasure}`}
                        />
                        <span className="cell-unit">/{line.unitOfMeasure}</span>
                      </div>
                    </td>
                    <td className="num">{formatPeso(lineCost)}</td>
                  </tr>
                );
              })}
            </tbody>
          </table>

          <div className="result-actions">
            <button
              type="button"
              className="btn btn-ghost btn-sm"
              onClick={handleResetLines}
              disabled={recalculating}
            >
              Reset to defaults
            </button>
            {recalculating && <span className="recalc-status">Updating…</span>}
          </div>

          {meal.instructions && (
            <div className="result-instructions">
              <h4>Steps</h4>
              <pre>{meal.instructions}</pre>
            </div>
          )}
        </div>
      )}
    </li>
  );
};

function buildDraftFromMeal(meal) {
  return (meal.ingredients || []).map((ing) => ({
    ingredientId: ing.ingredientId,
    ingredientName: ing.ingredientName,
    unitOfMeasure: ing.unitOfMeasure,
    quantity: String(ing.requiredQuantity ?? ing.quantityPerServing ?? 0),
    baselineQuantity: ing.baselineQuantity ?? ing.requiredQuantity ?? 0,
    price: String(ing.pricePerBaseUnit ?? ing.pricePerUnit ?? 0),
    defaultPrice: ing.pricePerBaseUnit ?? ing.pricePerUnit ?? 0,
    isPantryItem: ing.isPantryItem,
  }));
}

export default RecipeMealCard;
