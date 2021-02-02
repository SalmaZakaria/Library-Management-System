using LibrarySystem.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibrarySystem.Controllers
{
    public class UsersController : Controller
    {
        private Context context;
        private IWebHostEnvironment webHostEnvironment;
        public UsersController(Context ctx, IWebHostEnvironment webHostEnv)
        {
            context = ctx;
            webHostEnvironment = webHostEnv;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User U)
        {
            User user = context.Users.FirstOrDefault(u => u.Email == U.Email && u.Password == U.Password);
            if (user == null)
            {
                ViewBag.Error = "Invalid Email or Password";
                return View("Login");
            }
            else
            {
                return RedirectToAction("YourBooks", new { id = user.ID});
            }
        }
        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.AllBooks = new SelectList(context.Books, "ID", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User U)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;
                Guid ImageGuid = Guid.NewGuid();
                string extension = Path.GetExtension(U.ImageFile.FileName);
                string imageFullName = ImageGuid + extension;

                U.ImageName = imageFullName;
                string imagePath = wwwRootPath + "/images/" + imageFullName;
                using (FileStream fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    U.ImageFile.CopyTo(fileStream);
                }
                context.Users.Add(U);
                Book book = context.Books.FirstOrDefault(B => B.ID == U.BookID);
                book.Count--;
                context.SaveChanges();
                return RedirectToAction("YourBooks", new { id = U.ID });
            }
            ViewBag.AllBooks = new SelectList(context.Books, "ID", "Name");
            return View();
        }
        public ActionResult YourBooks(int id)
        {
            User U = context.Users.Include(b => b.book).FirstOrDefault(u => u.ID == id);
            if (U == null)
                return NotFound();
            return View(U);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.AllBooks = new SelectList(context.Books, "ID", "Name");
            User U = context.Users.FirstOrDefault(u => u.ID == id);
            if (U == null)
                return NotFound();

            return View(U);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, User U)
        {
            if (id != U.ID)
                return BadRequest();
            if (ModelState.IsValid)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;
                Guid ImageGuid = Guid.NewGuid();
                string extension = Path.GetExtension(U.ImageFile.FileName);
                string imageFullName = ImageGuid + extension;

                U.ImageName = imageFullName;
                string imagePath = wwwRootPath + "/images/" + imageFullName;
                using (FileStream fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    U.ImageFile.CopyTo(fileStream);
                }
                Book book1 = context.Books.FirstOrDefault(B => B.ID == U.BookID);
                context.Users.Update(U);
                Book book2 = context.Books.FirstOrDefault(B => B.ID == U.BookID);
                if (book1 != book2)
                    book2.Count--;
                context.SaveChanges();
                return RedirectToAction("YourBooks", new { id = U.ID});
            }           
            return View();
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {        
            User U = context.Users.Include(b => b.book).FirstOrDefault(u => u.ID == id);
            if (U == null)
                return NotFound();
            return View(U);
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User U = context.Users.FirstOrDefault(u => u.ID == id);
            context.Users.Remove(U);
            context.SaveChanges();
            string wwwRoot = webHostEnvironment.WebRootPath;
            string imagePath = wwwRoot + "/images/" + U.ImageName;
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
            return RedirectToAction("Index", "Home");
        }
    }
}
