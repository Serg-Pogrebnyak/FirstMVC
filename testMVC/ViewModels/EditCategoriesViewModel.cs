using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TestMVC.ViewModels
{
    public class EditCategoriesViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Tag for display in URL")]
        public string Tag { get; set; }

        [Display(Name = "Choose category image (image should be 286x180px)")]
        public IFormFile File { get; set; }

        [Display(Name = "Parent Category")]
        public string ParentCategory { get; set; }
    }
}