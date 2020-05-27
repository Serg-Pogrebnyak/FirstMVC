using System.Linq;
using Microsoft.AspNetCore.Http;

namespace TestMVC.Extensions
{
    public static class HttpContextExtensions
    {
        private static string BasketKey => "basket";

        public static bool IsContainBasket(this HttpContext context)
        {
            return context.Session.Keys.Contains(BasketKey);
        }

        public static string GetBasket(this HttpContext context)
        {
            return context.Session.GetString(BasketKey);
        }

        public static void SetBasket(this HttpContext context, string basket)
        {
            context.Session.SetString(BasketKey, basket);
        }

        public static void RemoveBasketFromSession(this HttpContext context)
        {
            context.Session.Remove(BasketKey);
        }
    }
}