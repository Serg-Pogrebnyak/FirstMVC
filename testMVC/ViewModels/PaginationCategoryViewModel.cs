using System.Collections.Generic;

namespace TestMVC.ViewModels
{
    public class PaginationCategoryViewModel
    {
        public int CurrentPage { get; set; }

        public IEnumerable<CategoriesForDisplayViewModel> CategoryList { get; set; }

        public bool HasPrevious
        {
            get
            {
                return this.CurrentPage > 0;
            }
        }

        public bool HasNext { get; set; }
    }
}