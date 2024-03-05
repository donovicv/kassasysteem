namespace KassaSysteem
{
    internal class Product
    {
        public int Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price {  get; set; }
        public int Amount { get; set; }

        public Product(int id, string name, string description, float price, int amount)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Amount = amount;
        } 
        
        public Product(int id, string name, string description, float price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Amount = 0;
        }
    }
}
