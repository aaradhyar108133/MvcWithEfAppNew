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
            var cardNumber = HttpContext.Session.GetString("CCno");
            if (string.IsNullOrEmpty(cardNumber))
            {
                return RedirectToAction("Login", "Home");
            }
            MainModel mainModel = new MainModel();
            mainModel.masterData = _context.Masters.AsEnumerable().Where(x => x.CCno.ToString() == cardNumber).ToList();
            mainModel.lCardTempdetData = _context.LCardTempDET
                .Where(l => l.LCARDDESC.ToString() == cardNumber)
                .OrderByDescending(l => l.DLCARDID)
                .ToList();

            return View(mainModel);
        }

    }
}
