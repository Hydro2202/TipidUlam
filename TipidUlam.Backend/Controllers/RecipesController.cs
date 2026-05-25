using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TipidUlam.Backend.DTOs;
using TipidUlam.Backend.Services;
using TipidUlam.Backend.Models;
using TipidUlam.Backend.Repositories;

namespace TipidUlam.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly IBudgetMatchingService _budgetMatchingService;
        private readonly IRecipeRepository _recipeRepository;

        public RecipesController(
            IBudgetMatchingService budgetMatchingService,
            IRecipeRepository recipeRepository)
        {
            _budgetMatchingService = budgetMatchingService;
            _recipeRepository = recipeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetAllRecipes()
        {
            try
            {
                var recipes = await _recipeRepository.GetAllAsync();
                return Ok(recipes ?? Enumerable.Empty<Recipe>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Search distinct meals within any budget (GET). Costs computed from live DB prices.
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<BudgetSearchResponseDto>> SearchRecipesByBudget(
            [FromQuery] decimal maxBudget,
            [FromQuery] int familySize,
            [FromQuery] string? pantryIds = null)
        {
            try
            {
                var request = new BudgetSearchRequestDto
                {
                    MaxBudget = maxBudget,
                    FamilySize = familySize,
                    PantryIngredientIds = ParsePantryIds(pantryIds),
                };

                var response = await _budgetMatchingService.SearchDistinctMealsAsync(request);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Search distinct meals within any budget (POST). Same logic as GET; use for larger pantry lists.
        /// </summary>
        [HttpPost("search")]
        public async Task<ActionResult<BudgetSearchResponseDto>> SearchRecipesByBudgetPost(
            [FromBody] BudgetSearchRequestDto request)
        {
            try
            {
                var response = await _budgetMatchingService.SearchDistinctMealsAsync(request);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Real-time recalculation when the user adjusts one ingredient weight (e.g. 1.5 kg pork);
        /// all other ingredients scale proportionally and costs refresh from current prices.
        /// </summary>
        [HttpPost("{id:int}/calculate")]
        public async Task<ActionResult<RecipeCostCalculateResponseDto>> CalculateRecipeCost(
            int id,
            [FromBody] RecipeCostCalculateRequestDto request)
        {
            try
            {
                var response = await _budgetMatchingService.CalculateRecipeCostAsync(id, request);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Models.Recipe>> GetRecipeById(int id)
        {
            try
            {
                var recipe = await _recipeRepository.GetByIdAsync(id);
                if (recipe == null)
                    return NotFound(new { message = $"Recipe with ID {id} not found." });

                return Ok(recipe);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        private static List<int> ParsePantryIds(string? pantryIds)
        {
            var result = new List<int>();
            if (string.IsNullOrWhiteSpace(pantryIds))
                return result;

            foreach (var part in pantryIds.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                if (int.TryParse(part.Trim(), out var id))
                    result.Add(id);
            }

            return result;
        }
    }
}
