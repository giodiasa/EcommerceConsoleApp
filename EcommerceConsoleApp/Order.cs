using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceConsoleApp
{
    internal class Order
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal COGS { get; set; }
        public decimal SellingPrice { get; set; }

        public Order(string productId, int quantity, decimal price, decimal cogs, decimal sellingPrice)
        {
            ProductId = productId;
            Quantity = quantity;
            Price = price;
            COGS = cogs;
            SellingPrice = sellingPrice;
        }

    }
    
}
