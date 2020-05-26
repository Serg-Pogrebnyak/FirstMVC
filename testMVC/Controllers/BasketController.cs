using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using CustomIdentityApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestMVC.ViewModels;

namespace TestMVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly IOrderService orderService;
        private readonly UserManager<User> userManager;

        public BasketController(UserManager<User> userManager, IOrderService orderService)
        {
            this.userManager = userManager;
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            User user = await this.GetCurrentUserAsync();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, ProductInBasketViewModel>()).CreateMapper();

            List<ProductInBasketViewModel> orderList = new List<ProductInBasketViewModel> { };
            if (user != null)
            {
                this.ViewBag.TotalAmount = this.orderService.GetOrderTotalAmount(user.Id);
                orderList = mapper.Map<IEnumerable<ProductDTO>, List<ProductInBasketViewModel>>(this.orderService.GetAllProductsInBasket(userId: user.Id));
                return this.View(orderList);
            }
            else if (this.HttpContext.Session.Keys.Contains("basket"))
            {
                string cocieBasket = this.HttpContext.Session.GetString("basket");
                this.ViewBag.TotalAmount = this.orderService.GetOrderTotalAmount(basketInCache: cocieBasket);
                orderList = mapper.Map<IEnumerable<ProductDTO>, List<ProductInBasketViewModel>>(this.orderService.GetAllProductsInBasket(basketInCache: cocieBasket));
                return this.View(orderList);
            }

            // if user not sign-in and don't add product in basket
            this.ViewBag.TotalAmount = 0;
            return this.View(orderList);
        }

        public async Task<ActionResult> Delete(int productId)
        {
            User user = await this.GetCurrentUserAsync();
            if (user != null)
            {
                this.orderService.deleteProductFromBasket(productId, userId: user.Id);
            }
            else if (this.HttpContext.Session.Keys.Contains("basket"))
            {
                string cocieBasket = this.HttpContext.Session.GetString("basket");
                string basketCockie = this.orderService.deleteProductFromBasket(productId, basketInCache: cocieBasket);
                this.HttpContext.Session.SetString("basket", basketCockie);
            }

            return this.RedirectToAction("Index");
        }

        private async Task<User> GetCurrentUserAsync() => await this.userManager.GetUserAsync(this.HttpContext.User);
    }
}