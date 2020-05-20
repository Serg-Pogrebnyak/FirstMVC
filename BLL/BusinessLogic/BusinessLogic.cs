using System;
using System.Collections.Generic;
using System.Text;
using BLL.DTO;
using DAL.Entities;
using BLL.Interfaces;
using DAL.Interfaces;
using AutoMapper;

namespace BLL.Business_Logic
{
    public class BusinessLogic : IOrderService
    {
        private readonly IUnitOfWork _db;
        public BusinessLogic(IUnitOfWork db)
        {
            _db = db;
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
