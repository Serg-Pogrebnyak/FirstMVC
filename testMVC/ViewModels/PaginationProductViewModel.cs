namespace TestMVC.ViewModels
{
    public class PaginationProductViewModel
    {
        public ProductForDisplayViewModel[] ProductArray { get; set; }

        public int PriceFrom { get; set; }

        public int PriceTo { get; set; }

        public int Sort { get; set; }
    }
}