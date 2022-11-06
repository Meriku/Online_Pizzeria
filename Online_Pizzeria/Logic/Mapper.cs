using Online_Pizzeria.Models;

namespace Online_Pizzeria.Logic
{
    public class Mapper<T, TT>
        where TT : class
        where T : class
    {
        public Mapper()
        {

        }

        public TT? Map(T input)
        {
            if (input is null)
            {
                throw new ArgumentNullException();
            }

            switch (typeof(TT).Name)
            {
                case "OrderUserModel":
                    if (input is OrderDBModel OrderDB)
                    {
                        var result = new OrderUserModel()
                        {
                            UserPizza = new PizzaDBModel()
                            {
                                Name = "",
                                Ingredients = ""
                            },
                            Price = OrderDB.Price
                        };
                        return result as TT;
                    }
                    break;

                case "OrderDBModel":
                    if (input is OrderUserModel OrderUser)
                    {
                        var result = new OrderDBModel()
                        {
                            Price = OrderUser.Price,
                            Ordered = OrderUser.Ordered
                        };
                        return result as TT;
                    }
                    break;
            }

            return default;

        }






    }
}
