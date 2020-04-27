using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoleyForkingShirt.Models
{
    public class CartItems
    {
        public string CartID { get; set; }
        public string ProductID { get; set; }
        public int Qty { get; set; }
        public Product Product { get; set; }
    }
}
