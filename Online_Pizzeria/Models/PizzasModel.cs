namespace Online_Pizzeria.Models
{
    public class PizzasModel : PizzaDBModel
    {
        public string ImageTitle { get; set; }

        public List<Ingredient> Ingredients = new List<Ingredient>();
    }
}
