using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DAL.Entities
{
    public class Basket
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string InternalData { get; set; }

        [NotMapped]
        public List<int> ProductsId
        {
            get
            {
                return Array.ConvertAll(InternalData.Split(';'), int.Parse).ToList();
            }
            set
            {
                var _data = value;
                InternalData = String.Join(";", _data.Select(p => p.ToString()).ToArray());
            }
        }
    }
}
