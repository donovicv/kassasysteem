using System.Collections.Generic;

namespace KassaSysteem
{
    internal class Order
    {
        public int Id { get; }
        public List<Product> Products { get; }
        public Customer Customer { get; set; }

        public Order(int id, List<Product> products)
        {
            Id = id;
            Products = products;
            Customer = null;
        }

        public Order(int id, List<Product> products, Customer customer)
        {
            Id = id;
            Products = products;
            Customer = customer;
        }

        public void AddProduct(Product product)
        {
            if (Products == null)
            {
                Products.Add(product);
            }
        }
    }
}
