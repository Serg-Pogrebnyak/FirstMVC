using static BLL.Interfaces.ICategoryService;

namespace TestMVC.ViewModels
{
    public class PaginationProductViewModel
    {
        public ProductForDisplayViewModel[] ProductArray { get; set; }

        public int PriceFrom { get; set; }

        public int PriceTo { get; set; }

        public int Sort { get; set; }

        public SortByEnum SortBy
        {
            get
            {
                return (SortByEnum)this.Sort;
            }
        }

        public int CurrentPage { get; set; }

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