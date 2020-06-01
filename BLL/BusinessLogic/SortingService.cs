using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces;
using static BLL.Interfaces.ICategoryService;

namespace BLL.BusinessLogic
{
    internal static class SortingService
    {
        public static IEnumerable<T> SortByCriteria<T>(T[] array, SortByEnum sortBy) where T : class
        {
            switch (sortBy)
            {
                case SortByEnum.ByName:
                case SortByEnum.ByNameDescending:
                    return SortByName<T>(array.Cast<IName>().ToArray(), sortBy);
                default:
                    throw new NotImplementedException();
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
    }
}