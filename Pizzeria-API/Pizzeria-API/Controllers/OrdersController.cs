﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pizzeria_API.Data;
using Pizzeria_API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Pizzeria_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        public Orders Orders { get; set; }
        public Menu Menu { get; set; }

        private readonly HttpClient client = new HttpClient();

        public OrdersController()
        {
            Orders = Orders.GetOrders();
            Menu = Menu.GetMenu();
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            if (Orders == null)
            {
                return NotFound();
            }
            return Ok(Orders.Queue);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = GetOrderBy(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet("status={status}")]
        public IActionResult GetOrdersByStatus(string status)
        {
            if (Enum.TryParse(status, true, out Status orderStatus))
            {
                var orders = Orders.Queue.Where(o => o.Status == orderStatus).ToList();
                return Ok(new { count = orders.Count, orders });
            }
            return BadRequest($"Invalid status: {status}");
        }

        [HttpPost("{productName}")]
        public IActionResult CreateOrder(string productName)
        {
            return CreateOrUpdateOrder(productName);
        }

        [HttpPut("{id}/{action}={productName}")]
        public IActionResult UpdateOrder(int id, string action, string productName)
        {
            switch (action)
            {
                case "add":
                    return CreateOrUpdateOrder(productName, id);
                case "delete":
                    return DeleteProductFromOrder(productName, id);
                default:
                    return BadRequest();
            }
        }

        [HttpPut("{id}/submit")]
        public IActionResult SubmitOrder(int id)
        {
            var order = GetOrderBy(id);
            if (order is null)
            {
                return NotFound();
            }

            if (IsOrderEmpty(order))
            {
                return BadRequest("It is not posible to submit an empty order.");
            }
            if (order.Status == Status.InProgress)
            {
                if (order.Ingredients.Count>0)
                {
                    var ingredients = GetListOfIngredientsForStorage(order);
                    var json = JsonConvert.SerializeObject(ingredients);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var url = "http://pizzeria-storage-api:80/api/ingredientitems/";
                    var response =  client.PostAsync(url,data);
                    var result = response.Result;
                    if(result.IsSuccessStatusCode)
                    {
                        order.Status = Status.Submitted;
                        var calculator = new OrderSumCalculator();
                        order.Total = calculator.CalculateOrderSum(order);
                        return Ok(order);
                    }
                }
            }
            return BadRequest($"It is not possible to change the status of a {order.Status.ToString().ToLower()} order to submitted.");
        }

        public List<IngredientStorage> GetListOfIngredientsForStorage(Order order)
        {
            List<IngredientStorage> ingredients = new List<IngredientStorage>();
            var ingredientsGrouped = order.Ingredients.GroupBy(i => i.Name).ToList();
            foreach (var groupOfIngredients in ingredientsGrouped)
            {
                var ingredientForStorage =new IngredientStorage
                {
                    IngredientName = groupOfIngredients.Key,
                    Quantity = groupOfIngredients.Count()             
                };
                ingredients.Add(ingredientForStorage);
            }

            return ingredients;
        }

        [HttpPut("{id}/status={status}")]
        public IActionResult ChangeOrderStatus(int id, string status)
        {
            var order = GetOrderBy(id);
            if (order is null)
            {
                return NotFound();
            }

            if (order.Status == Status.Submitted)
            {
                if (Enum.TryParse(status, true, out Status newStatus))
                {
                    order.Status = newStatus;
                    return Ok(order);
                }
                else
                {
                    return BadRequest($"Invalid status parameter: {status}");
                }
            }

            return BadRequest($"It's not possible to change status of a {order.Status} order to {status}.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrderById(int id)
        {
            var order = GetOrderBy(id);

            Orders.Queue.Remove(order);
            if (order == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        private Order GetOrderBy(int? id)
        {
            return Orders.Queue.Find(o => o.Id == id);
        }

        public IActionResult CreateOrUpdateOrder(string productName, int? id = null)
        {
            var order = id is null ? new Order() : GetOrderBy(id);
            if (order == null)
            {
                return NotFound();
            }
            if (order.Status != Status.InProgress)
            {
                return BadRequest($"It is not possible to change a {order.Status.ToString().ToLower()} order.");
            }

            var pizza = Menu.Pizzas.Where(p => p.Name.ToLower() == productName.ToLower()).FirstOrDefault();
            var soda = Menu.Sodas.Where(s => s.Name.ToLower() == productName.ToLower()).FirstOrDefault();
            var ingredient = Menu.Ingredients.Where(i => i.Name.ToLower() == productName.ToLower()).FirstOrDefault();

            if (pizza == null && soda == null && ingredient == null)
            {
                return BadRequest("The product you are trying to order is not on the menu");
            }
            else
            {
                if (pizza != null)
                {
                    order.Pizzas.Add(pizza);
                }
                if (soda != null)
                {
                    order.Sodas.Add(soda);
                }
                if (ingredient != null)
                {
                    if (order.Pizzas.Count == 0)
                    {
                        return BadRequest("You can not add the ingredient because you do not have a pizza on your order");
                    }
                    order.Ingredients.Add(ingredient);
                }

                if(id is null)
                {
                    Orders.Queue.Add(order);
                }
                return Ok(order);
            }
        }

        public IActionResult DeleteProductFromOrder(string productName, int? id = null)
        {
            var order = GetOrderBy(id);
            if (order == null)
            {
                return NotFound("No order found");
            }

            if(order.Status!= Status.InProgress)
            {
                return BadRequest("Your order is not in progress so you can not delete products from it.");
            }

            var pizzaToRemove = order.Pizzas.FirstOrDefault(p => p.Name.ToLower() == productName.ToLower());
            if (pizzaToRemove != null)
            {
                order.Pizzas.Remove(pizzaToRemove);
                if (order.Pizzas.Count == 0)
                {
                    order.Ingredients.Clear();
                }
            }

            var sodaToRemove = order.Sodas.FirstOrDefault(s => s.Name.ToLower() == productName.ToLower());
            if (sodaToRemove != null)
            {
                order.Sodas.Remove(sodaToRemove);
            }

            var ingredientToToRemove = order.Ingredients.FirstOrDefault(i => i.Name.ToLower() == productName.ToLower());
            if (ingredientToToRemove != null)
            {
                order.Ingredients.Remove(ingredientToToRemove);
            }

            if (pizzaToRemove is null && sodaToRemove is null && ingredientToToRemove is null)
            {
                return BadRequest("The product cannot be found on your order");
            }

            return Ok(order);
        }

        private bool IsOrderEmpty(Order order)
        {
            return order.Pizzas.Count == 0 &&
                   order.Sodas.Count == 0 &&
                   order.Ingredients.Count == 0;
        }
    }
}
