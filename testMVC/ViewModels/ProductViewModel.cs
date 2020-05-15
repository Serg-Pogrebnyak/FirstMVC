using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using testMVC.Models;

namespace testMVC.ViewModels
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
        public String Category { get; set; }
    }
}
