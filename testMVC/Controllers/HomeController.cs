using System.Collections.Generic;
using System.Diagnostics;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TestMVC.Models;
using TestMVC.ViewModels;

namespace TestMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService productService;

        public HomeController(IProductService productService)
        {
            this.productService = productService;
        }

        public IActionResult Index()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, ProductForDisplayViewModel>()).CreateMapper();
            var productList = mapper.Map<IEnumerable<ProductDTO>, List<ProductForDisplayViewModel>>(this.productService.GetAllProduct());

            return this.View(productList);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}