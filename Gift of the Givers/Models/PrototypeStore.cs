namespace Gift_of_the_Givers.Models
{
    public class VolunteerSignup
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Skills { get; set; } = string.Empty;

        public string Availability { get; set; } = string.Empty;

        public string? AdditionalInformation { get; set; }

        public DateTime DateSubmitted { get; set; }
    }

    public class ReliefUpdate
    {
        public string ProjectName { get; set; } = string.Empty;

        public string UpdateMessage { get; set; } = string.Empty;

        public string PostedBy { get; set; } = string.Empty;

        public DateTime DatePosted { get; set; }
    }

    public class PrototypeStore
    {
        public List<VolunteerSignup> Volunteers { get; set; } = new();

        public List<ReliefUpdate> ReliefUpdates { get; set; } = new();
    }
}