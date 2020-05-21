using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using testMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using CustomIdentityApp.Models;
using BLL.Interfaces;
using BLL.DTO;

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
            return View(_productService.getAllProduct());
        }

        [HttpGet]
        [Route("Products/Index/{id?}")]
        public IActionResult Index(int id)
        {
            return View(_categoryService.getAllProductInCategory(id));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _categoryService.getAllCategory();
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
            var coockieId = HttpContext.Request.Cookies["BasketId"];

            string basketId = user != null ? user.Id : coockieId;
            _orderService.addProductInBasket(basketId, productId);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        private async Task<User> GetCurrentUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
    }
}
