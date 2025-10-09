namespace RailBook.Configuration
{
    /// <summary>
    /// Configuration settings for JWT token generation and validation
    /// </summary>
    public class JwtSettings
    {
        // Secret key used to sign tokens (should be stored securely, e.g., in User Secrets or Azure Key Vault)
        public string Secret { get; set; } = string.Empty;

        // Who creates the token (usually your application name)
        public string Issuer { get; set; } = string.Empty;

        // Who can use the token (usually your frontend URL or "Any")
        public string Audience { get; set; } = string.Empty;

        // How long the token is valid (in minutes)
        public int AccessTokenExpiryMinutes { get; set; } = 60;
        public double RefreshTokenExpiryDays { get; internal set; }
    }
}