using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    /// <summary>
    /// DB model for users
    /// </summary>
    public class User
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Country { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? Street { get; set; }

        [Required]
        public long ZipCode { get; set; }

        [Required]
        public long HouseNumber { get; set; }

        [NotMapped]
        public bool IsValid //checks if user data is valid
        {
            get
            {
                return IsPropertyValid(Name)
                    && IsPropertyValid(Country)
                    && IsPropertyValid(City)
                    && IsPropertyValid(Street);
            }
        }

        /// <summary>
        /// Method to decide if given property is valid or not
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private static bool IsPropertyValid(string? stringProperty)
        {
            return !string.IsNullOrWhiteSpace(stringProperty) && stringProperty != String.Empty;
        }
    }
}
