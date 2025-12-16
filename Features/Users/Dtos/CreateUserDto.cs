namespace ReactorTwinAPI.Features.Users.Dtos
{
    public class CreateUserDto
    {
        public string Username { get; set; } = null!;
        public bool IsSuperUser { get; set; } = false;
        public bool CanCreateReactor { get; set; } = false;
    }
}
