﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria_Storage_API.Models
{
    public class IngredientItem
    {
         public int Id { get; set; }
        public string IngredientName { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

    }
}
