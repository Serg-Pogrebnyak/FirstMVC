using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using AutoMapper;
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

        public string AddProductInBasket(int productId, int count, string userId = null, string basketInCache = null)
        {
            if (userId != null)
            {
                this.AddProductInDBBasket(userId, productId, count);
                return null;
            }
            else
            {
                return this.AddProductInCacheBasket(productId, count, basketInCache);
            }
        }

        public IEnumerable<ProductDTO> GetAllProductsInBasket(string userId = null, string basketInCache = null)
        {
            if (userId != null)
            {
                Basket basket = this.GetBasketByUserId(userId);
                if (basket == null)
                {
                    return new List<ProductDTO> { };
                }

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductInBasket, ProductCache>()).CreateMapper();
                var productInBasketList = mapper.Map<IEnumerable<ProductInBasket>, List<ProductCache>>(basket.Products);

                return this.MapperGetProductsFromBasket(productInBasketList);
            }
            else
            {
                BasketCache basketCache = JsonSerializer.Deserialize<BasketCache>(basketInCache);
                return this.MapperGetProductsFromBasket(basketCache.Products);
            }
        }

        public int GetOrderTotalAmount(string userId = null, string basketInCache = null)
        {
            if (userId != null)
            {
                Basket basket = this.GetBasketByUserId(userId);
                if (basket == null)
                {
                    return 0;
                }

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductInBasket, ProductCache>()).CreateMapper();
                var productInBasketList = mapper.Map<IEnumerable<ProductInBasket>, List<ProductCache>>(basket.Products);

                return this.MapperTotalAmountFromBasket(productInBasketList);
            }
            else
            {
                BasketCache basketCache = JsonSerializer.Deserialize<BasketCache>(basketInCache);
                return this.MapperTotalAmountFromBasket(basketCache.Products);
            }
        }

        public string DeleteProductFromBasket(int productId, string userId = null, string basketInCache = null)
        {
            if (userId != null)
            {
                Basket basket = this.GetBasketByUserId(userId);
                ProductInBasket productInBasket = basket.Products.SingleOrDefault(p => p.ProductId == productId);
                this.db.Repository.Delete<ProductInBasket>(productInBasket.Id);
                if (basket.Products.Count == 0)
                {
                    this.db.Repository.Delete<Basket>(basket.Id);
                }

                this.db.Save();
                return null;
            }
            else
            {
                BasketCache basketCache = JsonSerializer.Deserialize<BasketCache>(basketInCache);
                ProductCache productCache = basketCache.Products.SingleOrDefault(p => p.ProductId == productId);
                int index = basketCache.Products.IndexOf(productCache);
                basketCache.Products.RemoveAt(index);
                return JsonSerializer.Serialize<BasketCache>(basketCache);
            }
        }

        public void MigrateBasketFromCookie(string userId, string basketInCache)
        {
            BasketCache basketCache = JsonSerializer.Deserialize<BasketCache>(basketInCache);
            foreach (ProductCache product in basketCache.Products)
            {
                this.AddProductInDBBasket(userId, product.ProductId, product.ProductCount);
            }
        }

        // private function
        private IEnumerable<ProductDTO> MapperGetProductsFromBasket(IEnumerable<ProductCache> productIdArray)
        {
            List<ProductDTO> productsInBasketList = new List<ProductDTO> { };
            foreach (ProductCache productCache in productIdArray)
            {
                Product product = this.db.Repository.Get<Product>(productCache.ProductId);
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

        private int MapperTotalAmountFromBasket(IEnumerable<ProductCache> productIdArray)
        {
            int totalAmount = 0;
            foreach (ProductCache productCache in productIdArray)
            {
                totalAmount += this.db.Repository.Get<Product>(productCache.ProductId).Price * productCache.ProductCount;
            }

            return totalAmount;
        }

        // add product in local basket or cache
        private void AddProductInDBBasket(string userId, int productId, int count)
        {
            Basket basket = this.GetBasketByUserId(userId);
            if (basket == null)
            {
                Basket newBasket = new Basket
                {
                    UserId = userId.ToString()
                };
                this.db.Repository.Create(newBasket);
                this.db.Save();
                ProductInBasket productInBasket = new ProductInBasket
                {
                    ProductId = productId,
                    ProductCount = count,
                    Basket = newBasket
                };
                this.db.Repository.Create(productInBasket);
            }
            else
            {
                ProductInBasket productInBasket = basket.Products.SingleOrDefault(p => p.ProductId == productId);
                if (productInBasket == null)
                {
                    productInBasket = new ProductInBasket
                    {
                        ProductId = productId,
                        ProductCount = count,
                        Basket = basket
                    };
                    this.db.Repository.Create(productInBasket);
                }
                else
                {
                    productInBasket.ProductCount += count;
                    this.db.Repository.Update(productInBasket);
                }
            }

            this.db.Save();
        }

        private string AddProductInCacheBasket(int productId, int count, string basketInCache)
        {
            BasketCache basketCache;
            if (basketInCache == null)
            {
                ProductCache productCache = new ProductCache
                {
                    ProductId = productId,
                    ProductCount = count
                };
                basketCache = new BasketCache
                {
                    Products = new List<ProductCache>() { productCache }
                };
            }
            else
            {
                basketCache = JsonSerializer.Deserialize<BasketCache>(basketInCache);

                ProductCache productCache = basketCache.Products.SingleOrDefault(p => p.ProductId == productId);
                if (productCache == null)
                {
                    productCache = new ProductCache
                    {
                        ProductId = productId,
                        ProductCount = count
                    };
                    basketCache.Products.Add(productCache);
                }
                else
                {
                    productCache.ProductCount += count;
                }
            }

            return JsonSerializer.Serialize<BasketCache>(basketCache);
        }

        private Basket GetBasketByUserId(string userId)
        {
            return this.db.Repository.GetAll<Basket>().SingleOrDefault(basket => basket.UserId == userId);
        }
    }
}