namespace Online_Pizzeria.Models
{
    public class PizzasModel
    {
        public string ImageTitle { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public List<Ingredient> Ingredients = new List<Ingredient>();
    }
}
