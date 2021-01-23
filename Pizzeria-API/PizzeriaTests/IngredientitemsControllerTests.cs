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
            var ingredients = actualResult.Value as List<IngredientItem>;

            Assert.IsInstanceOfType(actualResult, typeof(OkObjectResult));
            CollectionAssert.AreEqual(ingredients, data.ToList());
        }

        [TestMethod]
        public void MassDelivery_WhenCalled_ReturnsOkObjectResultWithIncreasedQuantityOfIngredients()
        {
            var data = new List<IngredientItem>
            {
                new IngredientItem
                {
                    IngredientName= "Test ingredient",
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
            var actualResult = service.MassDelivery() as OkObjectResult;
            var ingredients = actualResult.Value as List<IngredientItem>;

            var actualQuantityAfterDelivery = ingredients.FirstOrDefault().Quantity;
            var expectedQuantityAfterDelivery = data.ToList().FirstOrDefault().Quantity;

            Assert.IsInstanceOfType(actualResult, typeof(OkObjectResult));
            Assert.AreEqual(actualQuantityAfterDelivery, expectedQuantityAfterDelivery);
        }

        [TestMethod]
        public void UpdateIngredientQuantityById_IdExistsAndQuantityAPositiveNumber_ReturnsOkObjectResultWithUpdateQuantityOfIngredients()
        {
            var data = new List<IngredientItem>
            {
                new IngredientItem
                {
                    Id=1,
                    IngredientName= "Test ingredient",
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
            int mockId = 1;
            int quantityUpdated = 15;
            var actualResult = service.UpdateIngredientQuantityById(mockId, quantityUpdated) as OkObjectResult;
            var ingredient = actualResult.Value as IngredientItem;
            var actualQuantityAfterDelivery = ingredient.Quantity;
            var expectedQuantityAfterDelivery = data.ToList().FirstOrDefault().Quantity;

            Assert.IsInstanceOfType(actualResult, typeof(OkObjectResult));
            Assert.AreEqual(actualQuantityAfterDelivery, expectedQuantityAfterDelivery);
        }

        [TestMethod]
        public void UpdateIngredientQuantityById_IdDoesNotExist_ReturnsNotFoundObjectResult()
        {
            var data = new List<IngredientItem>
            {
                new IngredientItem
                {
                    Id=1,
                    IngredientName= "Test ingredient",
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
            int mockIdDoesNotExist = 0;
            int quantityUpdated = 15;
            var actualResult = service.UpdateIngredientQuantityById(mockIdDoesNotExist, quantityUpdated) as NotFoundObjectResult;
         
            Assert.IsInstanceOfType(actualResult, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public void UpdateIngredientQuantityById_IdExistsAndQuantityIsANegativeNumber_ReturnsBadRequest()
        {
            var data = new List<IngredientItem>
            {
                new IngredientItem
                {
                    Id=1,
                    IngredientName= "Test ingredient",
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
            int mockId = 1;
            int quantityUpdated = -15;
            var actualResult = service.UpdateIngredientQuantityById(mockId, quantityUpdated) as BadRequestObjectResult;
           
            Assert.IsInstanceOfType(actualResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void CheckAvailabilityInStorage_WhenIngredientDoesNotExistInDatabase_ReturnsNotFound()
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

            List<IngredientOrder> ingredientsFromOrder = new List<IngredientOrder>()
            {
                new IngredientOrder
                {
                    IngredientName="Ingredient Does Not Exist In Database"
                }
            };


            var service = new IngredientItemsController(mockContext.Object);
            var actualResult = service.CheckAvailabilityInStorage(ingredientsFromOrder) as NotFoundResult;

            Assert.IsInstanceOfType(actualResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void CheckAvailabilityInStorage_WhenIngredientQuantityFromOrderIsHigherThenInStorage_ReturnsBadRequest()
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

            List<IngredientOrder> ingredientsFromOrder = new List<IngredientOrder>()
            {
                new IngredientOrder
                {
                    IngredientName="Ham",
                    Quantity = 6
                }
            };


            var service = new IngredientItemsController(mockContext.Object);
            var actualResult = service.CheckAvailabilityInStorage(ingredientsFromOrder) as BadRequestObjectResult;

            Assert.IsInstanceOfType(actualResult, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void CheckAvailabilityInStorage_WhenIngredientQuantityFromOrderIsLowerThenInStorage_ReturnsOkObjectResult()
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

            List<IngredientOrder> ingredientsFromOrder = new List<IngredientOrder>()
            {
                new IngredientOrder
                {
                    IngredientName="Ham",
                    Quantity = 2
                }
            };

            var service = new IngredientItemsController(mockContext.Object);
            var actualResult = service.CheckAvailabilityInStorage(ingredientsFromOrder) as OkObjectResult;
            var ingredients = actualResult.Value as List<IngredientItem>;
         
            Assert.IsInstanceOfType(actualResult, typeof(OkObjectResult));
            CollectionAssert.AreEqual(ingredients, data.ToList());
        }

        [TestMethod]
        public void GetIngredienQuantityByName_WhenIngredientNameDoesNotExist_ReturnsZero()
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
            string ingredientName = "Ingredient does not exist in the database";       

            var service = new IngredientItemsController(mockContext.Object);
            var actualResult = service.GetIngredienQuantityByName(ingredientName);

            Assert.AreEqual(actualResult, 0);
        }


        [TestMethod]
        public void GetIngredienQuantityByName_WhenIngredientNameExists_ReturnsQuantity()
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
            string ingredientName = "ham";

            var service = new IngredientItemsController(mockContext.Object);
            var actualResult = service.GetIngredienQuantityByName(ingredientName);

            Assert.AreEqual(actualResult, 5);
        }
    }
}
