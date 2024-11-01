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
        public long ZipCode { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public long HouseNumber { get; set; }

        [NotMapped]
        public bool IsValid //checks if user data is valud
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return false;

                if (string.IsNullOrWhiteSpace(Country))
                    return false;

                if (string.IsNullOrWhiteSpace(City))
                    return false;

                if (string.IsNullOrWhiteSpace(Street))
                    return false;

                return true;
            }
        }
    }
}
