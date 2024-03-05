namespace KassaSysteem
{
    public class JapaneseCustomer : Customer
    {
        public string Honorific { get; set; }

        public JapaneseCustomer(int id, string name, string surname, string honorific) : base(id, name, surname)
        {
            Honorific = honorific;
        }

        public override string PrintFullName()
        {
            return Surname + " " + Name + " " + Honorific;
        }
    }
}
