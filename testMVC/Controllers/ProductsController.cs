using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestMVC.Extensions;
using TestMVC.Models;
using TestMVC.ViewModels;

namespace TestMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly int countOfElenetPerPage = 6;
        private readonly UserManager<User> userManager;
        private readonly IProductService productService;
        private readonly ILogger logger;
        private readonly ICategoryService categoryService;
        private readonly IOrderService orderService;

        public ProductsController(IOrderService orderService, UserManager<User> userManager, ILogger<ProductsController> logger, IProductService productService, ICategoryService categoryService)
        {
            this.userManager = userManager;
            this.productService = productService;
            this.categoryService = categoryService;
            this.orderService = orderService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("Products/Index/{id?}")]
        public IActionResult Index(string id, PaginationProductViewModel paginationModel)
        {
            if (paginationModel.Submit != null)
            {
                paginationModel.CurrentPage = paginationModel.Submit == "Next" ? paginationModel.CurrentPage + 1 : paginationModel.CurrentPage - 1;
            }

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PaginationProductViewModel, SelectingSortingProductCriteriaBLL>()).CreateMapper();
            var criteriaBLL = mapper.Map<PaginationProductViewModel, SelectingSortingProductCriteriaBLL>(paginationModel);
            var paginationTuple = this.categoryService.GetProductsByPageAndCountOfPages(id, this.countOfElenetPerPage, criteriaBLL);

            mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, ProductForDisplayViewModel>()).CreateMapper();
            var productList = mapper.Map<IEnumerable<ProductDTO>, List<ProductForDisplayViewModel>>(paginationTuple.elements);
            paginationModel.ProductArray = productList.ToArray();
            paginationModel.HasNext = paginationTuple.countOfPages > (paginationModel.CurrentPage + 1); // plus one because start calculating from zero

            return this.View(paginationModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CategoriesDTO, CategoriesForDisplayViewModel>()).CreateMapper();
            var categoryList = mapper.Map<IEnumerable<CategoriesDTO>, List<CategoriesForDisplayViewModel>>(this.categoryService.GetAllCategory());

            this.ViewBag.Categories = categoryList;
            return this.View();
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            this.logger.LogError(id.ToString());
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CategoriesDTO, CategoriesForDisplayViewModel>()).CreateMapper();
            var categoryList = mapper.Map<IEnumerable<CategoriesDTO>, List<CategoriesForDisplayViewModel>>(this.categoryService.GetAllCategory());

            this.ViewBag.Categories = categoryList;
            return this.View();
        }

        [HttpGet]
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