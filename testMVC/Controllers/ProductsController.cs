using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestMVC.Extensions;
using TestMVC.Models;
using TestMVC.ViewModels;
using static BLL.Interfaces.ICategoryService;

namespace TestMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly ILogger logger;
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        private readonly IOrderService orderService;

        public ProductsController(IOrderService orderService, UserManager<User> userManager, ILogger<ProductsController> logger, IProductService productService, ICategoryService categoryService)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.productService = productService;
            this.categoryService = categoryService;
            this.orderService = orderService;
        }

        [HttpGet]
        [Route("Products")]
        [Route("Products/Index")]
        public IActionResult Index()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, ProductForDisplayViewModel>()).CreateMapper();
            var productList = mapper.Map<IEnumerable<ProductDTO>, List<ProductForDisplayViewModel>>(this.productService.GetAllProduct());

            return this.View(productList);
        }

        [HttpGet]
        [Route("Products/Index/{id?}")]
        public IActionResult Index(string id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, ProductForDisplayViewModel>()).CreateMapper();
            var productList = mapper.Map<IEnumerable<ProductDTO>, List<ProductForDisplayViewModel>>(this.categoryService.GetAllProductInCategory(id));

            return this.View(productList);
        }

        [HttpPost]
        public IActionResult Index(int id, int priceFrom, int priceTo, int sort)
        {
            SortByEnum by = (SortByEnum)sort;
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, ProductForDisplayViewModel>()).CreateMapper();
            var productList = mapper.Map<IEnumerable<ProductDTO>, List<ProductForDisplayViewModel>>(this.categoryService.SelectProduct(id, priceFrom, priceTo, by));

            return this.View(productList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CategoriesDTO, CategoriesForDisplayViewModel>()).CreateMapper();
            var categoryList = mapper.Map<IEnumerable<CategoriesDTO>, List<CategoriesForDisplayViewModel>>(this.categoryService.GetAllCategory());

            this.ViewBag.Categories = categoryList;
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel newProduct)
        {
            if (this.ModelState.IsValid)
            {
                if (this.categoryService.ContainCategoryWithName(newProduct.Category))
                {
                    ProductDTO productDTO = new ProductDTO
                        {
                            Name = newProduct.Name,
                            Price = newProduct.Price,
                            Description = newProduct.Description
                        };
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(newProduct.File.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)newProduct.File.Length);
                    }

                    productDTO.ImageInByte = imageData;
                    this.productService.CreateNewProduct(productDTO, newProduct.Category);
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Error in cloud - Selected category not found");
                    this.ViewBag.Categories = this.categoryService.GetAllCategory();
                    return this.View();
                }

                return this.RedirectToAction("Index");
            }
            else
            {
                this.ViewBag.Categories = this.categoryService.GetAllCategory();
                return this.View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int productId)
        {
            User user = await this.GetCurrentUserAsync();
            if (user != null)
            {
                this.orderService.AddProductInBasket(productId, userId: user.Id);
            }
            else
            {
                string basketCockie = this.orderService.AddProductInBasket(productId, basketInCache: this.HttpContext.GetBasket());
                this.HttpContext.SetBasket(basketCockie);
            }

            return this.Redirect(this.Request.Headers["Referer"].ToString());
        }

        private async Task<User> GetCurrentUserAsync() => await this.userManager.GetUserAsync(this.HttpContext.User);
    }
}