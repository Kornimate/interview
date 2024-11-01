namespace WPFClient.DTOs
{
    /// <summary>
    /// Container for serialization from API call
    /// </summary>
    public record UserDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public long ZipCode { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public long HouseNumber { get; set; }
    }
}
