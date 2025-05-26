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

namespace CardPayment.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IConfiguration _config;

        public PaymentController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [Route("api/payment")]
        [HttpPost]
        public JsonResult Pay([FromBody] PayUModelRequest model)
        {
            var key = _config["PayU:MerchantKey"];
            var salt = _config["PayU:Salt"];
            var baseUrl = _config["PayU:BaseUrl"];

            model.Key = key;
            model.TxnId = Guid.NewGuid().ToString("N").Substring(0, 20);
            model.Hash = PayUHashHelper.GenerateHash(key, model.TxnId, model.Amount, model.ProductInfo, model.FirstName, model.Email, salt);
            model.Surl = Url.Action("Success", "Payment", null, Request.Scheme);
            model.Furl = Url.Action("Failure", "Payment", null, Request.Scheme);

            ViewBag.PayUUrl = baseUrl;
            PayUModelResponse payUModelResponse = new PayUModelResponse();
            payUModelResponse.message = "success";
            payUModelResponse.status = true;

            return Json(payUModelResponse);
            //return View("Pay", model);
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
