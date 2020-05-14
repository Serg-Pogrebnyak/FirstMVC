using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace testMVC.ViewModels
{
    public class CategoriesViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }
    }
}
