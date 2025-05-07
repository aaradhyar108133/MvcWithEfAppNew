using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MvcWithEfApp.Data;
using MvcWithEfApp.Models;

namespace MvcWithEfApp.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
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
       

        var user = _context.Masters.FirstOrDefault(m => m.CCno == ccno && m.ADD3 == refPassword);

        if (user != null)
        {
            // Store user info in session
            HttpContext.Session.SetString("CCno", user.CCno ?? "");
            HttpContext.Session.SetString("Account", user.ACCOUNT ?? "");
            HttpContext.Session.SetInt32("AID", user.AID);

            // Redirect to MasterView/Index
            return RedirectToAction("Index", "MasterView");
        }

        // Invalid login
        ViewBag.Error = "Invalid CCno or ADD3";
        return View();
    }
}