using System.ComponentModel.DataAnnotations;

namespace testMVC.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }
    }
}
