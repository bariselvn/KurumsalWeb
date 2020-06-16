using KurumsalWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    public class AboutusController : Controller
    {
        KurumsalDBEntities db = new KurumsalDBEntities();
        // GET: Aboutus
        public ActionResult Index()
        {
            var about = db.AboutUs.ToList();
            return View(about);
        }
        public ActionResult Edit(int id)
        {
            var about = db.AboutUs.Where(x => x.AboutID== id).SingleOrDefault();
            return View(about);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit (int id, AboutUs aboutus)
        {
            if (ModelState.IsValid) { 
                var about = db.AboutUs.Where(X => X.AboutID == id).SingleOrDefault();
                about.Description = aboutus.Description;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(aboutus);
        }
    }
}