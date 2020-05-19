using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomIdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using testMVC.DataBase;
using testMVC.Models;

namespace testMVC.Controllers
{
    public class BasketController : Controller
    {

        private readonly UserManager<User> _userManager;
        public BasketController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
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
                    ViewBag.TotalAmount = 0;
                    return View(new Product[] { });
                }
                else
                {
                    List<Product> productList = new List<Product>();
                    int totalAmount = 0;
                    foreach (int id in currentUserBasket.ProductsId)
                    {
                        var product = db.Products.SingleOrDefault(product => product.Id == id);
                        if (product != null)
                        {
                            totalAmount += product.Price;
                            productList.Add(product);
                        }
                    }
                    ViewBag.TotalAmount = totalAmount;
                    return View(productList);
                }
            }
        }

        private async Task<User> GetCurrentUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
    }
}