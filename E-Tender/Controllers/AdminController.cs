using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_Tender.Models;
namespace E_Tender.Controllers
{
    public class AdminController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        
        // GET: Admin
        public ActionResult Login()
        {
            Admin a = new Admin();
            return View(a);
        }

        [HttpPost]
        public ActionResult Login(Admin obj)
        {
            Admin a = (from k in db.Admins
                       where k.AdminId == obj.AdminId
                       && k.AdminPassword == obj.AdminPassword
                       select k).FirstOrDefault();
            if (a != null)
            {
                Session["username"] = obj.AdminId;
                return RedirectToAction("AdminWelcome", "Admin");
            }
            else
            {
                ViewData["Invalid"] = "Invalid Username or Password";
            }
            return View();
        }

        public ActionResult AdminWelcome()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("username");
            return RedirectToAction("Login");
        }

    }
}