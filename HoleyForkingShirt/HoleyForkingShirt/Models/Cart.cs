using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoleyForkingShirt.Models
{
    public class Cart
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public List<CartItems> cartItems { get; set; }
    }
}
