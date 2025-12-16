namespace ReactorTwinAPI.Features.Users.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public bool IsSuperUser { get; set; }
        public bool CanCreateReactor { get; set; }
    }
}
