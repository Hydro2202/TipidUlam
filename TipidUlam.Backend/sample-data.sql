-- TipidUlam - Sample Data Initialization
-- This file populates the database with sample recipes and ingredients for testing

-- Insert sample ingredients
INSERT INTO ingredients (name, unit_of_measure, price_per_unit, description, category, created_at, updated_at) VALUES
('Chicken', 'kg', 150, 'Fresh chicken meat', 'protein', NOW(), NOW()),
('Pork', 'kg', 200, 'Fresh pork meat', 'protein', NOW(), NOW()),
('Beef', 'kg', 250, 'Fresh beef meat', 'protein', NOW(), NOW()),
('White Rice', 'kg', 50, 'Long-grain white rice', 'grain', NOW(), NOW()),
('Brown Rice', 'kg', 60, 'Whole grain brown rice', 'grain', NOW(), NOW()),
('Egg', 'piece', 10, 'Chicken egg', 'protein', NOW(), NOW()),
('Soy Sauce', 'liter', 100, 'Salt seasoning sauce', 'condiment', NOW(), NOW()),
('Vinegar', 'liter', 50, 'Calamansi vinegar', 'condiment', NOW(), NOW()),
('Onion', 'kg', 40, 'Yellow onion', 'vegetable', NOW(), NOW()),
('Garlic', 'kg', 150, 'Fresh garlic cloves', 'vegetable', NOW(), NOW()),
('Ginger', 'kg', 100, 'Fresh ginger root', 'vegetable', NOW(), NOW()),
('Tomato', 'kg', 60, 'Ripe tomatoes', 'vegetable', NOW(), NOW()),
('Radish', 'kg', 50, 'White radish (labanos)', 'vegetable', NOW(), NOW()),
('Cabbage', 'kg', 40, 'Green cabbage', 'vegetable', NOW(), NOW()),
('Banana Blossom', 'piece', 30, 'Banana flower (puso ng saging)', 'vegetable', NOW(), NOW()),
('Anchovy', 'kg', 200, 'Dried anchovies (dilis)', 'protein', NOW(), NOW()),
('Salt', 'kg', 20, 'Table salt', 'condiment', NOW(), NOW()),
('Pepper', 'gram', 2, 'Black pepper', 'spice', NOW(), NOW()),
('Oil', 'liter', 150, 'Cooking oil', 'condiment', NOW(), NOW()),
('Lemon', 'piece', 5, 'Calamansi lemon', 'fruit', NOW(), NOW()),
('Peanut Butter', 'kg', 250, 'Creamy peanut butter', 'condiment', NOW(), NOW());

-- Insert sample recipes
INSERT INTO recipes (name, description, instructions, base_servings, cuisine_type, difficulty_level, cooking_time_minutes, created_at, updated_at) VALUES
('Chicken Adobo', 'Classic Filipino braised chicken in vinegar and soy sauce', 
 '1. Heat oil and fry chicken until golden\n2. Add garlic, onions\n3. Pour vinegar and soy sauce\n4. Simmer 30 mins until tender\n5. Serve hot over rice', 
 2, 'Filipino', 'Easy', 45, NOW(), NOW()),

('Pork Sinigang', 'Sour soup with tamarind and vegetables', 
 '1. Boil pork until tender\n2. Add ginger and onions\n3. Add radish and leafy vegetables\n4. Simmer 20 mins\n5. Serve hot', 
 4, 'Filipino', 'Medium', 60, NOW(), NOW()),

('Fried Rice', 'Simple stir-fried rice with eggs and vegetables', 
 '1. Heat oil in wok\n2. Scramble eggs\n3. Add cooked rice\n4. Stir in onions and garlic\n5. Season with soy sauce', 
 2, 'Filipino', 'Easy', 20, NOW(), NOW()),

('Beef Nilaga', 'Beef and vegetables in clear broth', 
 '1. Boil beef 1 hour\n2. Add onions, garlic\n3. Add cabbage and radish\n4. Simmer 20 mins\n5. Season with salt', 
 4, 'Filipino', 'Medium', 90, NOW(), NOW()),

('Tapa with Egg', 'Cured beef with fried egg and rice', 
 '1. Pan-fry tapa until crispy\n2. Fry eggs sunny-side up\n3. Serve with steamed rice\n4. Top with fried garlic', 
 2, 'Filipino', 'Easy', 15, NOW(), NOW()),

('Chicken Tinola', 'Comforting ginger chicken soup with vegetables', 
 '1. Sauté garlic, onion, and ginger\n2. Add chicken pieces and lightly brown\n3. Pour water and simmer until chicken is tender\n4. Add cabbage and lemon\n5. Serve warm with rice', 
 4, 'Filipino', 'Easy', 50, NOW(), NOW()),

('Bistek Tagalog', 'Filipino-style beef steak with onions', 
 '1. Marinate beef in soy sauce, lemon, and garlic\n2. Fry onions until soft\n3. Pan-sear beef until cooked\n4. Add sauce and simmer briefly\n5. Serve with rice', 
 2, 'Filipino', 'Medium', 40, NOW(), NOW()),

('Pork Kare-Kare', 'Peanut stew with pork and vegetables', 
 '1. Simmer pork until tender\n2. Sauté garlic and onion\n3. Add peanut butter and water to make sauce\n4. Add cabbage and simmer until soft\n5. Serve with rice and bagoong if available', 
 4, 'Filipino', 'Medium', 80, NOW(), NOW()),

('Chicken Afritada', 'Tomato-based chicken stew with vegetables', 
 '1. Brown chicken in oil\n2. Sauté garlic and onion\n3. Add tomato and simmer\n4. Add cabbage and cook until tender\n5. Season and serve with rice', 
 4, 'Filipino', 'Easy', 55, NOW(), NOW()),

('Lumpiang Shanghai', 'Crispy Filipino pork spring rolls', 
 '1. Mix ground pork with garlic, onion, egg, and seasonings\n2. Roll filling in wrappers\n3. Fry until golden brown\n4. Serve with dipping sauce', 
 4, 'Filipino', 'Medium', 40, NOW(), NOW()),

('Pinakbet', 'Mixed vegetable stew with pork and savory seasonings', 
 '1. Sauté garlic and onion\n2. Add pork and cook until lightly browned\n3. Add tomato and cabbage\n4. Simmer until vegetables are tender\n5. Season and serve hot', 
 4, 'Filipino', 'Medium', 45, NOW(), NOW()),

('Beef Caldereta', 'Rich Filipino beef stew in tomato sauce', 
 '1. Brown beef in oil\n2. Sauté garlic and onion\n3. Add tomato and simmer until beef is tender\n4. Add cabbage and seasonings\n5. Serve with rice', 
 4, 'Filipino', 'Medium', 90, NOW(), NOW()),

('Chicken Inasal', 'Grilled chicken marinated in tangy sauce', 
 '1. Marinate chicken in soy sauce, vinegar, garlic, and lemon\n2. Grill or pan-sear until cooked\n3. Baste with sauce while cooking\n4. Serve with rice', 
 2, 'Filipino', 'Medium', 35, NOW(), NOW()),

('Pork Torta', 'Savory Filipino pork omelet', 
 '1. Cook ground pork with garlic, onion, and tomato\n2. Beat eggs and combine with cooked pork\n3. Fry until set and golden\n4. Serve warm with rice', 
 2, 'Filipino', 'Easy', 30, NOW(), NOW());

-- Link recipes to ingredients (RecipeIngredients)
-- Chicken Adobo (2 servings)
INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity_per_serving, created_at, updated_at) VALUES
((SELECT id FROM recipes WHERE name = 'Chicken Adobo'), (SELECT id FROM ingredients WHERE name = 'Chicken'), 0.25, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Adobo'), (SELECT id FROM ingredients WHERE name = 'Soy Sauce'), 0.025, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Adobo'), (SELECT id FROM ingredients WHERE name = 'Vinegar'), 0.025, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Adobo'), (SELECT id FROM ingredients WHERE name = 'Garlic'), 0.02, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Adobo'), (SELECT id FROM ingredients WHERE name = 'Onion'), 0.1, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Adobo'), (SELECT id FROM ingredients WHERE name = 'Oil'), 0.02, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Adobo'), (SELECT id FROM ingredients WHERE name = 'Salt'), 0.005, NOW(), NOW());

-- Pork Sinigang (4 servings)
INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity_per_serving, created_at, updated_at) VALUES
((SELECT id FROM recipes WHERE name = 'Pork Sinigang'), (SELECT id FROM ingredients WHERE name = 'Pork'), 0.1, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pork Sinigang'), (SELECT id FROM ingredients WHERE name = 'Radish'), 0.1, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pork Sinigang'), (SELECT id FROM ingredients WHERE name = 'Onion'), 0.05, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pork Sinigang'), (SELECT id FROM ingredients WHERE name = 'Ginger'), 0.02, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pork Sinigang'), (SELECT id FROM ingredients WHERE name = 'Salt'), 0.005, NOW(), NOW());

-- Fried Rice (2 servings)
INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity_per_serving, created_at, updated_at) VALUES
((SELECT id FROM recipes WHERE name = 'Fried Rice'), (SELECT id FROM ingredients WHERE name = 'White Rice'), 0.2, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Fried Rice'), (SELECT id FROM ingredients WHERE name = 'Egg'), 0.5, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Fried Rice'), (SELECT id FROM ingredients WHERE name = 'Garlic'), 0.01, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Fried Rice'), (SELECT id FROM ingredients WHERE name = 'Onion'), 0.05, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Fried Rice'), (SELECT id FROM ingredients WHERE name = 'Soy Sauce'), 0.01, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Fried Rice'), (SELECT id FROM ingredients WHERE name = 'Oil'), 0.02, NOW(), NOW());

-- Beef Nilaga (4 servings)
INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity_per_serving, created_at, updated_at) VALUES
((SELECT id FROM recipes WHERE name = 'Beef Nilaga'), (SELECT id FROM ingredients WHERE name = 'Beef'), 0.1, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Beef Nilaga'), (SELECT id FROM ingredients WHERE name = 'Cabbage'), 0.1, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Beef Nilaga'), (SELECT id FROM ingredients WHERE name = 'Radish'), 0.15, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Beef Nilaga'), (SELECT id FROM ingredients WHERE name = 'Onion'), 0.1, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Beef Nilaga'), (SELECT id FROM ingredients WHERE name = 'Garlic'), 0.02, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Beef Nilaga'), (SELECT id FROM ingredients WHERE name = 'Salt'), 0.01, NOW(), NOW());

-- Tapa with Egg (2 servings)
INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity_per_serving, created_at, updated_at) VALUES
((SELECT id FROM recipes WHERE name = 'Tapa with Egg'), (SELECT id FROM ingredients WHERE name = 'Beef'), 0.05, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Tapa with Egg'), (SELECT id FROM ingredients WHERE name = 'Egg'), 0.5, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Tapa with Egg'), (SELECT id FROM ingredients WHERE name = 'Garlic'), 0.01, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Tapa with Egg'), (SELECT id FROM ingredients WHERE name = 'Oil'), 0.02, NOW(), NOW());

-- Chicken Tinola (4 servings)
INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity_per_serving, created_at, updated_at) VALUES
((SELECT id FROM recipes WHERE name = 'Chicken Tinola'), (SELECT id FROM ingredients WHERE name = 'Chicken'), 0.15, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Tinola'), (SELECT id FROM ingredients WHERE name = 'Ginger'), 0.02, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Tinola'), (SELECT id FROM ingredients WHERE name = 'Onion'), 0.05, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Tinola'), (SELECT id FROM ingredients WHERE name = 'Garlic'), 0.01, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Tinola'), (SELECT id FROM ingredients WHERE name = 'Cabbage'), 0.1, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Tinola'), (SELECT id FROM ingredients WHERE name = 'Lemon'), 0.5, NOW(), NOW());

-- Bistek Tagalog (2 servings)
INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity_per_serving, created_at, updated_at) VALUES
((SELECT id FROM recipes WHERE name = 'Bistek Tagalog'), (SELECT id FROM ingredients WHERE name = 'Beef'), 0.15, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Bistek Tagalog'), (SELECT id FROM ingredients WHERE name = 'Onion'), 0.1, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Bistek Tagalog'), (SELECT id FROM ingredients WHERE name = 'Soy Sauce'), 0.02, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Bistek Tagalog'), (SELECT id FROM ingredients WHERE name = 'Lemon'), 0.5, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Bistek Tagalog'), (SELECT id FROM ingredients WHERE name = 'Garlic'), 0.01, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Bistek Tagalog'), (SELECT id FROM ingredients WHERE name = 'Oil'), 0.02, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Bistek Tagalog'), (SELECT id FROM ingredients WHERE name = 'Pepper'), 0.005, NOW(), NOW());

-- Pork Kare-Kare (4 servings)
INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity_per_serving, created_at, updated_at) VALUES
((SELECT id FROM recipes WHERE name = 'Pork Kare-Kare'), (SELECT id FROM ingredients WHERE name = 'Pork'), 0.12, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pork Kare-Kare'), (SELECT id FROM ingredients WHERE name = 'Peanut Butter'), 0.05, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pork Kare-Kare'), (SELECT id FROM ingredients WHERE name = 'Onion'), 0.05, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pork Kare-Kare'), (SELECT id FROM ingredients WHERE name = 'Garlic'), 0.01, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pork Kare-Kare'), (SELECT id FROM ingredients WHERE name = 'Cabbage'), 0.1, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pork Kare-Kare'), (SELECT id FROM ingredients WHERE name = 'Oil'), 0.02, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pork Kare-Kare'), (SELECT id FROM ingredients WHERE name = 'Salt'), 0.005, NOW(), NOW());

-- Chicken Afritada (4 servings)
INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity_per_serving, created_at, updated_at) VALUES
((SELECT id FROM recipes WHERE name = 'Chicken Afritada'), (SELECT id FROM ingredients WHERE name = 'Chicken'), 0.15, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Afritada'), (SELECT id FROM ingredients WHERE name = 'Tomato'), 0.12, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Afritada'), (SELECT id FROM ingredients WHERE name = 'Onion'), 0.05, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Afritada'), (SELECT id FROM ingredients WHERE name = 'Garlic'), 0.01, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Afritada'), (SELECT id FROM ingredients WHERE name = 'Cabbage'), 0.1, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Afritada'), (SELECT id FROM ingredients WHERE name = 'Oil'), 0.02, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Afritada'), (SELECT id FROM ingredients WHERE name = 'Salt'), 0.005, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Afritada'), (SELECT id FROM ingredients WHERE name = 'Pepper'), 0.003, NOW(), NOW());

-- Lumpiang Shanghai (4 servings)
INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity_per_serving, created_at, updated_at) VALUES
((SELECT id FROM recipes WHERE name = 'Lumpiang Shanghai'), (SELECT id FROM ingredients WHERE name = 'Pork'), 0.08, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Lumpiang Shanghai'), (SELECT id FROM ingredients WHERE name = 'Egg'), 0.5, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Lumpiang Shanghai'), (SELECT id FROM ingredients WHERE name = 'Garlic'), 0.01, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Lumpiang Shanghai'), (SELECT id FROM ingredients WHERE name = 'Onion'), 0.05, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Lumpiang Shanghai'), (SELECT id FROM ingredients WHERE name = 'Soy Sauce'), 0.01, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Lumpiang Shanghai'), (SELECT id FROM ingredients WHERE name = 'Oil'), 0.03, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Lumpiang Shanghai'), (SELECT id FROM ingredients WHERE name = 'Pepper'), 0.003, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Lumpiang Shanghai'), (SELECT id FROM ingredients WHERE name = 'Salt'), 0.005, NOW(), NOW());

-- Pinakbet (4 servings)
INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity_per_serving, created_at, updated_at) VALUES
((SELECT id FROM recipes WHERE name = 'Pinakbet'), (SELECT id FROM ingredients WHERE name = 'Pork'), 0.08, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pinakbet'), (SELECT id FROM ingredients WHERE name = 'Tomato'), 0.1, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pinakbet'), (SELECT id FROM ingredients WHERE name = 'Onion'), 0.05, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pinakbet'), (SELECT id FROM ingredients WHERE name = 'Garlic'), 0.01, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pinakbet'), (SELECT id FROM ingredients WHERE name = 'Cabbage'), 0.12, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pinakbet'), (SELECT id FROM ingredients WHERE name = 'Oil'), 0.02, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pinakbet'), (SELECT id FROM ingredients WHERE name = 'Salt'), 0.005, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pinakbet'), (SELECT id FROM ingredients WHERE name = 'Pepper'), 0.003, NOW(), NOW());

-- Beef Caldereta (4 servings)
INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity_per_serving, created_at, updated_at) VALUES
((SELECT id FROM recipes WHERE name = 'Beef Caldereta'), (SELECT id FROM ingredients WHERE name = 'Beef'), 0.15, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Beef Caldereta'), (SELECT id FROM ingredients WHERE name = 'Tomato'), 0.1, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Beef Caldereta'), (SELECT id FROM ingredients WHERE name = 'Onion'), 0.05, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Beef Caldereta'), (SELECT id FROM ingredients WHERE name = 'Garlic'), 0.01, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Beef Caldereta'), (SELECT id FROM ingredients WHERE name = 'Cabbage'), 0.1, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Beef Caldereta'), (SELECT id FROM ingredients WHERE name = 'Oil'), 0.02, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Beef Caldereta'), (SELECT id FROM ingredients WHERE name = 'Salt'), 0.005, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Beef Caldereta'), (SELECT id FROM ingredients WHERE name = 'Pepper'), 0.003, NOW(), NOW());

-- Chicken Inasal (2 servings)
INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity_per_serving, created_at, updated_at) VALUES
((SELECT id FROM recipes WHERE name = 'Chicken Inasal'), (SELECT id FROM ingredients WHERE name = 'Chicken'), 0.2, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Inasal'), (SELECT id FROM ingredients WHERE name = 'Soy Sauce'), 0.02, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Inasal'), (SELECT id FROM ingredients WHERE name = 'Vinegar'), 0.02, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Inasal'), (SELECT id FROM ingredients WHERE name = 'Garlic'), 0.01, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Inasal'), (SELECT id FROM ingredients WHERE name = 'Lemon'), 0.5, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Inasal'), (SELECT id FROM ingredients WHERE name = 'Oil'), 0.02, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Chicken Inasal'), (SELECT id FROM ingredients WHERE name = 'Salt'), 0.005, NOW(), NOW());

-- Pork Torta (2 servings)
INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity_per_serving, created_at, updated_at) VALUES
((SELECT id FROM recipes WHERE name = 'Pork Torta'), (SELECT id FROM ingredients WHERE name = 'Pork'), 0.08, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pork Torta'), (SELECT id FROM ingredients WHERE name = 'Egg'), 0.5, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pork Torta'), (SELECT id FROM ingredients WHERE name = 'Onion'), 0.05, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pork Torta'), (SELECT id FROM ingredients WHERE name = 'Tomato'), 0.08, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pork Torta'), (SELECT id FROM ingredients WHERE name = 'Garlic'), 0.01, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pork Torta'), (SELECT id FROM ingredients WHERE name = 'Oil'), 0.02, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pork Torta'), (SELECT id FROM ingredients WHERE name = 'Salt'), 0.005, NOW(), NOW()),
((SELECT id FROM recipes WHERE name = 'Pork Torta'), (SELECT id FROM ingredients WHERE name = 'Pepper'), 0.003, NOW(), NOW());

-- Create test admin user (password should be hashed in production)
INSERT INTO users (username, email, password_hash, role, created_at, updated_at) VALUES
('admin', 'admin@tipidulam.com', '$2b$10$3F6q.XZ8q1Z8q1Z8q1Z8q1Z8q1Z8q1Z8q1Z8q1Z8q1Z8q1Z8q1Z8q', 'admin', NOW(), NOW());

-- Verify data
SELECT 'Recipes count: ' || COUNT(*) FROM recipes;
SELECT 'Ingredients count: ' || COUNT(*) FROM ingredients;
SELECT 'RecipeIngredients count: ' || COUNT(*) FROM recipe_ingredients;
