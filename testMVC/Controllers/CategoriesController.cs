using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using testMVC.DataBase;
using testMVC.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace testMVC.Controllers
{
    public class CategoriesController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            using (DBContext db = new DBContext())
            {
                Categories[] categories = db.Categoies.ToArray();
                return View(categories);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create() => View();

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(string name)
        {
            using (DBContext db = new DBContext())
            {
                Categories newCategories = new Categories { Name = name };
                db.Categoies.Add(newCategories);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
