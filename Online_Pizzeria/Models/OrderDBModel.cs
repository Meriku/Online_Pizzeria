namespace Online_Pizzeria.Models
{
    public class OrderDBModel
    {
        public int Id { get; set; }
        public int? PizzaId { get; set; }
        public decimal Price { get; set; }

    }
}
