using System.ComponentModel.DataAnnotations;

namespace TestMVC.ViewModels
{
    public class ProductViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Цена")]
        public int Price { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "В какую категорию добавить:")]
        public string Category { get; set; }
    }
}