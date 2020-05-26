using System.ComponentModel.DataAnnotations;

namespace TestMVC.ViewModels
{
    public class CategoriesViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }
    }
}