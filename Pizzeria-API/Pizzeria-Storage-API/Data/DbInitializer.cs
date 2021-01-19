using Pizzeria_Storage_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria_Storage_API.Data
{
    public class DbInitializer
    {
        public static void Initialize(IngredientContext ingredientContext)
        {
            ingredientContext.Database.EnsureCreated();

            if (ingredientContext.Ingredients.Any())
            {
                return;
            }

            int initialQuantityInStorage = 5;

            Dictionary<string, int> ingredientsNamePrice = new Dictionary<string, int>()
            {
                {"Ham",10 },
                {"Pineapple",10 },
                {"Mushrooms",10 },
                {"Onion",10 },
                {"Kebab sauce",10 },
                {"Shrimps",15 },
                {"Mussels",15 },
                {"Artichoke",15 },
                {"Kebab",20 },
                {"Coriander",20 }
            };

            foreach (var pair in ingredientsNamePrice)
            {
                var newIngredient = new IngredientItem
                {
                    IngredientName = pair.Key,
                    Price = pair.Value,
                    Quantity = initialQuantityInStorage
                };
                ingredientContext.Ingredients.Add(newIngredient);
                ingredientContext.SaveChanges();
            }
        }  
    }
}
