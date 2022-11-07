using Online_Pizzeria.Models;

namespace Online_Pizzeria.Logic
{
    public static class Helper
    {

        public static string[] GetPossibleIngredients()
        {
            return Enum.GetNames(typeof(Ingredient));
        }

    }
}
