using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RailBook.Configuration;
using RailBook.Domain.Entities;
using RailBook.Manager.Interfaces;

namespace RailBook.Manager.Implementations
{
    /// Service responsible for generating and validating JWT tokens

    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        /// Generates a JWT token for the given user
        /// User entity to create token for
        /// JWT token string
        public string GenerateToken(User user)
        {
            // 1. Create claims (information stored in the token)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()) // Issued at
            };


            //  Its like a configuration for which algorithm and key to use for signing the token.
            // 2. Create signing credentials using the secret key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 3. Create token descriptor
            /*  What SecurityTokenDescriptor Contains

                    Subject (ClaimsIdentity)
                    This is your payload.
                    It holds all the claims (user ID, username, email, issued-at, etc.).
                    Conceptually: “These are the pieces of information I want in the token.”


                    Expires
                    The token’s expiration time.
                    Conceptually: “This token is only valid until this date/time.”


                    Issuer (Issuer)
                    Who created the token.
                    Helps the server verify that the token actually came from your authentication server.
                    Conceptually: “This server issued the token.”


                    Audience (Audience)
                    Specifies who the token is intended for (like your API).
                    Conceptually: “This token is meant for this specific audience.”



                    SigningCredentials
                    The configuration for signing (secret + algorithm).
                    Conceptually: “Here’s how to seal this token so it can be verified later.”
 * 
 */
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = credentials
            };

            // 4. Generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 5. Return token as string
            return tokenHandler.WriteToken(token);
        }

        /// Validates a JWT token and returns the claims principal
        /// ClaimsPrincipal if valid, null otherwise
        public ClaimsPrincipal? ValidateToken(string token)
        {
            /* Step 1: Create a token handler
             * ------------------------------
             * JwtSecurityTokenHandler is responsible for:
             * - Reading (parsing) JWT tokens
             * - Validating them according to given parameters
             * - Extracting the claims (user data) if valid
             */
            var tokenHandler = new JwtSecurityTokenHandler();


            /* Step 2: Convert the secret key to byte array
             * --------------------------------------------
             * The secret key used to sign tokens is stored as a string.
             * It must be converted to bytes because cryptographic operations
             * work with byte arrays, not plain text strings.
             */
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);


            try
            {
                /* Step 3: Define token validation parameters
                 * ------------------------------------------
                 * TokenValidationParameters specify how the JWT should be validated.
                 * This includes:
                 * 
                 * - ValidateIssuerSigningKey: ensures token is signed using our secret key
                 * - IssuerSigningKey: provides the actual symmetric key for signature validation
                 * - ValidateIssuer: checks that the token’s 'iss' (issuer) matches our expected value
                 * - ValidIssuer: sets the expected issuer (from configuration)
                 * - ValidateAudience: ensures the token’s 'aud' (audience) matches our expected value
                 * - ValidAudience: sets the expected audience (from configuration)
                 * - ValidateLifetime: ensures the token hasn’t expired
                 * - ClockSkew: sets tolerance for time difference between systems (0 = no tolerance)
                 * 
                 * This object defines the “rules” for trusting or rejecting a token.
                 */
                var principal = tokenHandler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = _jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = _jwtSettings.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero // No grace period for expired tokens
                    },
                    out SecurityToken validatedToken // Output: stores the validated token if successful
                );


                /* Step 4: Return ClaimsPrincipal
                 * -------------------------------
                 * If validation is successful, the method returns a ClaimsPrincipal object.
                 * ClaimsPrincipal represents the authenticated user and contains all claims
                 * (like user ID, email, role, etc.) from the token payload.
                 * 
                 * This object is later used by ASP.NET’s authentication system
                 * to identify the currently logged-in user.
                 */
                return principal;
            }
            catch
            {
                /* Step 5: Handle validation failure
                 * ---------------------------------
                 * If token validation fails (e.g., expired, invalid signature,
                 * wrong issuer, or audience), an exception is thrown.
                 * In such cases, return null to indicate the token is invalid.
                 */
                return null;
            }
        }

    }
}