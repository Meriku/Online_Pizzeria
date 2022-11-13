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
                    if (input is OrderDBModel OrderDBtoUser)
                    {
                        throw new NotImplementedException();
                        var result = new OrderUserModel()
                        {
                            UserPizza = new PizzaUserModel()
                            {
                                Name = "",
                                Ingredients = ""
                            },
                            Price = OrderDBtoUser.Price
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

                case "OrderAdminModel":
                    if (input is OrderDBModel OrderDBtoAdmin)
                    {
                        var result = new OrderAdminModel()
                        {
                            Id = OrderDBtoAdmin.Id,
                            PizzaName = "",
                            Price = OrderDBtoAdmin.Price,
                            StatusCode = OrderDBtoAdmin.StatusCode.ToString(),
                            Ordered = OrderDBtoAdmin.Ordered.ToString("G"),
                            Delivered = OrderDBtoAdmin.Delivered == null ? "" : OrderDBtoAdmin.Delivered.ToString(),

                        };
                        return result as TT;
                    }
                    break;

                case "PizzaUserModel":
                    if (input is PizzaDBModel PizzaDBtoUser)
                    {
                        var imgsrc = $"wwwroot/images/AllPizzas/{PizzaDBtoUser.Name}.png";

                        var result = new PizzaUserModel()
                        {
                            Name = PizzaDBtoUser.Name,
                            ImageName = File.Exists(imgsrc) ? PizzaDBtoUser.Name : "Create",
                            BasePrice = PizzaDBtoUser.BasePrice,
                            Ingredients = PizzaDBtoUser.Ingredients
                        };
                        return result as TT;
                    }
                    break;
            }

            return default;

        }






    }
}
