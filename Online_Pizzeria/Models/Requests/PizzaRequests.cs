namespace Online_Pizzeria.Models.Requests
{
    public class BasePizzaRequest
    {
        public string Name { get; set; }
        public string BasePrice { get; set; }
        public string Ingredients { get; set; }
    }

    public class CreatePizzaRequest: BasePizzaRequest
    {
    }

    public class EditPizzaRequest: BasePizzaRequest
    {
        public string PizzaId { get; set; }
    }

    public class DeletePizzaRequest 
    {
        public string PizzaId { get; set; }
    }


}

