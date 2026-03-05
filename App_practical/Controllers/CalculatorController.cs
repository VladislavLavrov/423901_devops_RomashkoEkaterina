using Calculator.Data;
using Calculator.Data;
using Calculator.Models;
using Calculator.Models;
using Calculator.Services;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace Calculator.Controllers
{
    public class CalculatorController : Controller
    {
        private CalculatorContext _context;
        private readonly KafkaProducerService<Null, string> _producer;
        public CalculatorController(CalculatorContext context, KafkaProducerService<Null, string> producer)
        {
            _context = context;
            _producer = producer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Index(double number1, double number2, Operation operation)
        {
            double result = 0;
            var dataInputVariant = new DataInputVariant
            {
                Operand_1 = number1,
                Operand_2 = number2,
                Type_operation = operation,
            };
            switch (operation)
            {
                case Operation.Add:
                    result = number1 + number2;
                    break;
                case Operation.Subtract:
                    result = number1 - number2;
                    break;
                case Operation.Multiply:
                    result = number1 * number2;
                    break;
                case Operation.Divide:
                    result = number2 != 0 ? number1 / number2 : 0;
                    break;
            }
            await SendDataToKafka(dataInputVariant);
            ViewBag.Result = result;
            ViewBag.Number1 = number1;
            ViewBag.Number2 = number2;
            ViewBag.Operation = operation;
            return View();
        }
        public IActionResult Callback([FromBody] DataInputVariant inputData)
        {
            SaveDataAndResult(inputData);
            return Ok();
        }
        private DataInputVariant SaveDataAndResult(DataInputVariant inputData)
        {
            _context.DataInputVariants.Add(inputData);
            _context.SaveChanges();
            return inputData;
        }
        private async Task SendDataToKafka(DataInputVariant dataInputVariant)
        {
            var json = JsonSerializer.Serialize(dataInputVariant);
            await _producer.ProduceAsync("13_Calculator", new Message<Null, string>
            { Value = json });
        }
    }
}