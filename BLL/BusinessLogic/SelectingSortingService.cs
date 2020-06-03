using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces;
using static BLL.Interfaces.ICategoryService;

namespace BLL.BusinessLogic
{
    internal static class SelectingSortingService
    {
        public static IEnumerable<T> SortByCriteria<T>(T[] array, SortByEnum sortBy) where T : class
        {
            switch (sortBy)
            {
                case SortByEnum.PriceToUp:
                case SortByEnum.PriceToDown:
                    return SortByPrice<T>(array.Cast<IPrice>().ToArray(), sortBy);
                case SortByEnum.ByName:
                case SortByEnum.ByNameDescending:
                    return SortByName<T>(array.Cast<IName>().ToArray(), sortBy);
                default:
                    throw new NotImplementedException();
            }
        }

        public static IEnumerable<IPrice> SelectByPrice(IPrice[] array, int from, int to)
        {
            if (to > from)
            {
                return array.Where(productDTO => productDTO.Price >= from && productDTO.Price <= to).ToList();
            }
            else
            {
                return array.Where(productDTO => productDTO.Price >= from);
            }
        }

        private static IEnumerable<T> SortByName<T>(IName[] array, SortByEnum sortBy) where T : class
        {
            switch (sortBy)
            {
                case SortByEnum.ByName:
                    return array.OrderBy(element => element.Name).Cast<T>();
                case SortByEnum.ByNameDescending:
                    return array.OrderByDescending(element => element.Name).Cast<T>();
            }

            throw new NotImplementedException();
        }

        private static IEnumerable<T> SortByPrice<T>(IPrice[] array, SortByEnum sortBy) where T : class
        {
            switch (sortBy)
            {
                case SortByEnum.PriceToUp:
                    return array.OrderBy(productDTO => productDTO.Price).Cast<T>();
                case SortByEnum.PriceToDown:
                    return array.OrderByDescending(productDTO => productDTO.Price).Cast<T>();
            }

            throw new NotImplementedException();
        }
    }
}