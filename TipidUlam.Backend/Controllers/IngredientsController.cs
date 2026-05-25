using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TipidUlam.Backend.DTOs;
using TipidUlam.Backend.Services;

namespace TipidUlam.Backend.Controllers
{
    /// <summary>
    /// Admin-only API endpoint for managing ingredient prices.
    /// Requires JWT authentication with 'admin' role.
    /// Full CRUD operations for ingredient prices.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]  // JWT Protected - Admin only
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientsController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService ?? throw new ArgumentNullException(nameof(ingredientService));
        }

        /// <summary>
        /// Get all ingredients.
        /// GET /api/ingredients
        /// </summary>
        [HttpGet]
        [AllowAnonymous]  // Public endpoint
        public async Task<ActionResult<IEnumerable<IngredientDto>>> GetAllIngredients()
        {
            try
            {
                var ingredients = await _ingredientService.GetAllAsync();
                return Ok(ingredients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get ingredients by category.
        /// GET /api/ingredients/category/{category}
        /// </summary>
        [HttpGet("category/{category}")]
        [AllowAnonymous]  // Public endpoint
        public async Task<ActionResult<IEnumerable<IngredientDto>>> GetIngredientsByCategory(string category)
        {
            try
            {
                var ingredients = await _ingredientService.GetByCategoryAsync(category);
                return Ok(ingredients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get a single ingredient by ID.
        /// GET /api/ingredients/{id}
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]  // Public endpoint
        public async Task<ActionResult<IngredientDto>> GetIngredientById(int id)
        {
            try
            {
                var ingredient = await _ingredientService.GetByIdAsync(id);
                if (ingredient == null)
                    return NotFound(new { message = $"Ingredient with ID {id} not found." });

                return Ok(ingredient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new ingredient.
        /// POST /api/ingredients
        /// Admin only - Requires JWT token with 'admin' role
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IngredientDto>> CreateIngredient(CreateUpdateIngredientDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var ingredient = await _ingredientService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetIngredientById), new { id = ingredient.Id }, ingredient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing ingredient (including price changes).
        /// PUT /api/ingredients/{id}
        /// Admin only - Requires JWT token with 'admin' role
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IngredientDto>> UpdateIngredient(int id, CreateUpdateIngredientDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var ingredient = await _ingredientService.UpdateAsync(id, dto);
                if (ingredient == null)
                    return NotFound(new { message = $"Ingredient with ID {id} not found." });

                return Ok(ingredient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete an ingredient.
        /// DELETE /api/ingredients/{id}
        /// Admin only - Requires JWT token with 'admin' role
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteIngredient(int id)
        {
            try
            {
                var deleted = await _ingredientService.DeleteAsync(id);
                if (!deleted)
                    return NotFound(new { message = $"Ingredient with ID {id} not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}
