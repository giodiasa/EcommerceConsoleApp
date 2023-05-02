using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceConsoleApp
{
    internal class Product
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPurchase { get; set; }
        public decimal TotalOrder { get; set; }
        public int TotalPurchaseQuantity { get; set; }
        public int TotalOrderQuantity { get; set; }

        public Product(string id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = 0;
            TotalOrder = 0;
            TotalPurchase = 0;
            TotalPurchaseQuantity = 0;
            TotalOrderQuantity = 0;
        }

    }
}
