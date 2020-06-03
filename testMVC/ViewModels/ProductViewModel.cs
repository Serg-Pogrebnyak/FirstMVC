using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TestMVC.ViewModels
{
    public class ProductViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Price")]
        public int Price { get; set; }

        [Display(Name = "Shord Description")]
        public string Description { get; set; }

        [Display(Name = "Long Description")]
        public string LongDescription { get; set; }

        [Required]
        [Display(Name = "Add in category:")]
        public string Category { get; set; }

        [Required]
        [Display(Name = "Choose product image (image should be 286x180px)")]
        public IFormFile File { get; set; }

        public string ReturnURL { get; set; }
    }
}