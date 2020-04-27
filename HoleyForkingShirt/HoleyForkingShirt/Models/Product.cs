using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoleyForkingShirt.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public Sizes Size { get; set; }
        public string Image { get; set; }
       
    }

    public enum Sizes
    {
        XS = 0,
        S = 1,
        M = 2,
        L = 3,
        XL = 4,
        XXL = 5
    }
}
