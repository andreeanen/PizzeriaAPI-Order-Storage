using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizzeria_Storage_API.Data;
using Pizzeria_Storage_API.Models;
using Pizzeria_Storage_API.Services;

namespace Pizzeria_Storage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientItemsController : ControllerBase
    {
        private readonly IngredientContext _context;

        public IngredientItemsController(IngredientContext context)
        {
            _context = context;
        }

        // GET: api/IngredientItems
        [HttpGet]
        public  IActionResult GetIngredients()
        {
            var ingredients =  _context.Ingredients.ToList();

            return Ok(ingredients);
        }

        [HttpPut("massdelivery")]
        public IActionResult MassDelivery()
        {
            var ingredientsBeforeDelivery = _context.Ingredients.ToList();
            var massDeliveryVisitor = new MassDeliveryVisitor();
            var ingredientsAfterDelivery = massDeliveryVisitor.IncreaseQuantityOfIngredients(ingredientsBeforeDelivery);

            _context.Ingredients.UpdateRange(ingredientsAfterDelivery);
            _context.SaveChanges();

            return Ok(ingredientsAfterDelivery);
        }

        //// GET: api/IngredientItems/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<IngredientItem>> GetIngredientItem(int id)
        //{
        //    var ingredientItem = await _context.Ingredients.FindAsync(id);

        //    if (ingredientItem == null)
        //    {
        //        return NotFound();
        //    }

        //    return ingredientItem;
        //}

        //// PUT: api/IngredientItems/5

        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutIngredientItem(int id, IngredientItem ingredientItem)
        //{
        //    if (id != ingredientItem.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(ingredientItem).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!IngredientItemExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/IngredientItems

        //[HttpPost]
        //public async Task<ActionResult<IngredientItem>> PostIngredientItem(IngredientItem ingredientItem)
        //{
        //    _context.Ingredients.Add(ingredientItem);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetIngredientItem", new { id = ingredientItem.Id }, ingredientItem);
        //}

        //// DELETE: api/IngredientItems/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteIngredientItem(int id)
        //{
        //    var ingredientItem = await _context.Ingredients.FindAsync(id);
        //    if (ingredientItem == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Ingredients.Remove(ingredientItem);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool IngredientItemExists(int id)
        //{
        //    return _context.Ingredients.Any(e => e.Id == id);
        //}
    }
}
