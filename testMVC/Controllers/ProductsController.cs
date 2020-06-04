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
        [Route("Products/Index/{tag}/{id?}")]
        public async Task<IActionResult> ProductDetail(string tag, int id)
        {
            this.logger.LogDebug(tag.ToString());
            ProductDTO productDTO = this.productService.GetProductById(id);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, ProductForDisplayViewModel>()).CreateMapper();
            var productDetailViewModel = mapper.Map<ProductDTO, ProductForDisplayViewModel>(productDTO);
            return this.View("ProductDetailView", productDetailViewModel);
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
            foreach (ProductForDisplayViewModel item in productList)
            {
                item.DetailURL = $"{id}/{item.Id}";
            }

            paginationModel.ProductArray = productList.ToArray();
            paginationModel.HasNext = paginationTuple.countOfPages > (paginationModel.CurrentPage + 1); // plus one because start calculating from zero
            return this.View(paginationModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ProductViewModel productViewModel = new ProductViewModel { ReturnURL = this.Request.Headers["Referer"].ToString() };

            this.ViewBag.Categories = this.GetAllCategoryForDisplay();
            return this.View(productViewModel);
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
                            Description = newProduct.Description,
                            LongDescription = newProduct.LongDescription
                        };
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(newProduct.File.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)newProduct.File.Length);
                    }

                    productDTO.ImageInByte = imageData;
                    this.productService.CreateNewProduct(productDTO, newProduct.Category);
                    return this.Redirect(newProduct.ReturnURL);
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Error in cloud - Selected category not found");
                    this.ViewBag.Categories = this.GetAllCategoryForDisplay();
                    return this.View();
                }
            }
            else
            {
                this.ViewBag.Categories = this.GetAllCategoryForDisplay();
                return this.View();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            this.ViewBag.Categories = this.GetAllCategoryForDisplay();

            ProductDTO productDTO = this.productService.GetProductById(id);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, EditProductViewModel>()).CreateMapper();
            var editProductViewModel = mapper.Map<ProductDTO, EditProductViewModel>(productDTO);
            editProductViewModel.ReturnURL = this.Request.Headers["Referer"].ToString();
            return this.View(editProductViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(EditProductViewModel changedProduct)
        {
            if (this.ModelState.IsValid)
            {
                if (this.categoryService.ContainCategoryWithName(changedProduct.Category))
                {
                    ProductDTO productDTO = new ProductDTO
                    {
                        Id = changedProduct.Id,
                        Name = changedProduct.Name,
                        Price = changedProduct.Price,
                        Description = changedProduct.Description,
                        LongDescription = changedProduct.LongDescription
                    };
                    if (changedProduct.File != null)
                    {
                        byte[] imageData = null;
                        using (var binaryReader = new BinaryReader(changedProduct.File.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)changedProduct.File.Length);
                        }

                        productDTO.ImageInByte = imageData;
                    }

                    this.productService.UpdateProduct(productDTO, changedProduct.Category);
                    return this.Redirect(changedProduct.ReturnURL);
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Error in cloud - Selected category not found");
                    this.ViewBag.Categories = this.categoryService.GetAllCategory();
                    return this.View(changedProduct);
                }
            }
            else
            {
                this.ViewBag.Categories = this.categoryService.GetAllCategory();
                return this.View(changedProduct);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Buy(int productId, int countOfProduct)
        {
            this.logger.LogError(countOfProduct.ToString());
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

        private CategoriesForDisplayViewModel[] GetAllCategoryForDisplay()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CategoriesDTO, CategoriesForDisplayViewModel>()).CreateMapper();
            return mapper.Map<IEnumerable<CategoriesDTO>, CategoriesForDisplayViewModel[]>(this.categoryService.GetAllCategory());
        }
    }
}