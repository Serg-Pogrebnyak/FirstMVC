using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TestMVC.ViewModels
{
    public class CategoriesViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Tag for display in URL")]
        public string Tag { get; set; }

        [Required]
        [Display(Name = "Choose category image (image should be 286x180px)")]
        public IFormFile File { get; set; }

        [Display(Name = "Parent Category")]
        public string ParentCategory { get; set; }
    }
}