using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaSysteem
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; }
        public string Surname { get; }
        
        public Customer(int id, string name, string surname)
        {
            Id = id;
            Name = name;
            Surname = surname;
        }

        public virtual string PrintFullName()
        {
            return Name + " " + Surname;
        }
    }
}
