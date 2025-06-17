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
using Newtonsoft.Json.Linq;
using System.Security.Policy;
using System.Xml.Linq;

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
        [Route("api/GetHash")]
        [HttpPost]
        public async Task<IActionResult> GetHash([FromBody] PayUModelRequest model)
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
            if (model == null || model.Card <= 0 || model.Amount <= 0)
            {
                return BadRequest(new PayUModelResponse
                {
                    status = false,
                    message = "Invalid card or amount."
                });
            }
            byte[] hash;
            var Key = _config["PayU:MerchantKey"];
            var Salt = _config["PayU:Salt"];
            model.Surl = Url.Action("Success", "Payment", null, Request.Scheme);
            model.Furl = Url.Action("Failure", "Payment", null, Request.Scheme);
            try
            {
                var data = new
                {
                    key = Key,
                    salt = Salt,
                    amount = model.Amount,
                    txnid = GenerateOrderID(),
                    plan = model.ProductInfo,
                    fname = model.FirstName
                    ,
                    email = model.Email,
                    udf5 = ""
                };
                string d = data.key + "|" + data.txnid + "|" + data.amount + "|" + data.plan + "|" + data.fname + "|" + data.email + "|||||||||||" + data.salt;
                var datab = Encoding.UTF8.GetBytes(d);
                using (SHA512 shaM = new SHA512Managed())
                {
                    hash = shaM.ComputeHash(datab);
                }
                return Ok(new
                {
                    Key = data.key,
                    txn = data.txnid,
                    Amount = data.amount,
                    hash = GetStringFromHash(hash),
                    FName = data.fname,
                    Email = data.email,
                    Plan = data.plan,
                    surl = model.Surl,
                    furl = model.Furl
                });
            }
            catch (Exception ex)
            {
                return BadRequest("inner error");
            }



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
            if (model == null || model.Card <= 0 || model.Amount <= 0)
            {
                return BadRequest(new PayUModelResponse
                {
                    status = false,
                    message = "Invalid card or amount."
                });
            }
            var key = _config["PayU:MerchantKey"];
            var salt = _config["PayU:Salt"];
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
                        message = "Card recharge successful."
                    });
                }
                else
                {
                    return BadRequest(new PayUModelResponse
                    {
                        status = false,
                        message = "Card recharge failed."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new PayUModelResponse
                {
                    status = false,
                    message = $"Error: {ex.Message}"
                });
            }

        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2").ToLower());
            }
            return result.ToString();
        }
        public string GenerateOrderID()
        {
            Random rnd = new Random();
            Int64 s1 = rnd.Next(000000, 999999);
            Int64 s2 = Convert.ToInt64(DateTime.Now.ToString("ddMMyyyyHHmmss"));
            string s3 = s1.ToString() + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8) + s2.ToString();
            return s3;
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
