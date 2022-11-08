namespace Online_Pizzeria.Models.Requests
{
    public class CreatePizzaRequest
    {
        public string Name { get; set; }
        public string BasePrice { get; set; }
        public string Ingredients { get; set; }
    }
}

