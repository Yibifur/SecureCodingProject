using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using OneDose.FirstProject.WebAPI.Caching;
using OneDose.FirstProject.WebAPI.Security.Abstract;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;

namespace OneDose.FirstProject.WebAPI.Security
{
    public class TokenBlackListService:ITokenBlackListService
    {
        private readonly string _host;
        private readonly int _port;
        private IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public TokenBlackListService( string host, int port)
        {

            
            
            _host = host;
            _port = port;
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { $"{host}:{port}" },
                AbortOnConnectFail = false  // Bağlantı kesildiğinde yeniden denesin
            };

            // ConnectionMultiplexer'ı oluştur
            _redis = ConnectionMultiplexer.Connect(configurationOptions);
            _db = _redis.GetDatabase();  // Redis veritabanına bağlan
        }

        // Token'ı kara listeye ekler
        public async Task BlacklistTokenAsync(string token, TimeSpan expireTime)
        {
            await _db.SetAddAsync("blacklist_tokens", token);
            //await _db.ListSetByIndexAsync(token, 2, "blacklisted");
        }

        // Token'ın kara listede olup olmadığını kontrol eder
        public  async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                // Token'ı çözümle ve içindeki veriyi bir JwtSecurityToken nesnesine aktar
                var jwtToken = handler.ReadJwtToken(token);

                // Claim'leri kullanarak userId'yi (örneğin 'sub' claim) elde edin
                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub).ToString();
                string trimmedValue = userIdClaim.Substring(5);
                if (await _db.KeyExistsAsync(trimmedValue))
                {

                    var entity = await _db.ListRangeAsync(trimmedValue);
                    if (entity[1].ToString() == token)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }

                
                 
            }

           

            return false;   

            //var value = await _db.ListGetByIndexAsync(token,1);
            //return value == "blacklisted";
        }
        public void Connect() => _redis = ConnectionMultiplexer.Connect($"{_host}:{_port}");
    }
}
