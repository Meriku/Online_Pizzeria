namespace Online_Pizzeria.Models
{
    public class OrderAdminModel
    {
        public int Id { get; set; }
        public string PizzaName { get; set; }
        public decimal Price { get; set; }
        public string StatusCode { get; set; }
        public string Ordered { get; set; }
        public string Delivered { get; set; }
    }
}
