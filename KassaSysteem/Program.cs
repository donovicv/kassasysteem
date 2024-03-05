using System;
using System.Collections.Generic;
using System.Linq;

namespace KassaSysteem
{
    internal class Program
    {
        static List<Customer> customers = new List<Customer>();
        static List<Product> stock = new List<Product>();
        static List<Product> shopCart = new List<Product>();
        static List<Order> orders = new List<Order>();
        static Random random = new Random();

        public static void Main(string[] args)
        {
            stock.Add(new Product(1, "Banaan", "", 1f, 10));
            stock.Add(new Product(2, "Kaas", "", 1.5f, 10));
            stock.Add(new Product(3, "Melk", "", 1f, 10));
            stock.Add(new Product(4, "Eieren", "", 2.5f, 10));
            stock.Add(new Product(5, "Sla", "", .5f, 10));
            customers.Add(new Customer(0, "Viktor", "Donovic"));
            bool cashSystemIsOn = true;
            while (cashSystemIsOn)
            {
                Console.Clear();
                Console.WriteLine("Welkom!");
                Console.WriteLine("1: Klantaccount aanmaken");
                Console.WriteLine("2: Voorraad laten zien");
                Console.WriteLine("3: Producten toevoegen aan winkelwagen");
                Console.WriteLine("4: Betalen");
                Console.WriteLine("5: Orders weergeven");
                Console.WriteLine("6: Afsluiten");
                string input = AskInput("Kies 1 - 6", new string[] { "1", "2", "3", "4", "5", "6" });

                switch (input)
                {
                    case "1":
                        CreateAccountMenu();

                        break;
                    case "2":
                        StockMenu();

                        break;
                    case "3":
                        Console.Clear();

                        AddProductToCartMenu();

                        break;
                    case "4":
                        PayMenu();

                        break;
                    case "5":
                        OrderMenu();

                        break;
                    case "6":
                        cashSystemIsOn = false;
                        Console.WriteLine("Bedankt voor het gebruiken van ons kassasysteem");
                        Console.WriteLine("Druk op een knop om verder te gaan");
                        Console.ReadKey();

                        break;
                }
            }
        }

        private static void OrderMenu()
        {
            foreach (Order order in orders)
            {
                Console.WriteLine($@"OrderId    Klantnaam");
                Console.WriteLine($"{order.Id}    {order.Customer.Name + " " + order.Customer.Surname}");
                Console.WriteLine();
                Console.WriteLine($"Producten in order {order.Id}");
                Console.WriteLine($@"Id    Naam          Price       Aantal");
                foreach (Product product in order.Products)
                {
                    string format = $@"{product.Id}     {product.Name}             {product.Price}       {product.Amount}";
                    Console.WriteLine(format);
                }

            }
            Console.WriteLine("Druk op een knop om verder te gaan");
            Console.ReadKey();
        }

        private static void CreateAccountMenu()
        {
            Console.Clear();
            Console.WriteLine("Voer de voornaam in");
            string name = Console.ReadLine();
            Console.WriteLine("Voer de achternaam in");
            string surname = Console.ReadLine();
            Customer customer = new Customer(random.Next(10000), name, surname);
            customers.Add(customer);
            Console.WriteLine($"Klant {name} {surname} is toegevoegd aan het systeem");
            Console.WriteLine("Druk op een knop om verder te gaan");
            Console.ReadKey();
        }

        private static void StockMenu()
        {
            Console.Clear();
            Console.WriteLine($@"Id    Naam          Price       Aantal");
            foreach (Product product in stock)
            {
                string format = $@"{product.Id}     {product.Name}             {product.Price}       {product.Amount}";
                Console.WriteLine(format);
            }

            Console.WriteLine("Druk op een knop om verder te gaan");
            Console.ReadKey();
        }

        private static void AddProductToCartMenu()
        {
            bool choosingProducts = true;
            while (choosingProducts)
            {
                Console.WriteLine($@"Id     Naam          Price           Description");
                List<string> inputChoicesProducts = new List<string>();

                foreach (Product product in stock)
                {
                    string format = $@"{product.Id}     {product.Name}      {product.Price} {product.Description}";
                    inputChoicesProducts.Add(product.Id.ToString());
                    Console.WriteLine(format);
                }

                inputChoicesProducts.Add("x");
                inputChoicesProducts.Add("X");
                string productChoice = AskInput("Kies het productId om toe te voegen aan de winkelwagen. Of x als je terug naar het menu wilt.", inputChoicesProducts.ToArray());

                Console.Clear();
                if (productChoice == "x" || productChoice == "X")
                {
                    choosingProducts = false;
                }
                else
                {
                    Product chosenProduct = FindProductById(int.Parse(productChoice));
                    bool productAlreadyInCart = false;
                    Product cartProduct = null;
                    foreach (Product product in shopCart)
                    {
                        if (product.Id == int.Parse(productChoice))
                        {
                            productAlreadyInCart = true;
                            cartProduct = product;
                        }
                    }

                    if (productAlreadyInCart)
                    {
                        cartProduct.Amount++;
                        Console.WriteLine("Er is opgehoogd naar " + cartProduct.Amount);
                    }
                    else
                    {
                        chosenProduct.Amount = 1;
                        shopCart.Add(chosenProduct);
                    }
                    Console.WriteLine($"{chosenProduct.Name} is toegevoegd aan de winkelwagen");
                }
            }
        }

        private static void PayMenu()
        {
            if (shopCart.Count < 1)
            {
                Console.WriteLine("De winkelwagen is leeg, vul deze om te kunnen afrekenen");
                Console.WriteLine("Druk op een knop om verder te gaan");
                Console.ReadKey();
                return;
            }
            Console.Clear();
            Console.WriteLine($@"Id     Naam          Price           Description   amount");

            float shopCartSum = 0;
            foreach (Product product in shopCart)
            {
                string format = $@"{product.Id}     {product.Name}      {product.Price} {product.Description}   {product.Amount}";
                Console.WriteLine(format);
                shopCartSum += product.Price * product.Amount;

            }
            Console.WriteLine($"Het totaal is: {shopCartSum}");

            Order order = new Order(random.Next(100000), new List<Product>(shopCart));

            Customer customer;
            if (customers.Count > 0)
            {
                customer = PayShopCart(shopCartSum, customers);
                order.Customer = customer;
            }
            else
            {
                Console.WriteLine("Er bestaat nog geen klantaccount, afrekenen wordt gedaan zonder account.");
            }

            orders.Add(order);
            ReduceStock(shopCart);
            shopCart.Clear();
            Console.WriteLine("Druk op een knop om verder te gaan");
            Console.ReadKey();
        }

        private static Customer PayShopCart(float shopCartSum, List<Customer> customers)
        {
            List<string> inputCustomerChoices = new List<string>();
            string customerHeader = @"Id     Naam      Achternaam";
            Console.WriteLine(customerHeader);
            foreach (Customer payingCustomer in customers)
            {
                // choose customer
                string customerFormat = $@"{payingCustomer.Id}     {payingCustomer.Name}      {payingCustomer.Surname}";
                Console.WriteLine(customerFormat);
                inputCustomerChoices.Add(payingCustomer.Id.ToString());
                Console.WriteLine();
            }
            inputCustomerChoices.Add("x");
            inputCustomerChoices.Add("X");

            string customerChoice = AskInput("Welke klant betaald? Voer x in om zonder account af te rekenen.", inputCustomerChoices.ToArray());
            Customer chosenCustomer = null;
            if (customerChoice == "x" || customerChoice == "X")
            {
                Console.WriteLine("Afrekenen wordt gedaan zonder account.");
            }
            else
            {
                chosenCustomer = FindCustomerById(int.Parse(customerChoice));
                Console.WriteLine($"{chosenCustomer.Name} {chosenCustomer.Surname} heeft {shopCartSum} euro betaald.");
            }
            return chosenCustomer;
        }

        private static Product FindProductById(int id)
        {
            foreach (Product product in stock)
            {
                if (product.Id == id)
                {
                    return new Product(product.Id, product.Name, product.Description, product.Price);
                }
            }
            return null;
        }

        private static Customer FindCustomerById(int id)
        {
            foreach (Customer customer in customers)
            {
                if (customer.Id == id)
                {
                    return new Customer(customer.Id, customer.Name, customer.Surname);
                }
            }
            return null;
        }

        private static void ReduceStock(List<Product> boughtProducts)
        {
            foreach (Product product in stock)
            {
                foreach (Product boughtProduct in boughtProducts)
                {
                    if (product.Id == boughtProduct.Id)
                    {
                        product.Amount -= boughtProduct.Amount;
                    }
                }
            }
        }

        private static string AskInput(string question, string[] allowedInputs)
        {
            while (true)
            {
                string input = AskInput(question);
                if (allowedInputs.Contains(input) || allowedInputs.Length == 0)
                {
                    return input;
                }
                Console.WriteLine("Input is invalid.");
                Console.WriteLine("Allowed input is: ");
                foreach (string allowedInput in allowedInputs)
                {
                    Console.WriteLine($"{allowedInput}");
                }
            }
        }

        private static string AskInput(string question)
        {
            Console.WriteLine(question);
            return Console.ReadLine();
        }
    }
}
