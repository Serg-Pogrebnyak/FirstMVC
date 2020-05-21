using System;
using System.Threading.Tasks;
using CustomIdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;

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
            return View(_orderService.GetAllProductsInBasket(basketId));
        }

        private async Task<User> GetCurrentUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
    }
}