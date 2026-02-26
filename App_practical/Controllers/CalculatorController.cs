using Microsoft.AspNetCore.Mvc;
using Calculator.Data;
using Calculator.Models;
using System.Diagnostics;

namespace CalculatorApp.Controllers
{
    public class CalculatorController : Controller
    {
        private CalculatorContext _context;
        public CalculatorController(CalculatorContext context)
        {
            _context = context;
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

            DataInputVariant dataImputVariant = new DataInputVariant();
            dataImputVariant.Operand_1 = num1.ToString();
            dataImputVariant.Operand_2 = num2.ToString();
            dataImputVariant.Type_operation = operation.ToString();

            _context.DataInputVariants.Add(dataImputVariant);
            _context.SaveChanges();
            ViewBag.Result = result;
            ViewBag.Num1 = num1;
            ViewBag.Num2 = num2;
            ViewBag.Operation = operation;

            return View("Index");
            return View(model);

            
            
            
            
            
        }
    }
}