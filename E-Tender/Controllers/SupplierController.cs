using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_Tender.Models;


namespace E_Tender.Controllers
{
    public class SupplierController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        // GET: Supplier
        public ActionResult Index()
        {
            var res = from k in db.Suppliers
                      select k;
            return View(res.ToList());
        }

        public ActionResult RegisterSupplier()
        {
            Supplier t = new Supplier();
            return View(t);
        }
        [HttpPost]
        public ActionResult RegisterSupplier(Supplier u, HttpPostedFileBase file1)

            {
            try
            {
                byte[] imgdata = new byte[file1.ContentLength];
                file1.InputStream.Read(imgdata, 0, file1.ContentLength);
                u.Image = imgdata;
                u.IsActive = "False";
                db.Suppliers.InsertOnSubmit(u);
                db.SubmitChanges();
                TempData["msg"] = "Registration Success Please Wait for admin to verify your details";
                TempData["abc"] = "It may take 2/3 days to verify after Verification you can login";

                return RedirectToAction("RegSucces");
            }
            catch
            {
                TempData["msg"] = "Registration Failed";
                return View();
            }
        }

        public ActionResult RegSucces()
        {
            return View();
        }

        public ActionResult LoginUser()
        {
            Supplier u = new Supplier();
            return View(u);
        }

        [HttpPost]
        public ActionResult LoginUser(Supplier u)
        {

            Supplier t = (from k in db.Suppliers
                            where k.SCompanyId.Equals(u.SCompanyId)
                            && k.Password.Equals(u.Password) && k.IsActive == "True"
                            select k
                            ).FirstOrDefault();

            Supplier t2 = (from k in db.Suppliers
                             where k.SCompanyId.Equals(u.SCompanyId)
                             && k.Password.Equals(u.Password) && k.IsActive == "False"
                             select k
                            ).FirstOrDefault();
            if (t != null)
            {
                Session["username"] = u.SCompanyId;
                Session["Id"] = t.Id;
                return RedirectToAction("SupplierHome");
            }
            if (t == null)
            {
                TempData["msg1"] = "Invalid user id or password";
            }
            if (t2 != null)
                TempData["msg1"] = "Your Verification is under process";
            return View();
        }

        public ActionResult supplierhome(Tender t)
        {
            if (Session["username"] != null)
            {

                var res = from k in db.Tenders
                          select k;
                TempData["Id"] = t.Tender_Registration_Number;
                return View(res.ToList());
            }
            else
            {
                return RedirectToAction("loginuser");
            }
        }



        public ActionResult Logout()
        {
            Session.Remove("username");
            return RedirectToAction("LoginUser");

        }
        [HttpPost]
        public ActionResult Accept(Supplier t)
        {
            Supplier t1 = db.Suppliers.Where(x => x.SCompanyId == t.SCompanyId).FirstOrDefault();
            t1.SCompanyId = t.SCompanyId;
            t1.SCompanyName = t.SCompanyName;
            t1.IsActive = "True";
            db.SubmitChanges();
           // return Json(t1, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult Delete(Supplier s)
        {
            Supplier t1 = db.Suppliers.First(x => x.SCompanyId.Equals(s.SCompanyId));
            db.Suppliers.DeleteOnSubmit(t1);
            db.SubmitChanges();
            var res = from k in db.Suppliers
                      select k;

            return Json(t1);

        }
        public JsonResult Edit(Models.Supplier t)
        {

            // TODO: Add update logic here
            Supplier t1 = db.Suppliers.Where(x => x.SCompanyId == t.SCompanyId).FirstOrDefault();
            t1.SCompanyId = t.SCompanyId;
            t1.SCompanyName = t.SCompanyName;
            db.SubmitChanges();
            return Json(t1, JsonRequestBehavior.AllowGet);
        }
    }
}