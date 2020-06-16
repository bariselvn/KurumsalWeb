using KurumsalWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    public class FeaturesController : Controller
    {
        // GET: Features
        KurumsalDBEntities db = new KurumsalDBEntities();
        public ActionResult Index()
        {
            var query = db.Features.ToList();
            return View(query);
        }

        // GET: Features/Edit/5
        public ActionResult Edit(int id)
        {
            var feature = db.Features.Where(x => x.FeatureId == id).SingleOrDefault();
            return View(feature);
        }

        // POST: Features/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Features features ,HttpPostedFileBase LogoURL)
        {
            if (ModelState.IsValid)
            {
                var feature = db.Features.Where(x => x.FeatureId == id).SingleOrDefault();

                if (LogoURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(feature.LogoURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(feature.LogoURL));
                    }
                    WebImage img = new WebImage(LogoURL.InputStream);
                    FileInfo imginfo = new FileInfo(LogoURL.FileName);

                    string logoname = LogoURL.FileName + imginfo.Extension;
                    img.Resize(200, 200);
                    img.Save("~/Uploads/Features/"+ logoname);

                    feature.LogoURL = "/Uploads/Features/" + logoname;
                }
                feature.Title = features.Title;
                feature.Keywords = features.Keywords;
                feature.Description = features.Description;
                feature.Name = features.Name;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(features);
        }
    }
}
