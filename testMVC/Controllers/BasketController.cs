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
            String coockieId = HttpContext.Request.Cookies["BasketId"];

            String basketId = user != null ? user.Id : coockieId;

            ViewBag.TotalAmount = _orderService.GetOrderTotalAmount(basketId);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, ProductViewModel>()).CreateMapper();
            var orderList = mapper.Map<IEnumerable<ProductDTO>, List<ProductViewModel>>(_orderService.GetAllProductsInBasket(basketId));
            return View(orderList);
        }

        private async Task<User> GetCurrentUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
    }
}