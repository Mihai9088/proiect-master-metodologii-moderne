using Microsoft.AspNetCore.Mvc;
using SpendApp.Models;
using System.Diagnostics;

namespace SpendApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly SpendAppDbContext _context;

        public HomeController(ILogger<HomeController> logger, SpendAppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Expenses()
        {
            var allExpenses = _context.Expenses.ToList();
            var totalExpenses = allExpenses.Sum(x => x.Value);
            ViewBag.Expenses = totalExpenses;
            return View(allExpenses);
        }

        [HttpPost]
        public IActionResult CreateEditExpenseForm(Expense model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateEditExpense", model);
            }

            string message;

            if (model.Id == 0)
            {
                _context.Expenses.Add(model);
                message = "Expense added successfully.";
            }
            else
            {
                var expenseInDb = _context.Expenses.FirstOrDefault(e => e.Id == model.Id);
                if (expenseInDb == null)
                {
                    return NotFound();
                }

                expenseInDb.Value = model.Value;
                expenseInDb.Description = model.Description;
                message = "Expense updated successfully.";
            }

            _context.SaveChanges();
            TempData["Message"] = message;

            return RedirectToAction("Expenses");
        }

        public IActionResult CreateEditExpense(int id)
        {
            if (id == 0)
            {
                return View(new Expense());
            }

            var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);

            if (expenseInDb == null)
            {
                return RedirectToAction("Expenses");
            }

            return View(expenseInDb);
        }


        public IActionResult DeleteExpense(int id)
        {
            var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);

            if (expenseInDb == null)
            {
                return RedirectToAction("Expenses");
            }

            _context.Expenses.Remove(expenseInDb);
            _context.SaveChanges();
            return RedirectToAction("Expenses");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
