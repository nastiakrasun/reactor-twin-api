namespace ReactorTwinAPI.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public bool IsSuperUser { get; set; } = false;
        public bool CanCreateReactor { get; set; } = false;

        public ICollection<ReactorTwin>? ReactorTwins { get; set; }
    }
}
