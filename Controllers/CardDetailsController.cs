using Microsoft.AspNetCore.Mvc;
using CardPayment.Data;
using CardPayment.Models;

namespace CardPayment.Controllers
{
    public class CardDetailsController : Controller
    {
        private readonly AppDbContext _context;

        public CardDetailsController(AppDbContext context)
        {
            _context = context;
        }
        [Route("CardDetails")]
        public IActionResult Index()
        { 
            //if (HttpContext.Session != null) 
            //{
            //    string username = HttpContext.Session.GetString("CCno");
            //    string name = HttpContext.Session.GetString("Account");
            //    int? userId = HttpContext.Session.GetInt32("AID");
            //}
            string? ccno = HttpContext.Session.GetString("CCno");
            if (string.IsNullOrEmpty(ccno))
            {
                return RedirectToAction("Login", "Home");
            }
            MainModel mainModel = new MainModel();
            mainModel.masterData = _context.Masters.AsEnumerable().Where(x => x.CCno.ToString() == ccno).ToList();
            mainModel.lCardTempdetData = _context.LCardTempDET
                .Where(l => l.LCARDDESC.ToString() == ccno)
                .OrderByDescending(l => l.DLCARDID)
                .ToList();

            return View(mainModel);
        }

    }
}
