using TodoApi.Models;

namespace UsersApi.Services
{
    /// <summary>
    /// Static class to handle API start up
    /// </summary>
    public static class StartUpManager
    {
        /// <summary>
        /// Method to seed some base data into DB
        /// </summary>
        /// <param name="sp"></param>
        public static void APIStartUp(IServiceProvider sp)
        {
            var context = sp.GetRequiredService<WebAppContext>();

            context.Users.AddRange(new List<User>{
                new()
                {
                    Name = "Csaba",
                    Country = "Denmark",
                    City = "Aarhus",
                    Street = "Test Vej",
                    HouseNumber = 1,
                    ZipCode = 8200
                },
                new()
                {
                    Name = "Mate",
                    Country = "Hungary",
                    City = "Miskolc",
                    Street = "Test utca",
                    HouseNumber = 1,
                    ZipCode = 3523
                }
            });

            context.SaveChanges();
        }
    }
}
