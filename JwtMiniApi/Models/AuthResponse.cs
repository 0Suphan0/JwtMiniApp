namespace JwtMiniApi.Models
{
    public record AuthResponse(string AccessToken, DateTime ExpiresAt);

}
