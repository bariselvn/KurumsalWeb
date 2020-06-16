using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using KurumsalWeb.Models;

namespace KurumsalWeb.Controllers
{
    public class SliderController : Controller
    {
        private KurumsalDBEntities db = new KurumsalDBEntities();

        // GET: Slider
        public ActionResult Index()
        {
            return View(db.Slider.ToList());
        }

        // GET: Slider/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // GET: Slider/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Slider/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Slider slider, HttpPostedFileBase PictureURL)
        {
            if (ModelState.IsValid)
            {
                if (PictureURL != null)
                {
                    WebImage img = new WebImage(PictureURL.InputStream);
                    FileInfo imginfo = new FileInfo(PictureURL.FileName);
                    string picturename = PictureURL.FileName + imginfo.Extension;
                    img.Resize(1024, 360);
                    img.Save("~/Uploads/Sliders/" + picturename);
                    slider.PictureURL = "/Uploads/Sliders/" + picturename;
                }
                db.Slider.Add(slider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(slider);
        }

        // GET: Slider/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Slider/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Slider slider, HttpPostedFileBase PictureURL)
        {
            if (ModelState.IsValid)
            {
                var slid = db.Slider.Where(x => x.SliderId == id).SingleOrDefault();

                if (PictureURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(slid.PictureURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(slid.PictureURL));
                    }
                    WebImage img = new WebImage(PictureURL.InputStream);
                    FileInfo imginfo = new FileInfo(PictureURL.FileName);

                    string picturename = PictureURL.FileName + imginfo.Extension;
                    img.Resize(1024, 360);
                    img.Save("~/Uploads/Sliders/" + picturename);
                    slid.PictureURL = "/Uploads/Sliders/" + picturename;
                }
                slid.Title = slider.Title;
                slid.Description = slider.Description;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slider);
        }

        // GET: Slider/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            if (System.IO.File.Exists(Server.MapPath(slider.PictureURL)))
            {
                System.IO.File.Delete(Server.MapPath(slider.PictureURL));
            }
            db.Slider.Remove(slider);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Slider/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slider slider = db.Slider.Find(id);
            db.Slider.Remove(slider);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
