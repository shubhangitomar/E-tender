using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_Tender.Models;

namespace E_Tender.Controllers
{
    public class CompanyController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        // GET: Company
        public ActionResult Index()
        {
            var res = from k in db.Companies
                      select k;
            return View(res.ToList());
        }

        public ActionResult RegisterCompany()
        {
            Company t = new Company();
            return View(t);
        }


        [HttpPost]
        public ActionResult RegisterCompany(Company u, HttpPostedFileBase file1)
        {
            try
            {
                byte[] imgdata = new byte[file1.ContentLength];
                file1.InputStream.Read(imgdata, 0, file1.ContentLength);
                u.ProfilePhoto = imgdata;
                u.IsActive = "False";
                db.Companies.InsertOnSubmit(u);
                db.SubmitChanges();
                TempData["msg"] = "Registration Success Please Wait for admin to verify your details";
                TempData["abc"] = "It may take 2/3 days to verify after Verification you can login";

                return RedirectToAction("RegSucces");
            }
            catch
            {
                TempData["msg"] = "Registration Failed All Field are necessary";
                return View();
            }
        }

        public ActionResult RegSucces()
        {
            return View();
        }

        public ActionResult LoginUser()
        {
            Company u = new Company();
            return View(u);
        }

        [HttpPost]
        public ActionResult LoginUser(Company u)
        {

            Company t = (from k in db.Companies
                         where k.CCompanyId.Equals(u.CCompanyId)
                         && k.Password.Equals(u.Password) && k.IsActive == "True"
                         select k
                            ).FirstOrDefault();

            Company t2 = (from k in db.Companies
                          where k.CCompanyId.Equals(u.CCompanyId)
                          && k.Password.Equals(u.Password) && k.IsActive == "False"
                          select k).FirstOrDefault();
            if (t != null)
            {
                Session["username"] = t.CCompanyId;
                Session["Id"] = t.Id;
                TempData["name"] = t.CName;
                return RedirectToAction("CompanyHome");
            }
            if (t == null)
            {
                TempData["msg1"] = "Invalid user id or password";
            }
            if (t2 != null)
                TempData["msg1"] = "Your Verification is under process";
            return View();
        }

        public ActionResult CompanyHome(Company c)
        {

            if (Session["username"] != null)
            {
                TempData["name"] = c.CName;

                return View();
            }
            else
            {
                return RedirectToAction("LoginUser");
            }
        }


        public ActionResult TenderList(TenderResponse tr)
        {

            if (Session["username"] != null)
            {
                var res = from k in db.TenderResponses
                                     select k;
                return View(res.ToList());
            }
            else
            {
                return RedirectToAction("LoginUser");
            }
        }

        public ActionResult Logout()
        {
            Session.Remove("username");
            return RedirectToAction("LoginUser");

        }
        public JsonResult Edit(Models.Company t)
        {

            // TODO: Add update logic here
            Company t1 = db.Companies.Where(x => x.CCompanyId == t.CCompanyId).FirstOrDefault();
            t1.CCompanyId = t.CCompanyId;
            t1.CName = t.CName;
            t1.City = t.City;
            t1.State = t.State;
            t1.ContactNo = t.ContactNo;
            db.SubmitChanges();
            return Json(t1, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Accept(Company t)
        {
            Company t1 = db.Companies.Where(x => x.CCompanyId == t.CCompanyId).FirstOrDefault();
            t1.CCompanyId = t.CCompanyId;
            t1.CName = t.CName;
            t1.City = t.City;
            t1.State = t.State;
            t1.ContactNo = t.ContactNo;
            t1.IsActive = "True";
            db.SubmitChanges();
            return Json(t1, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Delete(String CCompanyId)
        {
            Company t1 = db.Companies.Where(x => x.CCompanyId == CCompanyId).FirstOrDefault();
            db.Companies.DeleteOnSubmit(t1);
            db.SubmitChanges();
            var res = from k in db.Companies
                      select k;

            return Json(t1);

        }
    }
}