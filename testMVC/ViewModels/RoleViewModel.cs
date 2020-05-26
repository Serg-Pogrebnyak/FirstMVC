using System.ComponentModel.DataAnnotations;

namespace TestMVC.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }
    }
}