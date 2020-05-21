using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DAL.Interfaces;
using DAL.Entities;
using testMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using CustomIdentityApp.Models;
using BLL.Interfaces;

namespace testMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly IUnitOfWork db;
        private readonly ICategoryService _categoryService;
        private readonly IOrderService _orderService;

        public ProductsController(IOrderService orderService, UserManager<User> userManager, ILogger<ProductsController> logger, IUnitOfWork unitOfWork, ICategoryService categoryService)
        {
            _userManager = userManager;
            _logger = logger;
            db = unitOfWork;
            _categoryService = categoryService;
            _orderService = orderService;
        }

        [HttpGet]
        [Route("Products")]
        [Route("Products/Index")]
        public IActionResult Index()
        {
            
            
                return View(db.Product.GetAll());
            
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
            
             
                //SelectList categories = new SelectList(db.Categories.GetAll(), "Name");
                ViewBag.Categories = db.Categories.GetAll();
                return View();
            
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel newProduct)
        {
            if (ModelState.IsValid)
            {


                //Categories SelectedCategory = db.Categories.Get(catgories => catgories.Name == newProduct.Category);
                Categories SelectedCategory = db.Categories.Get(0);// - fix this
                if (SelectedCategory != null)
                    {
                        Product product = new Product
                        {
                            Name = newProduct.Name,
                            Price = newProduct.Price,
                            Description = newProduct.Description,
                            Category = SelectedCategory
                        };
                        db.Product.Create(product);
                        db.Save();
                    } else
                    {
                        ModelState.AddModelError("", "Error in cloud - Selected category not found");
                    ViewBag.Categories = db.Categories.GetAll();
                        return View();
                    }
                
                return RedirectToAction("Index");
            } else
            {
                
                
                    //SelectList categories = new SelectList(db.Categories, "Name");
                    ViewBag.Categories = db.Categories.GetAll();
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
