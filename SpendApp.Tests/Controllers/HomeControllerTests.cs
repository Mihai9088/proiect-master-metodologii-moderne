using Xunit;
using Moq;
using SpendApp.Controllers;
using SpendApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace SpendApp.Tests.Controllers
{
    public class HomeControllerTests
    {
        private SpendAppDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<SpendAppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new SpendAppDbContext(options);
        }

        private HomeController GetController(SpendAppDbContext context)
        {
            var loggerMock = new Mock<ILogger<HomeController>>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            var controller = new HomeController(loggerMock.Object, context)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                },
                TempData = tempData
            };

            return controller;
        }

        [Fact]
        public void Expenses_ReturnsExpensesViewWithData()
        {
            using var context = GetDbContext("Db1");
            context.Expenses.AddRange(
                new Expense { Value = 50, Description = "Food" },
                new Expense { Value = 100, Description = "Utilities" }
            );
            context.SaveChanges();

            var controller = GetController(context);

            var result = controller.Expenses() as ViewResult;

            var model = Assert.IsAssignableFrom<List<Expense>>(result.Model);
            Assert.Equal(2, model.Count);
            Assert.Equal(150m, (decimal)result.ViewData["Expenses"]);
        }

        [Fact]
        public void DeleteExpense_RemovesExpense()
        {
            using var context = GetDbContext("Db4");
            var expense = new Expense { Value = 75, Description = "Snacks" };
            context.Expenses.Add(expense);
            context.SaveChanges();

            var controller = GetController(context);

            var result = controller.DeleteExpense(expense.Id);

            Assert.Empty(context.Expenses);
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Expenses", ((RedirectToActionResult)result).ActionName);
        }
    }
}
