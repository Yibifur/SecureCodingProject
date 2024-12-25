using Microsoft.IdentityModel.Tokens;
using OneDose.FirstProject.EntityLayer.Concrete;
using OneDose.FirstProject.WebAPI.Caching;
using OneDose.FirstProject.WebAPI.Security.Abstract;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OneDose.FirstProject.WebAPI.Security.Handlers
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IRedisCacheService _redisService;
        public TokenHandler(IConfiguration configuration, IRedisCacheService redisService)
        {
            _configuration = configuration;
            _redisService = redisService;
        }
        public void CreateSessionForUser(string userId,List<string> values)
        {
            
            _redisService.SetSessionIdAsync(userId,values );
        }
        public async Task< string> CreateTokenAsync(User user)
        {
            string sessionId = Guid.NewGuid().ToString();
            List<Claim> claims = new List<Claim>
            {
                new Claim("sub", user.UserId.ToString()),
                new Claim("name", user.Name),
                new Claim("surname", user.Surname),
                new Claim("address", user.Address),
                 new Claim("role", user.Role),
                 new Claim("SessionId",sessionId),

            };
            Token token = new Token();
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            token.ExpireDate = DateTime.Now.AddMinutes(double.Parse(_configuration.GetSection("AppSettings:Expiration").Value));
            JwtSecurityToken securityToken = new JwtSecurityToken(issuer: _configuration.GetSection("AppSettings:Issuer").Value, audience: _configuration.GetSection("AppSettings:Audience").Value,
              expires: token.ExpireDate, notBefore: DateTime.Now, signingCredentials: signingCredentials, claims: claims);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            token.AccessToken = handler.WriteToken(securityToken);
            if (token.AccessToken != null)
            {
                
                List<string> values = new List<string>();
                values.Add(sessionId);
                values.Add(token.AccessToken);
                
                CreateSessionForUser(user.UserId.ToString(), values);
            }
            

            return token.AccessToken;
        }

        

        public async Task<string> CreateTokenDoctorAsync(Doctor doctor)
        {
            string sessionId = Guid.NewGuid().ToString();
            List<Claim> claims = new List<Claim>
            {
                new Claim("sub", doctor.DoctorId.ToString()),
                new Claim("name", doctor.Name),
                new Claim("surname", doctor.Surname),
                new Claim("address", doctor.Address),
                new Claim("role", doctor.Role),
                new Claim("SessionId",sessionId),
                 


            };
            Token token = new Token();
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            token.ExpireDate = DateTime.Now.AddMinutes(double.Parse(_configuration.GetSection("AppSettings:Expiration").Value));
            JwtSecurityToken securityToken = new JwtSecurityToken(issuer: _configuration.GetSection("AppSettings:Issuer").Value, audience: _configuration.GetSection("AppSettings:Audience").Value,
              expires: token.ExpireDate, notBefore: DateTime.Now, signingCredentials: signingCredentials, claims: claims);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            token.AccessToken = handler.WriteToken(securityToken);
            if (token.AccessToken != null)
            {
                
                List<string> values = new List<string>();
                values.Add(sessionId);
                values.Add(token.AccessToken);
                CreateSessionForUser(doctor.DoctorId.ToString(), values);
            }

            return token.AccessToken;
        }
        public async Task<bool> ValidateTokenAndSessionAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["AppSettings:Token"]);

            try
            {
                // Token doğrulama parametreleri
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["AppSettings:Issuer"],
                    ValidAudience = _configuration["AppSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero 
                };

                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

               
                var sessionId = claimsPrincipal.FindFirst("SessionId")?.Value;
                var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                
                if (sessionId != null && userId != null)
                {
                   
                    var redisSessionId = _redisService.GetSessionIdAsync(userId);


                    //return sessionId == redisSessionId.ToString();
                    return true;
                }

                return false;
            }
            catch
            {
                return false; // Eğer token doğrulanmazsa false döner
            }
        }
    }
}
