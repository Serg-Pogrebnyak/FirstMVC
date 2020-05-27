using System.Collections.Generic;

namespace DAL.Entities
{
    public class Categories
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Categories> ChildCategory { get; set; }

        public virtual Categories? ParentCategory { get; set; }
    }
}