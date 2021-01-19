using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pizzeria_Storage_API.Controllers;
using Pizzeria_Storage_API.Data;
using Pizzeria_Storage_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaTests
{
    [TestClass]
    public class IngredientitemsControllerTests
    {
        [TestMethod]
        public void GetIngredients_WhenCalled_ReturnsOkObjectResult()
        {
            var data = new List<IngredientItem>
            {
                new IngredientItem
                {
                    IngredientName= "Ham",
                    Price= 10,
                    Quantity=5
                }
              
            }.AsQueryable();

            var mockSet = new Mock<DbSet<IngredientItem>>();
            mockSet.As<IQueryable<IngredientItem>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<IngredientItem>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<IngredientItem>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<IngredientItem>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IngredientContext>();
            mockContext.Setup(c => c.Ingredients).Returns(mockSet.Object);

            var service = new IngredientItemsController(mockContext.Object);
            var actualResult = service.GetIngredients() as OkObjectResult;
            var value = actualResult.Value as List<IngredientItem>;

            Assert.IsInstanceOfType(actualResult, typeof(OkObjectResult));
            CollectionAssert.AreEqual(value,data.ToList());
        }
    }
}
