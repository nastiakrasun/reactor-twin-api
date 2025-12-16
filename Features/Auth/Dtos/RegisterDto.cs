namespace ReactorTwinAPI.Features.Auth.Dtos
{
    public class RegisterDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool RequestSuper { get; set; } = false;
    }
}
