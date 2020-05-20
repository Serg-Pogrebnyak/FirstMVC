using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAL.Interfaces;
using DAL.Entities;

namespace testMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork db;
        public CategoriesController(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            Categories[] categories = db.Categories.GetAll().ToArray();
            return View(categories);   
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create() => View();

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(string name)
        {
            Categories newCategories = new Categories { Name = name };
            db.Categories.Create(newCategories);
            db.Save();
            
            return RedirectToAction("Index");
        }
    }
}
