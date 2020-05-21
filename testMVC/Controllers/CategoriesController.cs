using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using BLL.DTO;
using AutoMapper;
using System.Collections.Generic;
using testMVC.ViewModels;

namespace testMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CategoriesDTO, CategoriesForDisplayViewModel>()).CreateMapper();
            var categoryList = mapper.Map<IEnumerable<CategoriesDTO>, List<CategoriesForDisplayViewModel>>(_categoryService.getAllCategory());

            return View(categoryList);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create() => View();

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(string name)
        {
            _categoryService.createNewCategory(name);
            return RedirectToAction("Index");
        }
    }
}
