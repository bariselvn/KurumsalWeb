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
    public class ServicesController : Controller
    {
        private KurumsalDBEntities db = new KurumsalDBEntities();

        // GET: Services
        public ActionResult Index()
        {
            return View(db.Service.ToList());
        }

        // GET: Services/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Service.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Service service, HttpPostedFileBase PictureURL)
        {
            if (ModelState.IsValid)
            {
                if (PictureURL != null)
                {
                    WebImage img = new WebImage(PictureURL.InputStream);
                    FileInfo imginfo = new FileInfo(PictureURL.FileName);
                    string picturename = PictureURL.FileName + imginfo.Extension;
                    img.Resize(500, 500);
                    img.Save("~/Uploads/Services/" + picturename);
                    service.PictureURL = "/Uploads/Services/" + picturename;
                }
                db.Service.Add(service);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(service);
        }

        // GET: Services/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Service.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Service service, HttpPostedFileBase PictureURL)
        {
            if (ModelState.IsValid)
            {
                var servic = db.Service.Where(x => x.ServiceId == id).SingleOrDefault();

                if (PictureURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(servic.PictureURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(servic.PictureURL));
                    }
                    WebImage img = new WebImage(PictureURL.InputStream);
                    FileInfo imginfo = new FileInfo(PictureURL.FileName);

                    string picturename = PictureURL.FileName + imginfo.Extension;
                    img.Resize(500, 500);
                    img.Save("~/Uploads/Services/" + picturename);
                    servic.PictureURL = "/Uploads/Services/" + picturename;
                }
                servic.Title = service.Title;
                servic.Description = service.Description;
             
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(service);
        }

        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Service.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            if (System.IO.File.Exists(Server.MapPath(service.PictureURL)))
            {
                System.IO.File.Delete(Server.MapPath(service.PictureURL));
            }
            db.Service.Remove(service);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Service service = db.Service.Find(id);
            db.Service.Remove(service);
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
