namespace Services.Dtos
{
    public class AuthenticationResultDto
    {
        public string AccessToken { get; set; }

        public long Expiration { get; set; }

        public string RefreshToken { get; set; }
    }
}
