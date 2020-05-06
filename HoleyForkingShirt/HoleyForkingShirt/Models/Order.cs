using System;

namespace HoleyForkingShirt.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }
        public string SB { get; set; }
        

    }

    
}