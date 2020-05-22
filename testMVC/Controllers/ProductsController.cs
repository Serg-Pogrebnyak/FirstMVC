using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using testMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using CustomIdentityApp.Models;
using BLL.Interfaces;
using BLL.DTO;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace testMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IOrderService _orderService;

        public ProductsController(IOrderService orderService, UserManager<User> userManager, ILogger<ProductsController> logger, IProductService productService, ICategoryService categoryService)
        {
            _userManager = userManager;
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _orderService = orderService;
        }

        [HttpGet]
        [Route("Products")]
        [Route("Products/Index")]
        public IActionResult Index()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, ProductForDisplayViewModel>()).CreateMapper();
            var productList = mapper.Map<IEnumerable<ProductDTO>, List<ProductForDisplayViewModel>>(_productService.getAllProduct());

            return View(productList);
        }

        [HttpGet]
        [Route("Products/Index/{id?}")]
        public IActionResult Index(int id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, ProductForDisplayViewModel>()).CreateMapper();
            var productList = mapper.Map<IEnumerable<ProductDTO>, List<ProductForDisplayViewModel>>(_categoryService.getAllProductInCategory(id));

            return View(productList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CategoriesDTO, CategoriesForDisplayViewModel>()).CreateMapper();
            var categoryList = mapper.Map<IEnumerable<CategoriesDTO>, List<CategoriesForDisplayViewModel>>(_categoryService.getAllCategory());

            ViewBag.Categories = categoryList;
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel newProduct)
        {
            if (ModelState.IsValid)
            {
                if (_categoryService.containCategoryWithName(newProduct.Category))
                {
                    ProductDTO productDTO = new ProductDTO
                        {
                            Name = newProduct.Name,
                            Price = newProduct.Price,
                            Description = newProduct.Description
                        };
                    _productService.createNewProduct(productDTO, newProduct.Category);
                } else
                {
                    ModelState.AddModelError("", "Error in cloud - Selected category not found");
                    ViewBag.Categories = _categoryService.getAllCategory();
                    return View();
                }
                return RedirectToAction("Index");
            } else
            {
                ViewBag.Categories = _categoryService.getAllCategory();
                return View();
                
            }
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int productId)
        {
            User user = await GetCurrentUserAsync();
            if (user != null)
            {
                _orderService.addProductInBasketAsync(productId, userId: user.Id);
            } else
            {
                string basketCockie = await _orderService.addProductInBasketAsync(productId, basketInCache: HttpContext.Session.GetString("basket"));
                HttpContext.Session.SetString("basket", basketCockie);
            }
            
            return Redirect(Request.Headers["Referer"].ToString());
        }
        private async Task<User> GetCurrentUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
    }
}
