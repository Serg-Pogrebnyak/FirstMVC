using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Parent Category")]
        public string ParentCategory { get; set; }
    }
}