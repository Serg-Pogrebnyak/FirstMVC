using static BLL.Interfaces.ICategoryService;

namespace BLL.DTO
{
    public class SelectingSortingProductCriteriaBLL
    {
        public int PriceFrom { get; set; }

        public int PriceTo { get; set; }

        public SortByEnum SortBy { get; set; }

        public int CurrentPage { get; set; }
    }
}