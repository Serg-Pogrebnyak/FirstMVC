using System.Collections.Generic;
using System.Text.Json;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.BusinessLogic
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork db;

        public OrderService(IUnitOfWork db)
        {
            this.db = db;
        }

        public string AddProductInBasket(int productId, string userId = null, string basketInCache = null)
        {
            if (userId != null)
            {
                this.AddProductInDBBasket(userId, productId);
                return null;
            }
            else
            {
                return this.AddProductInCacheBasket(productId, basketInCache);
            }
        }

        public IEnumerable<ProductDTO> GetAllProductsInBasket(string userId = null, string basketInCache = null)
        {
            if (userId != null)
            {
                Basket basket = this.db.Basket.GetByUserId(userId);
                if (basket == null)
                {
                    return new List<ProductDTO> { };
                }

                return this.MapperGetProductsFromBasket(basket.ProductsId);
            }
            else
            {
                BasketCache basketCache = JsonSerializer.Deserialize<BasketCache>(basketInCache);
                return this.MapperGetProductsFromBasket(basketCache.ProductsId);
            }
        }

        public int GetOrderTotalAmount(string userId = null, string basketInCache = null)
        {
            if (userId != null)
            {
                Basket basket = this.db.Basket.GetByUserId(userId);
                if (basket == null)
                {
                    return 0;
                }

                return this.MapperTotalAmountFromBasket(basket.ProductsId);
            }
            else
            {
                BasketCache basketCache = JsonSerializer.Deserialize<BasketCache>(basketInCache);
                return this.MapperTotalAmountFromBasket(basketCache.ProductsId);
            }
        }

        public string DeleteProductFromBasket(int productId, string userId = null, string basketInCache = null)
        {
            if (userId != null)
            {
                Basket basket = this.db.Basket.GetByUserId(userId);
                var productsList = basket.ProductsId;
                productsList.Remove(productId);
                if (productsList.Count == 0)
                {
                    this.db.Basket.Delete(basket.Id);
                }
                else
                {
                    basket.ProductsId = productsList;
                }

                this.db.Save();
                return null;
            }
            else
            {
                BasketCache basketCache = JsonSerializer.Deserialize<BasketCache>(basketInCache);
                basketCache.ProductsId.Remove(productId);
                return JsonSerializer.Serialize<BasketCache>(basketCache);
            }
        }

        public void MigrateBasketFromCookie(string userId, string basketInCache)
        {
            BasketCache basketCache = JsonSerializer.Deserialize<BasketCache>(basketInCache);
            foreach (int productId in basketCache.ProductsId)
            {
                this.AddProductInDBBasket(userId, productId);
            }
        }

        // private function
        private IEnumerable<ProductDTO> MapperGetProductsFromBasket(IEnumerable<int> productIdArray)
        {
            List<ProductDTO> productsInBasketList = new List<ProductDTO> { };
            foreach (int id in productIdArray)
            {
                Product product = this.db.Product.Get(id);
                ProductDTO productDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description
                };
                productsInBasketList.Add(productDTO);
            }

            return productsInBasketList;
        }

        private int MapperTotalAmountFromBasket(IEnumerable<int> productIdArray)
        {
            int totalAmount = 0;
            foreach (int id in productIdArray)
            {
                totalAmount += this.db.Product.Get(id).Price;
            }

            return totalAmount;
        }

        // add product in local basket or cache
        private void AddProductInDBBasket(string userId, int productId)
        {
            Basket basket = this.db.Basket.GetByUserId(userId);
            if (basket == null)
            {
                Basket newBasket = new Basket
                {
                    UserId = userId.ToString(),
                    ProductsId = new List<int>() { productId }
                };
                this.db.Basket.Create(newBasket);
            }
            else
            {
                List<int> productList = basket.ProductsId;
                productList.Add(productId);
                basket.ProductsId = productList;
            }

            this.db.Save();
        }

        private string AddProductInCacheBasket(int productId, string basketInCache)
        {
            BasketCache basketCache;
            if (basketInCache == null)
            {
                basketCache = new BasketCache
                {
                    ProductsId = new List<int>() { productId }
                };
            }
            else
            {
                basketCache = JsonSerializer.Deserialize<BasketCache>(basketInCache);
                basketCache.ProductsId.Add(productId);
            }

            return JsonSerializer.Serialize<BasketCache>(basketCache);
        }
    }
}