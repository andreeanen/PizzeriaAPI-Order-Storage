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

        [HttpGet]
        public  IActionResult GetIngredients()
        {
            var ingredients =  _context.Ingredients.ToList();
            return Ok(ingredients);
        }

        [HttpGet("{ingredientName}")]
        public int GetIngredienQuantityByName(string ingredientName)
        {
            var ingredient = _context.Ingredients.Where(i => i.IngredientName.ToLower() == ingredientName.ToLower()).FirstOrDefault();
            if (ingredient == null)
            {
                return 0;
            }
            int ingredientQuantity = ingredient.Quantity;

            return ingredientQuantity;
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

        [HttpPut("{id}/{quantity}")]
        public IActionResult UpdateIngredientQuantityById(int id, int quantity)
        {
            var ingredientToUpdate = _context.Ingredients.Where(i=>i.Id == id).FirstOrDefault();
            if(ingredientToUpdate == null)
            {
                return NotFound("There is no ingredient with the specified id in the database.");
            }
            if(quantity<0)
            {
                return BadRequest("The value for quantity has to be a positive number.");
            }

            ingredientToUpdate.Quantity = quantity;
            _context.Ingredients.Update(ingredientToUpdate);
            _context.SaveChanges();

            return Ok(ingredientToUpdate);
        }

        [HttpPost]
        public IActionResult CheckAvailabilityInStorage([FromBody] List<IngredientOrder> ingredientsFromOrder)
        {
            List<IngredientItem> ingredientsInStorage = new List<IngredientItem>();
            foreach (var ingredient in ingredientsFromOrder)
            {
                var ingredientFromStorage = _context.Ingredients.Where(i=>i.IngredientName == ingredient.IngredientName).FirstOrDefault();

                if(ingredientFromStorage == null)
                {
                    return NotFound();
                }

                if(ingredientFromStorage.Quantity < ingredient.Quantity)
                {
                    return BadRequest($"The number of {ingredientFromStorage.IngredientName} that you are trying to order exceeds the quantity in the storage.");
                }
                               
                ingredientFromStorage.Quantity -= ingredient.Quantity;
                ingredientsInStorage.Add(ingredientFromStorage);
                _context.Ingredients.Update(ingredientFromStorage);
                _context.SaveChanges();
            }

            return Ok(ingredientsInStorage);
        }

    }
}
