using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardPayment.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CardPayment.Data;
using CardPayment.Models;
using XSystem.Security.Cryptography;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CardPayment.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public PaymentController(AppDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [Route("api/payment")]
        [HttpPost]
        public async Task<IActionResult> Pay([FromBody] PayUModelRequest model)
        {
            var cardNumber = HttpContext.Session.GetString("CCno");
            if (string.IsNullOrEmpty(cardNumber))
            {
                return BadRequest(new PayUModelResponse
                {
                    status = false,
                    message = "Session time Out",
                    PayUUrl = "0"
                });
            }
            var key = _config["PayU:MerchantKey"];
            var salt = _config["PayU:Salt"];
            var baseUrl = _config["PayU:BaseUrl"];

            model.Key = key;
            model.TxnId = Guid.NewGuid().ToString("N").Substring(0, 20);
            model.Hash = PayUHashHelper.GenerateHash(key, model.TxnId, model.AmountStr, model.ProductInfo, model.FirstName, model.Email, salt);
            model.Surl = Url.Action("Success", "Payment", null, Request.Scheme);
            model.Furl = Url.Action("Failure", "Payment", null, Request.Scheme);

            try
            {
                using var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "dbo.CardRecharge";
                command.CommandType = CommandType.StoredProcedure;

                var cardParam = new SqlParameter("@card", SqlDbType.Decimal)
                {
                    Precision = 20,
                    Scale = 0,
                    Value = model.Card
                };

                var amountParam = new SqlParameter("@Amount", SqlDbType.Int)
                {
                    Value = model.Amount
                };

                var returnParam = new SqlParameter
                {
                    ParameterName = "@ReturnVal",
                    Direction = ParameterDirection.ReturnValue
                };

                command.Parameters.Add(cardParam);
                command.Parameters.Add(amountParam);
                command.Parameters.Add(returnParam);

                await command.ExecuteNonQueryAsync();

                var result = (int)returnParam.Value;
                if (result == 1)
                {
                    return Ok(new PayUModelResponse
                    {
                        status = true,
                        message = "Card recharge successful.",
                        PayUUrl = baseUrl
                    });
                }
                else
                {
                    return BadRequest(new PayUModelResponse
                    {
                        status = false,
                        message = "Card recharge failed.",
                        PayUUrl = baseUrl
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new PayUModelResponse
                {
                    status = false,
                    message = $"Error: {ex.Message}",
                    PayUUrl = baseUrl
                });
            }

        }


        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Failure()
        {
            return View();
        }
    }

}
