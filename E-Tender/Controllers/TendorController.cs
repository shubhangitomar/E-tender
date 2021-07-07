using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_Tender.Models;
using System.Web.Helpers;

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


        public ActionResult Response()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Response(string username)
        {
            string subject = "Response of Tender";
            string body = "We are happy to announce your proposal for our tender has been approved .We can continue with further process";
            WebMail.Send(username, subject, body, null, null, null, true, null, null, null, null, null, null);
            ViewBag.msg = "Email sent successfully.....";
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            TenderResponse t1 = db.TenderResponses.First(x => x.Id == Id);
            db.TenderResponses.DeleteOnSubmit(t1);
            db.SubmitChanges();
            return RedirectToAction("TenderList","Company");

        }


        public ActionResult GetImage(int Id)
        {
            Tender t = (from k in db.Tenders
                          where k.Id == Id
                          select k).FirstOrDefault();

            byte[] imgdata = t.Tender_Documents.ToArray();
            return File(imgdata, "image/jpeg/png/pdf");
        }
    }
}