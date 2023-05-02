using EcommerceConsoleApp;
using OfficeOpenXml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using LicenseContext = OfficeOpenXml.LicenseContext;

List <Product> products = new List <Product> ();
List<Order> orders = new List<Order>();

void SaveProduct(string id, string name, decimal price)
{
    if(products.Any(x=>x.Id == id)){
        Product? product = products.FirstOrDefault(x => x.Id == id);
        if (product != null) {
            product.Name = name;
            product.Price = price;
            Console.WriteLine("Changes applied");
            return;
        }        
    }
    Product newProduct = new Product(id, name, price);
    products.Add(newProduct);
    Console.WriteLine("Product added successfully");
}

void PurchaseProduct(string id, int quantity, decimal price)
{
    Product ?productToPurchase = products.FirstOrDefault(x => x.Id == id);
    if(productToPurchase != null)
    {
        productToPurchase.Quantity += quantity;
        productToPurchase.TotalPurchase += price * quantity;
        productToPurchase.TotalPurchaseQuantity += quantity;
        Console.WriteLine("Product successfully purchased");
    }
    else
    {
        Console.WriteLine("Product with this ID not found");
        return;
    }    
}

void OrderProduct(string id, int quantity)
{
    Product? productToOrder = products.FirstOrDefault(x => x.Id == id);
    if(productToOrder != null)
    {
        if (productToOrder.Quantity < quantity)
        {
            Console.WriteLine($"Not enough {productToOrder.Name}");
            return;
        }
        productToOrder.Quantity -= quantity;
        productToOrder.TotalOrder += productToOrder.Price * quantity;
        productToOrder.TotalOrderQuantity += quantity;
        Console.WriteLine("Product successfully ordered");
        decimal cogs = productToOrder.TotalPurchase / productToOrder.TotalPurchaseQuantity * quantity;
        decimal sellingPrice = quantity * productToOrder.Price;
        Order order = new Order(id, quantity, productToOrder.Price, cogs, sellingPrice);
        orders.Add(order);
    }
    else
    {
        Console.WriteLine("Product with this ID not found");
        return;
    }
}

void GetQuantityOfProduct(string id)
{
    Product? product = products.FirstOrDefault(x => x.Id == id);
    if(product != null)
    {
        Console.WriteLine($"There is {product.Quantity} number of {product.Name} in stock");
    }
    else
    {
        Console.WriteLine("Product with this ID not found");
        return;
    }
}

void GetAveragePrice(string id)
{
    Product? product = products.FirstOrDefault(x => x.Id == id);
    if (product != null)
    {
        decimal averagePrice = product.TotalPurchase / product.TotalPurchaseQuantity;
        Console.WriteLine($"Average price of {product.Name} is {averagePrice}");
    }
    else
    {
        Console.WriteLine("Product with this ID not found");
        return;
    }
}

void GetProductProfit(string id)
{
    Product? product = products.FirstOrDefault(x => x.Id == id);
    if(product != null)
    {
        decimal averagePurchasePrice = product.TotalPurchase / product.TotalPurchaseQuantity;
        decimal averageOrderPrice = product.TotalOrder / product.TotalOrderQuantity;
        decimal profitPerUnit = averageOrderPrice - averagePurchasePrice;
        decimal totalProfit = profitPerUnit * product.TotalOrderQuantity;
        Console.WriteLine($"Total profit for {product.Name} is {totalProfit}");
    }
    else
    {
        Console.WriteLine("Product with this ID not found");
        return;
    }
}

void GetFewestProduct()
{
    if (products.Count == 0)
    {
        Console.WriteLine("Products list is empty");
        return;
    }
    var product = products.OrderBy(x => x.Quantity).FirstOrDefault();
    if (product != null)
    {
        int fewestProduct = product.Quantity;
        Console.WriteLine($"Product with the lowest remaining quantity is {product.Name}");
    }
}

void GetMostPopularProduct()
{
    if (products.Count == 0)
    {
        Console.WriteLine("Products list is empty");
        return;
    }
    var product = products.OrderBy(x => x.TotalOrderQuantity).LastOrDefault();
    if (product != null)
    {
        int fewestProduct = product.TotalOrderQuantity;
        Console.WriteLine($"Product with the highest number of orders is {product.Name}");
    }
}

void GetOrdersReport()
{
    if (orders.Count == 0)
    {
        Console.WriteLine("Orders list is empty");
        return;
    }
    Console.WriteLine("Orders Report");
    Console.WriteLine("----------------------------------------------------------------------------------");
    Console.WriteLine("| Product ID | Product Name | Quantity |   Price   |    COGS     | Selling Price |");
    Console.WriteLine("----------------------------------------------------------------------------------");

    foreach (var item in orders)
    {
        var name = products.Find(x => x.Id == item.ProductId)?.Name;
        Console.WriteLine($"| {item.ProductId,-10} | {name,-12} | {item.Quantity,-8} | {item.Price,-5:C2} | {item.COGS,-4:C2} | {item.SellingPrice,-7:C2} ");
    }

}

void ExportOrdersReport(string path)
{
    path += "\\report"+DateTime.Now.ToString("yyyyMMdd_HHmmss") +".csv";
    var stream = new MemoryStream();

    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    using (var package = new ExcelPackage(stream))
    {
        var workSheet = package.Workbook.Worksheets.Add("Sheet1");
        workSheet.Cells.LoadFromCollection(orders, true);
        Stream writeStream = File.Create(path); 
        package.SaveAs(writeStream); 
        writeStream.Close();
    }
    Console.WriteLine("Orders report is ready");
}

while (true)
{
    Console.WriteLine("Enter command (save_product, purchase_product, order_product, get_quantity_of_product, get_average_price, get_product_profit, get_fewest_product, get_most_popular_product, get_orders_report, export_orders_report, exit):");
    string input = Console.ReadLine() ?? string.Empty;
    string[] splitInput = input.Split(' ');
    switch (splitInput[0])
    {
        case "save_product":
            if(splitInput.Length != 4)
            {
                Console.WriteLine("Input example: save_product {product_id} {product_name} {price}");
                continue;
            }
            try
            {
                SaveProduct(splitInput[1], splitInput[2], Convert.ToDecimal(splitInput[3]));
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
            break;
        case "purchase_product":
            if (splitInput.Length != 4)
            {
                Console.WriteLine("Input example: purchase_product {product_id} {quantity} {price}");
                continue;
            }
            try
            {
                PurchaseProduct(splitInput[1], Convert.ToInt32(splitInput[2]), Convert.ToDecimal(splitInput[3]));
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
            break;
        case "order_product":
            if (splitInput.Length != 3)
            {
                Console.WriteLine("Input example: order_product {product_id} {quantity}");
                continue;
            }
            try
            {
                OrderProduct(splitInput[1], Convert.ToInt32(splitInput[2]));
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
            break;
        case "get_quantity_of_product":
            if (splitInput.Length != 2)
            {
                Console.WriteLine("Input example: get_quantity_of_product {product_id}");
                continue;
            }
            GetQuantityOfProduct(splitInput[1]);
            break;
        case "get_average_price":
            if (splitInput.Length != 2)
            {
                Console.WriteLine("Input example: get_average_price {product_id}");
                continue;
            }
            GetAveragePrice(splitInput[1]);
            break;
        case "get_product_profit":
            if (splitInput.Length != 2)
            {
                Console.WriteLine("Input example: get_product_profit {product_id}");
                continue;
            }
            GetProductProfit(splitInput[1]);
            break;
        case "get_fewest_product":
            if (splitInput.Length != 1)
            {
                Console.WriteLine("Input example: get_fewest_product");
                continue;
            }
            GetFewestProduct();
            break;
        case "get_most_popular_product":
            if (splitInput.Length != 1)
            {
                Console.WriteLine("Input example: get_most_popular_product:");
                continue;
            }
            GetMostPopularProduct();
            break;
        case "get_orders_report":
            if (splitInput.Length != 1)
            {
                Console.WriteLine("Input example: get_orders_report:");
                continue;
            }
            GetOrdersReport();
            break;
        case "export_orders_report":
            ExportOrdersReport(splitInput[1]);
            break;
        case "exit":
            Console.WriteLine("Exiting...");
            return;

        default:
            Console.WriteLine("Invalid input. Please try again");
            break;
    }
}

