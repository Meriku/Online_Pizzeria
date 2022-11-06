namespace Online_Pizzeria.Models
{
    public class OrderUserModel
    {
        public PizzaDBModel UserPizza { get; set; }
        public decimal Price { get; set; }
        public DateTime Ordered { get; set; }
    }
}
