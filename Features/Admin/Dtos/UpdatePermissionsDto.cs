namespace ReactorTwinAPI.Features.Admin.Dtos
{
    public class UpdatePermissionsDto
    {
        public bool? CanCreate { get; set; }
        public bool? IsSuper { get; set; }
    }
}
