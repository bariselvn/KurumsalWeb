using KurumsalWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace KurumsalWeb.Controllers
{
    public class HomeController : Controller
    {
        KurumsalDBEntities db = new KurumsalDBEntities();
        // GET: Home
        public ActionResult Index()
        {          
            return View();
        }
        public ActionResult SliderPartial()
        {
            return View(db.Slider.ToList().OrderByDescending(x => x.SliderId));
        }
        public ActionResult ServicesPartial()
        {
            
            return View(db.Service.ToList().OrderByDescending(x => x.ServiceId));
        }
        public ActionResult Aboutus ()
        {
            return View(db.AboutUs.SingleOrDefault());
        }
    
        public ActionResult OurServices()
        {

            return View(db.Service.ToList().OrderByDescending(x => x.ServiceId));
        }
        public ActionResult ContactUs()
        {
            return View(db.Contact.SingleOrDefault());
        }
        [HttpPost]
        public ActionResult ContactUs(string name=null,string email=null,string subject=null,string message=null)
        {
            if(name!=null && email != null)
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "nckurumsalweb@gmail.com";
                WebMail.Password = "baris5252";
                WebMail.SmtpPort = 587;
                WebMail.Send("nckurumsalweb@gmail.com", subject, email +message);
            
            }
            return View();
        }
        public ActionResult Blog(int Page=1)
        {
            return View(db.Blog.Include("Category").OrderByDescending(x => x.BlogId).ToPagedList(Page,5));
        }
        public ActionResult BlogCategoryPartial()
        {
            return View(db.Category.Include("Blog").ToList().OrderBy(x=>x.Categoryname));
        }
        public ActionResult BlogCategoryPage(int id, int Page = 1)
        {
            return View(db.Blog.Include("Category").Where(x=>x.Category.CategoryId==id).OrderByDescending(x => x.BlogId).ToPagedList(Page, 5));
        }
        public ActionResult BlogRecentPostPartial()
        {
            return View(db.Blog.ToList().OrderByDescending(x => x.BlogId));
        }
        public ActionResult BlogDetail(int id)
        {
            var blog = db.Blog.Include("Category").Include("Comment").Where(x => x.BlogId == id).SingleOrDefault();
            return View(blog);
        }
        public JsonResult PostComment(string name,string email,string content,int blogid)
        {
            if (content==null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            db.Comment.Add(new Comment { Username = name, Mail = email, Contents = content, BlogId = blogid,Status=false });
            db.SaveChanges();
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FooterPartial()
        {
            ViewBag.Contact = db.Contact.SingleOrDefault();
            ViewBag.Services = db.Service.ToList();
            ViewBag.Blog = db.Blog.ToList();

            return PartialView();
        }
    }
}