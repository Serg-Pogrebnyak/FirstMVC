using System.Collections.Generic;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
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

        [HttpGet]
        public IActionResult Create() => this.View();

        [HttpPost]
        public IActionResult Create(string name)
        {
            this.categoryService.CreateNewCategory(name);
            return this.RedirectToAction("Index");
        }
    }
}