using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace WebApiTest
{
    /// <summary>
    /// Static class for Db actions handling
    /// </summary>
    public static class TestDbSeeder
    {
        /// <summary>
        /// Static method to create db and seed with test data
        /// </summary>
        /// <param name="context"></param>
        public static void SeedDb(WebAppContext context)
        {
            context.Database.EnsureCreated();

            context.Users.AddRange(new List<User>{ // there must be at least 1 mock data in db
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

        /// <summary>
        /// Static method to delete db
        /// </summary>
        /// <param name="context"></param>
        public static void DeleteDb(WebAppContext context)
        {
            context.Database.EnsureDeleted();
        }
    }
}