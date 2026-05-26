import React, { useState, useEffect } from 'react';
import { useAuth } from '../context/AuthContext';
import { useTipidUlam } from '../context/TipidUlamContext';
import { pantryService, ingredientService } from '../services/api';
import './PantryPage.css';

const PantryPage = () => {
  const { user } = useAuth();
  const { pantryItems, setPantryItems } = useTipidUlam();
  const [allIngredients, setAllIngredients] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(null);
  const [showAddForm, setShowAddForm] = useState(false);
  const [selectedIngredient, setSelectedIngredient] = useState(null);
  const [quantity, setQuantity] = useState('1');
  const [notes, setNotes] = useState('');
  const [editingId, setEditingId] = useState(null);
  const [editQuantity, setEditQuantity] = useState('');
  const [editNotes, setEditNotes] = useState('');
  const [searchTerm, setSearchTerm] = useState('');

  // Load all ingredients and pantry items on mount
  useEffect(() => {
    const loadData = async () => {
      setLoading(true);
      try {
        const ingredients = await ingredientService.getAllIngredients();
        setAllIngredients(ingredients || []);

        const items = await pantryService.getPantry();
        setPantryItems(items || []);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    if (user) {
      loadData();
    }
  }, [user, setPantryItems]);

  const handleAddIngredient = async (e) => {
    e.preventDefault();
    if (!selectedIngredient || !quantity || parseFloat(quantity) <= 0) {
      setError('Please select an ingredient and enter a valid quantity.');
      return;
    }

    setLoading(true);
    setError(null);
    setSuccess(null);

    try {
      const newItem = await pantryService.addIngredient(
        selectedIngredient,
        parseFloat(quantity),
        notes
      );

      setPantryItems(prev => [...prev, newItem]);
      setSuccess(`${allIngredients.find(i => i.id === selectedIngredient)?.name} added to pantry!`);

      // Reset form
      setSelectedIngredient(null);
      setQuantity('1');
      setNotes('');
      setShowAddForm(false);

      setTimeout(() => setSuccess(null), 3000);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleRemoveItem = async (pantryItemId) => {
    if (!window.confirm('Are you sure you want to remove this item?')) return;

    setLoading(true);
    setError(null);

    try {
      await pantryService.removeItem(pantryItemId);
      setPantryItems(prev => prev.filter(item => item.id !== pantryItemId));
      setSuccess('Item removed from pantry.');
      setTimeout(() => setSuccess(null), 3000);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleStartEdit = (item) => {
    setEditingId(item.id);
    setEditQuantity(item.quantity.toString());
    setEditNotes(item.notes || '');
  };

  const handleUpdateItem = async (pantryItemId) => {
    if (!editQuantity || parseFloat(editQuantity) <= 0) {
      setError('Quantity must be greater than 0.');
      return;
    }

    setLoading(true);
    setError(null);

    try {
      const updated = await pantryService.updateIngredient(
        pantryItemId,
        parseFloat(editQuantity),
        editNotes
      );

      setPantryItems(prev =>
        prev.map(item => (item.id === pantryItemId ? updated : item))
      );

      setEditingId(null);
      setSuccess('Item updated successfully.');
      setTimeout(() => setSuccess(null), 3000);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  // Filter ingredients to show only those not in pantry
  const availableIngredients = allIngredients.filter(
    ing => !pantryItems.some(p => p.ingredientId === ing.id)
  );

  // Filter pantry items by search term
  const filteredPantryItems = pantryItems.filter(item =>
    item.ingredientName.toLowerCase().includes(searchTerm.toLowerCase()) ||
    item.category.toLowerCase().includes(searchTerm.toLowerCase())
  );

  // Group pantry items by category
  const groupedItems = filteredPantryItems.reduce((acc, item) => {
    if (!acc[item.category]) {
      acc[item.category] = [];
    }
    acc[item.category].push(item);
    return acc;
  }, {});

  const categories = Object.keys(groupedItems).sort();

  return (
    <div className="pantry-container">
      <header className="pantry-header">
        <h1>My Pantry</h1>
        <p className="pantry-subtitle">Manage the ingredients you have at home</p>
      </header>

      {error && <div className="alert alert-error">{error}</div>}
      {success && <div className="alert alert-success">{success}</div>}

      <div className="pantry-actions">
        <div className="pantry-search">
          <input
            type="text"
            placeholder="Search ingredients..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="search-input"
          />
        </div>
        <button
          type="button"
          className="btn btn-primary"
          onClick={() => setShowAddForm(!showAddForm)}
          disabled={loading || availableIngredients.length === 0}
        >
          {showAddForm ? 'Cancel' : '+ Add Ingredient'}
        </button>
      </div>

      {showAddForm && availableIngredients.length > 0 && (
        <div className="add-ingredient-form">
          <h3>Add to Pantry</h3>
          <form onSubmit={handleAddIngredient}>
            <div className="form-group">
              <label htmlFor="ingredient">Ingredient *</label>
              <select
                id="ingredient"
                value={selectedIngredient || ''}
                onChange={(e) => setSelectedIngredient(parseInt(e.target.value))}
                className="form-control"
                required
              >
                <option value="">Select an ingredient...</option>
                {availableIngredients.map(ing => (
                  <option key={ing.id} value={ing.id}>
                    {ing.name} ({ing.category})
                  </option>
                ))}
              </select>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label htmlFor="quantity">Quantity *</label>
                <input
                  id="quantity"
                  type="number"
                  step="0.01"
                  min="0"
                  value={quantity}
                  onChange={(e) => setQuantity(e.target.value)}
                  placeholder="Enter quantity"
                  className="form-control"
                  required
                />
                {selectedIngredient && (
                  <span className="unit-hint">
                    {allIngredients.find(i => i.id === selectedIngredient)?.unitOfMeasure}
                  </span>
                )}
              </div>

              <div className="form-group">
                <label htmlFor="notes">Notes</label>
                <input
                  id="notes"
                  type="text"
                  value={notes}
                  onChange={(e) => setNotes(e.target.value)}
                  placeholder="e.g., expiring soon"
                  className="form-control"
                />
              </div>
            </div>

            <div className="form-actions">
              <button type="submit" className="btn btn-primary" disabled={loading}>
                {loading ? 'Adding...' : 'Add to Pantry'}
              </button>
            </div>
          </form>
        </div>
      )}

      {loading && <div className="loading">Loading...</div>}

      {!loading && pantryItems.length === 0 ? (
        <div className="empty-state">
          <div className="empty-icon">🥘</div>
          <h3>Your pantry is empty</h3>
          <p>Start by adding the ingredients you have at home.</p>
        </div>
      ) : (
        <div className="pantry-items">
          {categories.length > 0 ? (
            categories.map(category => (
              <div key={category} className="pantry-category">
                <h3 className="category-title">{category}</h3>
                <div className="items-grid">
                  {groupedItems[category].map(item => (
                    <div key={item.id} className="pantry-item">
                      {editingId === item.id ? (
                        <div className="item-edit-form">
                          <div className="form-group">
                            <label>Quantity</label>
                            <input
                              type="number"
                              step="0.01"
                              min="0"
                              value={editQuantity}
                              onChange={(e) => setEditQuantity(e.target.value)}
                              className="form-control"
                            />
                            <span className="unit">{item.unitOfMeasure}</span>
                          </div>
                          <div className="form-group">
                            <label>Notes</label>
                            <input
                              type="text"
                              value={editNotes}
                              onChange={(e) => setEditNotes(e.target.value)}
                              className="form-control"
                            />
                          </div>
                          <div className="item-actions">
                            <button
                              type="button"
                              className="btn btn-sm btn-primary"
                              onClick={() => handleUpdateItem(item.id)}
                              disabled={loading}
                            >
                              Save
                            </button>
                            <button
                              type="button"
                              className="btn btn-sm btn-ghost"
                              onClick={() => setEditingId(null)}
                            >
                              Cancel
                            </button>
                          </div>
                        </div>
                      ) : (
                        <>
                          <div className="item-header">
                            <h4>{item.ingredientName}</h4>
                            <span className="badge category-badge">{item.category}</span>
                          </div>
                          <div className="item-details">
                            <div className="quantity-display">
                              <span className="quantity-value">{item.quantity}</span>
                              <span className="quantity-unit">{item.unitOfMeasure}</span>
                            </div>
                            {item.notes && <p className="item-notes">{item.notes}</p>}
                          </div>
                          <div className="item-actions">
                            <button
                              type="button"
                              className="btn btn-sm btn-ghost"
                              onClick={() => handleStartEdit(item)}
                            >
                              Edit
                            </button>
                            <button
                              type="button"
                              className="btn btn-sm btn-danger"
                              onClick={() => handleRemoveItem(item.id)}
                              disabled={loading}
                            >
                              Remove
                            </button>
                          </div>
                        </>
                      )}
                    </div>
                  ))}
                </div>
              </div>
            ))
          ) : (
            <div className="empty-search">
              <p>No ingredients match your search.</p>
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default PantryPage;
