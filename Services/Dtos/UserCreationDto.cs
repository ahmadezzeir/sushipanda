namespace Services.Dtos
{
    public class UserCreationDto
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool IsFromExternalService { get; set; } = false;
    }
}