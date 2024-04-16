namespace TaskMonitorWebAPI.Dto
{
    public class Register
    {
        public string Name { get; set; } = String.Empty;
        public string Surname { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
        public int age { get; set; }
        public string EmailAddress { get; set; }  = String.Empty;
        public string DateOfBirth { get; set; } = String.Empty;
        public string? Address { get; set; }

        public string? Password { get; set; }
    }
}
