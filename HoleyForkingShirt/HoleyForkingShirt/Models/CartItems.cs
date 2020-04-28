using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoleyForkingShirt.Models
{
    public class CartItems
    {
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public int Qty { get; set; }
        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}
