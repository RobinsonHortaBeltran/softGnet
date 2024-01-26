namespace apiNew.Models
{
    public class Drivers
    {
        public int Id { get; set; }
        public string? Last_name { get; set; }
        public string? First_name { get; set; }
        public string? Ssn { get; set; }
        public DateTime Dod { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Zip { get; set; }
        public int? Phone { get; set; }
        public bool Active { get; set; }


        public Drivers()
        {
            Dod = DateTime.UtcNow;
        }
    }
}