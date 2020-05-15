﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using testMVC.DataBase;
using testMVC.Models;
using testMVC.ViewModels;

namespace testMVC.Controllers
{
    public class ProductsController : Controller
    {
        [HttpGet]
        [Route("Products")]
        [Route("Products/Index")]
        public IActionResult Index()
        {
            using (DBContext db = new DBContext())
            {
                return View(db.Products.ToArray());
            }
        }

        [HttpGet]
        [Route("Products/Index/{id?}")]
        public IActionResult Index(int id)
        {
            using (DBContext db = new DBContext())
            {
                Categories category = db.Categories.SingleOrDefault(e => e.Id == id);
                if (category != null)
                {
                    db.Entry(category).Collection(c => c.Products).Load();
                    return View(category.Products.ToArray());
                } else
                {
                    return RedirectToAction("Index");
                }
                
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            using (DBContext db = new DBContext())
            { 
                SelectList categories = new SelectList(db.Categories, "Name");
                ViewBag.Categories = db.Categories.ToArray();
                return View();
            }
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel newProduct)
        {
            if (ModelState.IsValid)
            {
                using (DBContext db = new DBContext())
                {
                    Categories SelectedCategory = db.Categories.SingleOrDefault(catgories => catgories.Name == newProduct.Category);
                    if (SelectedCategory != null)
                    {
                        Product product = new Product
                        {
                            Name = newProduct.Name,
                            Price = newProduct.Price,
                            Description = newProduct.Description,
                            Category = SelectedCategory
                        };
                        db.Products.Add(product);
                        db.SaveChanges();
                    } else
                    {
                        ModelState.AddModelError("", "Error in cloud - Selected category not found");
                        ViewBag.Categories = db.Categories.ToArray();
                        return View();
                    }
                }
                return RedirectToAction("Index");
            } else
            {
                using (DBContext db = new DBContext())
                {
                    SelectList categories = new SelectList(db.Categories, "Name");
                    ViewBag.Categories = db.Categories.ToArray();
                    return View();
                }
            }
        }
    }
}
