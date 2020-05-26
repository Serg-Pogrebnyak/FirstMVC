﻿using System.Collections.Generic;

namespace DAL.Entities
{
    public class Categories
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}