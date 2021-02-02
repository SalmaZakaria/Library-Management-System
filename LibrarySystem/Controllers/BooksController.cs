using LibrarySystem.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibrarySystem.Controllers
{
    public class BooksController : Controller
    {
        private Context context;
        private IWebHostEnvironment webHostEnvironment;
        public BooksController(Context ctx, IWebHostEnvironment webHostEnv)
        {
            context = ctx;
            webHostEnvironment = webHostEnv;
        }
        [HttpGet]
        public ActionResult AddBooks()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBooks(Book B)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;
                Guid ImageGuid = Guid.NewGuid();
                string extension = Path.GetExtension(B.ImageFile.FileName);
                string imageFullName = ImageGuid + extension;

                B.ImageName = imageFullName;
                string imagePath = wwwRootPath + "/images/" + imageFullName;
                using (FileStream fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    B.ImageFile.CopyTo(fileStream);
                }
                context.Books.Add(B);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
       
        public ActionResult Index()
        {
            return View(context.Books);
        }
        [HttpGet]
        public ActionResult Index(string SearchText, string sort)
        {
            if (sort == "asc")
            {
                ViewBag.SortA = true;
                return View(context.Books.OrderBy(B => B.Name));
            }
            else if (sort == "desc")
            {
                ViewBag.SortD = true;
                return View(context.Books.OrderByDescending(B => B.Name));
            }
            else if (string.IsNullOrEmpty(SearchText))
                return View(context.Books.Take(2));
            else
            {
                ViewBag.Search = SearchText.Trim();
                return View(context.Books.Where(B => B.Name.Contains(SearchText.Trim())));
            }
            
        }
        public ActionResult Details(int id)
        {
            Book B = context.Books.FirstOrDefault(b => b.ID == id);
            if (B == null)
                return NotFound();
            return View(B);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {           
            Book B = context.Books.FirstOrDefault(b => b.ID == id);
            if (B == null)
                return NotFound();
            return View(B);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Book B)
        {
            if (id != B.ID)
                return BadRequest();
            if (ModelState.IsValid)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;
                Guid ImageGuid = Guid.NewGuid();
                string extension = Path.GetExtension(B.ImageFile.FileName);
                string imageFullName = ImageGuid + extension;

                B.ImageName = imageFullName;
                string imagePath = wwwRootPath + "/images/" + imageFullName;
                using (FileStream fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    B.ImageFile.CopyTo(fileStream);
                }
                context.Books.Update(B);                          
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Book B = context.Books.FirstOrDefault(b => b.ID == id);
            if (B == null)
                return NotFound();
            return View(B);
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book B = context.Books.FirstOrDefault(b => b.ID == id);
            context.Books.Remove(B);
            context.SaveChanges();
            string wwwRoot = webHostEnvironment.WebRootPath;
            string imagePath = wwwRoot + "/images/" + B.ImageName;
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
            return RedirectToAction("Index", "Home");
        }
    }
}
