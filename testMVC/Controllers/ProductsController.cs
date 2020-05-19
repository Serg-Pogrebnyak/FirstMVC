using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using testMVC.DataBase;
using testMVC.Models;
using testMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using CustomIdentityApp.Models;
using System.Security.Claims;

namespace testMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        public ProductsController(UserManager<User> userManager, ILogger<ProductsController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

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

        [HttpPost]
        public async Task<IActionResult> Buy(int productId)
        {
            using (DBContext db = new DBContext())
            {
                User user = await GetCurrentUserAsync();
                var coockieId = HttpContext.Request.Cookies["BasketId"];

                string basketId;
                if (user != null)
                {
                    basketId = user.Id;
                }
                else
                {
                    basketId = coockieId;
                }

                Basket currentUserBasket = db.Baskets.SingleOrDefault(basket => basket.UserId == basketId);


                if (currentUserBasket == null)
                {
                    Basket newBasket = new Basket
                    {
                        UserId = basketId,
                        ProductsId = new List<int>() { productId }
                    };
                    db.Baskets.Add(newBasket);
                } else
                {
                    List<int> productList = currentUserBasket.ProductsId;
                    productList.Add(productId);
                    currentUserBasket.ProductsId = productList;
                }
                db.SaveChanges();
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }
        private async Task<User> GetCurrentUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
    }
}
