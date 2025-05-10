using Microsoft.AspNetCore.Mvc;
using MvcWithEfApp.Data;
using MvcWithEfApp.Models;

namespace MvcWithEfApp.Controllers
{
    public class MasterViewController : Controller
    {
        private readonly AppDbContext _context;

        public MasterViewController(AppDbContext context)
        {
            _context = context;
        }
        [Route("CareDetails")]
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
            mainModel.masterData = _context.Masters.Where(x => x.CCno.ToString() == ccno).ToList();
            mainModel.lCardTempdetData = _context.LCardTempDET
                .Where(l => l.LCARDDESC.ToString() == ccno)
                .OrderByDescending(l => l.DLCARDID)
                .ToList();

            return View(mainModel);
        }
    }
}
