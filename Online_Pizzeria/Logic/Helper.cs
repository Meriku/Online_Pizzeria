using Online_Pizzeria.Models;

namespace Online_Pizzeria.Logic
{
    public static class Helper
    {

        public static string[] GetPossibleIngredients()
        {
            return Enum.GetNames(typeof(Ingredient));
        }
        
        public static string[] GetPossibleStatuses()
        {
            return Enum.GetNames(typeof(Models.StatusCodes));
        }
        public static bool ParseInt(string? input, out int result)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(input))
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
        public static bool ParseDecimal(string? input, out decimal result)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }
            if (decimal.TryParse(input, out result))
            {
                return true;
            }
            else
            {
                throw new Exception("Parsing exception. Value must be decimal.");
            }
        }

        public static Models.StatusCodes GetEnum(string input)
        {
            if (Enum.TryParse(input, out Models.StatusCodes code))
            {
                return code;
            }
            else
            {
                throw new Exception("Parsing exception. String must be enum.");
            }
        }

        public static string GetConnectionString(this WebApplicationBuilder builder)
        {
            var appConfig = builder.Configuration;

            string dbname = appConfig.GetSection("RDS_DB_NAME").Value;
            string username = appConfig["RDS_USERNAME"];
            string password = appConfig["RDS_PASSWORD"];
            string hostname = appConfig["RDS_HOSTNAME"];
            string port = appConfig["RDS_PORT"];

            return "Data Source=" + hostname + ";Initial Catalog=" + dbname + ";User ID=" + username + ";Password=" + password + ";";
        }
    }
}
