using System;
using System.Threading.Tasks;
using CustomIdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using BLL.DTO;
using AutoMapper;
using System.Collections.Generic;
using testMVC.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace testMVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<User> _userManager;
        public BasketController(UserManager<User> userManager, IOrderService orderService)
        {
            _userManager = userManager;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            User user = await GetCurrentUserAsync();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, ProductViewModel>()).CreateMapper();

            List<ProductViewModel> orderList = new List<ProductViewModel> { };
            if (user != null)
            {
                ViewBag.TotalAmount = await _orderService.GetOrderTotalAmount(user.Id);
                orderList = mapper.Map<IEnumerable<ProductDTO>, List<ProductViewModel>>(await _orderService.GetAllProductsInBasket(userId: user.Id));
                return View(orderList);
            } else if (HttpContext.Session.Keys.Contains("basket"))
            {
                String cocieBasket = HttpContext.Session.GetString("basket");
                ViewBag.TotalAmount = await _orderService.GetOrderTotalAmount(basketInCache: cocieBasket);
                orderList = mapper.Map<IEnumerable<ProductDTO>, List<ProductViewModel>>(await _orderService.GetAllProductsInBasket(basketInCache: cocieBasket));
                return View(orderList);
            }

            //if user not sign-in and don't add product in basket
            ViewBag.TotalAmount = 0;
            return View(orderList);
        }

        private async Task<User> GetCurrentUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
    }
}