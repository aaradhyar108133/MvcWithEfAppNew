using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcWithEfApp.Data;
using MvcWithEfApp.Models;
using XSystem.Security.Cryptography;

namespace MvcWithEfApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IConfiguration _config;

        public PaymentController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Pay()
        {
           // var merchantKey = _config["PayU:MerchantKey"];
           // var salt = _config["PayU:MerchantSalt"];
            var baseUrl = _config["PayU:BaseUrl"];  // You’ll pass this to the view

            string key = _config["PayU:MerchantKey"];
            string salt = _config["PayU:MerchantSalt"];
            string txnid = "99b69aae9956497ea6be";
            string amount = "500";
            string productInfo = "Test Product";
            string firstName = "John";
            string email = "john@example.com";
            var phone = "9876543210";

            var surl = Url.Action("Success", "Payment", null, Request.Scheme);
            var furl = Url.Action("Failure", "Payment", null, Request.Scheme);


            // Even if udf fields are empty, they must be included
            string udf1 = ""; string udf2 = ""; string udf3 = "";
            string udf4 = ""; string udf5 = "";

            // Build the hash string with exactly 15 values before salt
            string hashString = string.Join("|", new string[] {
    key, txnid, amount, productInfo, firstName, email,
    udf1, udf2, udf3, udf4, udf5, "", "", "", "", "", salt
});

            string hash = GenerateSHA512Hash(hashString);

            var model = new PayUModel
            {
                Key = key,
                TxnId = txnid,
                Amount = "500",
                ProductInfo = "Test Product",
                FirstName = "John",
                Email = "john@example.com",
                Phone = "9876543210",
                Surl = Url.Action("Success", "Payment", null, Request.Scheme),
                Furl = Url.Action("Failure", "Payment", null, Request.Scheme),
                Udf1 = "",
                Udf2 = "",
                Udf3 = "",
                Udf4 = "",
                Udf5 = ""

            };
            // Correct hash generation:
            model.Hash = GeneratePayUHash(model, key, salt);
            Console.WriteLine("Hash String: " + model.Hash);
            ViewBag.PayUUrl = baseUrl; // ?? Pass the URL to the view

            return View(model);
        }
        public string GeneratePayUHash(PayUModel model, string key, string salt)
        {
            string[] hashVars = new string[]
            {
        key,
        model.TxnId,
        model.Amount,
        model.ProductInfo,
        model.FirstName,
        model.Email,
        model.Udf1 ?? "",  // Empty if not provided
        model.Udf2 ?? "",
        model.Udf3 ?? "",
        model.Udf4 ?? "",
        model.Udf5 ?? "",
        "", "", "", "", "", // udf6-udf10 (empty)
        salt
            };

            // Join the array with "|" separator
            string hashString = string.Join("|", hashVars);

            // Log hash string for debugging
            Console.WriteLine("Hash String: " + hashString);  // Log for debugging

            // Generate the hash using SHA512
            return GenerateSHA512Hash(hashString);
        }




        private string GenerateSHA512Hash(string text)
        {
            byte[] message = Encoding.UTF8.GetBytes(text);
            using (SHA512Managed hashString = new SHA512Managed())
            {
                byte[] hashValue = hashString.ComputeHash(message);
                return string.Concat(hashValue.Select(b => b.ToString("x2")));
            }
        }

        public ActionResult Success()
        {
            // handle success: update DB via Entity Framework
            return View();
        }

        public ActionResult Failure()
        {
            // handle failure
            return View();
        }
    }

}
