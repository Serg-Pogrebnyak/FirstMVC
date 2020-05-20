using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomIdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DAL.Interfaces;
using DAL.Entities;
using BLL.Interfaces;

namespace testMVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly IOrderService orderService;
        private readonly UserManager<User> _userManager;
        public BasketController(UserManager<User> userManager, IOrderService unitOfWork)
        {
            _userManager = userManager;
            orderService = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            User user = await GetCurrentUserAsync();
            String coockieId = HttpContext.Request.Cookies["BasketId"];

            String basketId = user != null ? user.Id : coockieId;

            ViewBag.TotalAmount = orderService.GetOrderTotalAmount(basketId);
            return View(orderService.GetAllProductsInBasket(basketId));
        }

        private async Task<User> GetCurrentUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
    }
}