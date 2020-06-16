using KurumsalWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    public class BlogController : Controller
    {
        KurumsalDBEntities db = new KurumsalDBEntities();
        // GET: Blog
        public ActionResult Index()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return View(db.Blog.Include("Category").ToList()) ;
        }
        // GET: Services/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "Categoryname");
            return View();
        }

        // POST: Services/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Blog blog, HttpPostedFileBase PictureURL)
        {
            if (ModelState.IsValid)
            {
                if (PictureURL != null)
                {
                    WebImage img = new WebImage(PictureURL.InputStream);
                    FileInfo imginfo = new FileInfo(PictureURL.FileName);
                    string picturename = PictureURL.FileName + imginfo.Extension;
                    img.Resize(500, 500);
                    img.Save("~/Uploads/Blogs/" + picturename);
                    blog.PictureURL = "/Uploads/Blogs/" + picturename;
                }
                db.Blog.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blog);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blog.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "Categoryname",blog.CategoryId);
            return View(blog);
        }

        // POST: Services/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Blog blog, HttpPostedFileBase PictureURL)
        {
            if (ModelState.IsValid)
            {
                var bl = db.Blog.Where(x => x.BlogId == id).SingleOrDefault();

                if (PictureURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(bl.PictureURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(bl.PictureURL));
                    }
                    WebImage img = new WebImage(PictureURL.InputStream);
                    FileInfo imginfo = new FileInfo(PictureURL.FileName);

                    string picturename = PictureURL.FileName + imginfo.Extension;
                    img.Resize(500, 500);
                    img.Save("~/Uploads/Blogs/" + picturename);
                    bl.PictureURL = "/Uploads/Blogs/" + picturename;
                }
                bl.Title = blog.Title;
                bl.Contents = blog.Contents;
                bl.CategoryId = blog.CategoryId;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blog);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blog.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            if (System.IO.File.Exists(Server.MapPath(blog.PictureURL)))
            {
                System.IO.File.Delete(Server.MapPath(blog.PictureURL));
            }
            db.Blog.Remove(blog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}