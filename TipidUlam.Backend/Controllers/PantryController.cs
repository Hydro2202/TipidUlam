using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TipidUlam.Backend.DTOs;
using TipidUlam.Backend.Services;

namespace TipidUlam.Backend.Controllers
{
    /// <summary>
    /// Pantry management API endpoints.
    /// Allows authenticated users to manage their pantry items (ingredients they own).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // JWT Protected - Authenticated users only
    public class PantryController : ControllerBase
    {
        private readonly IUserPantryService _pantryService;

        public PantryController(IUserPantryService pantryService)
        {
            _pantryService = pantryService ?? throw new ArgumentNullException(nameof(pantryService));
        }

        /// <summary>
        /// Get the current user's pantry.
        /// GET /api/pantry
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPantryItemDto>>> GetPantry()
        {
            try
            {
                var userId = GetCurrentUserId();
                var pantryItems = await _pantryService.GetUserPantryAsync(userId);
                return Ok(pantryItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Add ingredient to pantry.
        /// POST /api/pantry
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UserPantryItemDto>> AddIngredient(AddToPantryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = GetCurrentUserId();
                var pantryItem = await _pantryService.AddToPantryAsync(userId, dto);
                return CreatedAtAction(nameof(GetPantry), pantryItem);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Update a pantry item (quantity and notes).
        /// PUT /api/pantry/{id}
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserPantryItemDto>> UpdatePantryItem(int id, UpdatePantryItemDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = GetCurrentUserId();
                var pantryItem = await _pantryService.UpdatePantryItemAsync(userId, id, dto);
                return Ok(pantryItem);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Remove a pantry item.
        /// DELETE /api/pantry/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromPantry(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var success = await _pantryService.RemoveFromPantryAsync(userId, id);

                if (!success)
                    return NotFound(new { message = $"Pantry item with ID {id} not found." });

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Remove ingredient from pantry by ingredient ID.
        /// DELETE /api/pantry/ingredient/{ingredientId}
        /// </summary>
        [HttpDelete("ingredient/{ingredientId}")]
        public async Task<IActionResult> RemoveByIngredient(int ingredientId)
        {
            try
            {
                var userId = GetCurrentUserId();
                var success = await _pantryService.RemoveByIngredientAsync(userId, ingredientId);

                if (!success)
                    return NotFound(new { message = $"Ingredient with ID {ingredientId} not found in pantry." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                              ?? User.FindFirst(JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                throw new UnauthorizedAccessException("User ID not found in token.");

            return userId;
        }
    }
}
