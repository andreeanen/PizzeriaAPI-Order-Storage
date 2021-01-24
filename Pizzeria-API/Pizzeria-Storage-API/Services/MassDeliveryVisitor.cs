using Pizzeria_Storage_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria_Storage_API.Services
{
    public class MassDeliveryVisitor
    {
        public List<IngredientItem> IncreaseQuantityOfIngredients(List<IngredientItem> ingredients)
        {
            int quantityDelivered = 10;
            foreach (var ingredient in ingredients)
            {
                ingredient.Quantity = GetIngredientQuantity(ingredient) + quantityDelivered;
            }
          
            return ingredients;
        }

        private int GetIngredientQuantity(IngredientItem ingredient)
        {
            return ingredient.Quantity;
        }
    }
}
