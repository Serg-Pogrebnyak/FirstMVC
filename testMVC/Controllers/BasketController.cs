using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomIdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DAL.Interfaces;
using DAL.Entities;

namespace testMVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly IUnitOfWork db;
        private readonly UserManager<User> _userManager;
        public BasketController(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            db = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
                User user = await GetCurrentUserAsync();
                int coockieId = int.Parse(HttpContext.Request.Cookies["BasketId"]);

                int basketId;
                if (user != null)
                {
                    basketId = int.Parse(user.Id);
                }
                else
                {
                    basketId = coockieId;
                }

            
                Basket currentUserBasket = db.Basket.Get(basketId);

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
                        var product = db.Product.Get(id);
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

        private async Task<User> GetCurrentUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
    }
}