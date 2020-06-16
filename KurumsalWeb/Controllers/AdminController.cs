using KurumsalWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        KurumsalDBEntities db = new KurumsalDBEntities();
        public ActionResult Index()
        {
            ViewBag.BlogCount = db.Blog.Count();
            ViewBag.ServiceCount = db.Service.Count();
            ViewBag.CategoryCount = db.Category.Count();
            ViewBag.CommentCount = db.Comment.Count();
            ViewBag.CommentStatus = db.Comment.Where(x => x.Status == false).Count();
            var query = db.Category.ToList();
            return View(query);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            
            var login = db.Admin.Where(x => x.Mail == admin.Mail).SingleOrDefault();
            if (login != null) {
                if (login.Mail == admin.Mail && login.password == Crypto.Hash(admin.password, "MD5"))
                {
                    Session["adminId"] = login.AdminId;
                    Session["mail"] = login.Mail;
                    Session["permission"] = login.Permission;

                    return RedirectToAction("Index", "Admin");
                }
             
                
            }
            ViewBag.Uyari = ("Email ya da şifreniz yanlış");
            return View();

        }
        public ActionResult Logout()
        {
            Session["adminId"] = null;
            Session["mail"] = null;
            Session["permission"] = null;
            Session.Abandon();

            return RedirectToAction("Login", "Admin");
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string mail)
        {
            var user = db.Admin.Where(x => x.Mail == mail).SingleOrDefault();
            if (user != null)
            {
                Random random = new Random();
                string newpassword = "1234";
                Admin admin = new Admin();
                user.password = Crypto.Hash(newpassword, "MD5");
                db.SaveChanges();

                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "nckurumsalweb@gmail.com";
                WebMail.Password = "baris5252";
                WebMail.SmtpPort = 587;
                WebMail.Send(mail, "Admin Paneli Giriş Şifreniz", "Şifreniz: " + newpassword);

                ViewBag.Warning = "Şifreniz  başarılı bir şeklilde gönderilmiştir .";

            }
            else
            {

                ViewBag.Warning = "Hata oluştu lütfen tekrar deneyiniz !";
            }


            return View();
        }
        public ActionResult Users()
        {
            return View(db.Admin.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Admin admin,string password,string mail)
        {
            if (ModelState.IsValid == true)
            {
                admin.password = Crypto.Hash(password, "MD5");
                db.Admin.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Users");
            }
            return View(admin);
        }
        public ActionResult Edit(int id)
        {
            var admins = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();
            return View(admins);
        }
        [HttpPost]
        public ActionResult Edit(int id,Admin admin,string password,string mail)
        {
            if (ModelState.IsValid == true)
            {
                var admins = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();
                admins.password = Crypto.Hash(password, "MD5");
                admins.Mail = admin.Mail;
                admins.Permission = admin.Permission;
                db.SaveChanges();
                return RedirectToAction("Users");

            }
            return View();
        }
        public ActionResult Delete(int id)
        {
            var admin = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();
            if (admin != null)
            {
                db.Admin.Remove(admin);
                db.SaveChanges();
                return RedirectToAction("Users");

            }
            return View();
        }
    }
}