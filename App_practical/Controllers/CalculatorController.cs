using Microsoft.AspNetCore.Mvc;
using Calculator.Models;
using System.Diagnostics;

namespace CalculatorApp.Controllers
{
    public class CalculatorController : Controller
    {
        public CalculatorController()
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(double num1, double num2, string operation)
        {
            double result = 0;
            var model = new CalculatorModel
            {
                Number1 = num1,
                Number2 = num2,
                Operation = operation
            };
            switch (operation)
            {
                case "add":
                    result = num1 + num2;
                    break;
                case "subtract":
                    result = num1 - num2;
                    break;
                case "multiply":
                    result = num1 * num2;
                    break;
                case "divide":
                    result = num2 != 0 ? num1 / num2 : 0;
                    break;
            }

 
            ViewBag.Result = result;
            ViewBag.Num1 = num1;
            ViewBag.Num2 = num2;
            ViewBag.Operation = operation;

            return View("Index");
  
        }
    }
}