import React from 'react';
import { useTipidUlam } from '../context/TipidUlamContext';
import HomePage from '../components/HomePage';
import RecipeResults from '../components/RecipeResults';

const PlannerPage = () => {
  const { searchResults, loading, error, budget, familySize } = useTipidUlam();
  const showResults = searchResults.length > 0 || loading || error;

  return (
    <>
      <HomePage />
      {showResults && (
        <RecipeResults
          recipes={searchResults}
          loading={loading}
          error={error}
          budget={budget}
          familySize={familySize}
        />
      )}
    </>
  );
};

export default PlannerPage;
