using System;
using System.Collections.Generic;
using BLL.DTO;
using DAL.Entities;
using BLL.Interfaces;
using DAL.Interfaces;
using System.Threading.Tasks;
using System.Text.Json;

namespace BLL.BusinessLogic
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _db;
        public OrderService(IUnitOfWork db)
        {
            _db = db;
        } 
        public async Task<String> addProductInBasketAsync(int productId, String userId = null, String basketInCache = null)
        {
            if (userId != null)
            {
                await Task.Run(() => addProductInDBBasket(userId, productId));
                return null;
            } else
            {
                return await Task.Run(() => addProductInCacheBasket(productId, basketInCache));
            }
        }

        private void addProductInDBBasket(String userId, int productId)
        {
            Basket basket = _db.Basket.GetByUserId(userId);
            if (basket == null)
            {
                Basket newBasket = new Basket
                {
                    UserId = userId.ToString(),
                    ProductsId = new List<int>() { productId }
                };
                _db.Basket.Create(newBasket);
            }
            else
            {
                List<int> productList = basket.ProductsId;
                productList.Add(productId);
                basket.ProductsId = productList;
            }
            _db.Save();
        }

        private String addProductInCacheBasket(int productId, String basketInCache)
        {
            BasketCache basketCache;
            if (basketInCache == null)
            {
                basketCache = new BasketCache
                {
                    ProductsId = new List<int>() { productId }
                };
            } else
            {
                basketCache = JsonSerializer.Deserialize<BasketCache>(basketInCache);
                basketCache.ProductsId.Add(productId);
            }
            
            return JsonSerializer.Serialize<BasketCache>(basketCache);
        }

        public IEnumerable<ProductDTO> GetAllProductsInBasket(String userId)
        {
            Basket basket = _db.Basket.GetByUserId(userId);
            return mapperGetProductsFromBasket(basket);
        }

        public int GetOrderTotalAmount(String userId)
        {
            Basket basket = _db.Basket.GetByUserId(userId);
            return mapperTotalAmountFromBasket(basket);
        }

        private IEnumerable<ProductDTO> mapperGetProductsFromBasket(Basket basket)
        {
            List<ProductDTO> productsInBasketList = new List<ProductDTO> { };
            foreach (int id in basket.ProductsId)
            {
                Product product = _db.Product.Get(id);
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

        private int mapperTotalAmountFromBasket(Basket basket)
        {
            int totalAmount = 0;
            foreach (int id in basket.ProductsId)
            {
                totalAmount += _db.Product.Get(id).Price;
            }
            return totalAmount;
        }
    }
}
