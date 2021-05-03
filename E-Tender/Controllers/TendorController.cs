using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_Tender.Models;

namespace E_Tender.Controllers
{
    public class TendorController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        // GET: Tendor
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Tender t, HttpPostedFileBase file1)
        {
            try
            {
                byte[] imgdata = new byte[file1.ContentLength];
                file1.InputStream.Read(imgdata, 0, file1.ContentLength);
                t.Tender_Documents = imgdata; 
                db.Tenders.InsertOnSubmit(t);
                db.SubmitChanges();
                return RedirectToAction("CompanyHome","Company");
            }
            catch
            {
                TempData["msg"] = "Tender details was not filled properly ";
                return View();
            }
        }

       
        public ActionResult Accept()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Accept(TenderResponse t)
        {
            try
            {
                db.TenderResponses.InsertOnSubmit(t);
                db.SubmitChanges();
                return RedirectToAction("supplierhome", "Supplier");
            }
            catch
            {
                return RedirectToAction("Accept");
            }
            
        }

        public ActionResult Accepted()
        {
            return View();
        }
    }
}