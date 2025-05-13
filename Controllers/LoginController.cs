using Microsoft.AspNetCore.Mvc;
using CardPayment.Data;

namespace CardPayment.Controllers;

public class LoginController : Controller
{
    private readonly AppDbContext _context;

    public LoginController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string ccno, string refPassword)
    {
        var Userdetail = _context.Masters.FirstOrDefault(m => m.CCno == ccno);
        if (Userdetail != null) 
        {
            var Userdetail1 = _context.LCardTempDET.FirstOrDefault(m => m.LCARDDESC.ToString() == Userdetail.CCno);
            if (Userdetail1 != null) 
            {
                var userActive = _context.LOYALTYCARD.FirstOrDefault(m => m.LCARDDESC == Userdetail1.LCARDDESC);
                if (userActive?.ACTIVATE == false)
                {
                    var user = _context.Masters.FirstOrDefault(m => m.CCno == ccno && m.ADD3 == refPassword);

                    if (user != null)
                    {
                        // Store user info in session
                        HttpContext.Session.SetString("CCno", user.CCno ?? "");
                        HttpContext.Session.SetString("Account", user.ACCOUNT ?? "");
                        HttpContext.Session.SetInt32("AID", user.AID);

                        // Redirect to MasterView/Index
                        return RedirectToAction("Index", "CardDetails");
                    }
                    // Invalid login
                    ViewBag.Error = "Invalid Card Number or Password";
                }
                // Invalid login
                ViewBag.Error = "Card is not Active";
            }
            // Invalid login
            ViewBag.Error = "Card is Invalid";
        }

        // Invalid login
        ViewBag.Error = "Card is Invalid";
        return View();
    }
}