namespace UserRegistrationAPI
{
    public class User
    {
        public int Id { get; set; }
        public string emailAddress { get; set; } = string.Empty;
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

       
    }
}
