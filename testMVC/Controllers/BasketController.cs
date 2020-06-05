using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestMVC.Extensions;
using TestMVC.Models;
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
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductInBasketDTO, ProductInBasketViewModel>()).CreateMapper();

            List<ProductInBasketViewModel> orderList = new List<ProductInBasketViewModel> { };
            if (user != null)
            {
                this.ViewBag.TotalAmount = this.orderService.GetOrderTotalAmount(user.Id);
                orderList = mapper.Map<IEnumerable<ProductInBasketDTO>, List<ProductInBasketViewModel>>(this.orderService.GetAllProductsInBasket(userId: user.Id));
                return this.View(orderList);
            }
            else if (this.HttpContext.IsContainBasket())
            {
                string cocieBasket = this.HttpContext.GetBasket();
                this.ViewBag.TotalAmount = this.orderService.GetOrderTotalAmount(basketInCache: cocieBasket);
                orderList = mapper.Map<IEnumerable<ProductInBasketDTO>, List<ProductInBasketViewModel>>(this.orderService.GetAllProductsInBasket(basketInCache: cocieBasket));
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
                this.orderService.DeleteProductFromBasket(productId, userId: user.Id);
            }
            else if (this.HttpContext.IsContainBasket())
            {
                string cocieBasket = this.HttpContext.GetBasket();
                string basketCockie = this.orderService.DeleteProductFromBasket(productId, basketInCache: cocieBasket);
                this.HttpContext.SetBasket(basketCockie);
            }

            return this.RedirectToAction("Index");
        }

        public async Task<ActionResult> MakeOrder()
        {
            User user = await this.GetCurrentUserAsync();
            if (user != null)
            {
                this.orderService.MakeOrder(user.Id);
            }
            else
            {
                this.HttpContext.RemoveBasketFromSession();
            }

            return this.View("CongratulationView");
        }

        private async Task<User> GetCurrentUserAsync() => await this.userManager.GetUserAsync(this.HttpContext.User);
    }
}