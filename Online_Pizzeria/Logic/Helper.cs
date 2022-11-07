using Online_Pizzeria.Models;

namespace Online_Pizzeria.Logic
{
    public static class Helper
    {

        public static string[] GetPossibleIngredients()
        {
            return Enum.GetNames(typeof(Ingredient));
        }

        public static bool ParseInt(string? input, out int result)
        {
            result = 0;

            if (input == null)
            {
                return false;
            }
            if (int.TryParse(input, out result))
            {
                return true;
            } 
            else
            {
                throw new Exception("Parsing exception. Value must be int.");
            }
        }

    }
}
