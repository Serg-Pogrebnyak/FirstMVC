using System.Collections.Generic;
using System.IO;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMVC.ViewModels;

namespace TestMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CategoriesDTO, CategoriesForDisplayViewModel>()).CreateMapper();
            var categoryList = mapper.Map<IEnumerable<CategoriesDTO>, List<CategoriesForDisplayViewModel>>(this.categoryService.GetAllCategory());

            return this.View(categoryList);
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
        public IActionResult Create(CategoriesViewModel model)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CategoriesViewModel, CategoriesDTO>()).CreateMapper();
            var newCategory = mapper.Map<CategoriesViewModel, CategoriesDTO>(model);

            var validationTuple = this.categoryService.IsContainCategoryWithNameAndTag(newCategory);
            if (this.ModelState.IsValid && validationTuple.isValid)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(model.File.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)model.File.Length);
                }

                newCategory.ImageInByte = imageData;
                this.categoryService.CreateNewCategory(newCategory);
                return this.RedirectToAction("Index");
            }
            else
            {
                if (validationTuple.textError != null)
                {
                    this.ModelState.AddModelError(string.Empty, validationTuple.textError);
                }

                mapper = new MapperConfiguration(cfg => cfg.CreateMap<CategoriesDTO, CategoriesForDisplayViewModel>()).CreateMapper();
                var categoryList = mapper.Map<IEnumerable<CategoriesDTO>, List<CategoriesForDisplayViewModel>>(this.categoryService.GetAllCategory());
                this.ViewBag.Categories = categoryList;
                return this.View();
            }
        }
    }
}