using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace OneDose.FirstProject.WebAPI.Caching
{
    public class RedisCacheService :IRedisCacheService
    {
        private const string BlacklistSetKey = "blacklisted_tokens";
        private readonly string _host;
        private readonly int _port;
        private  IConnectionMultiplexer _redis;
        private readonly IDatabase _db;
        private readonly IConfiguration _configuration;
        private TimeSpan ExpireTime => TimeSpan.FromMinutes(double.Parse(_configuration.GetSection("AppSettings:Expiration").Value));

        public RedisCacheService(string host, int port, IConfiguration configuration)
        {
            // Redis bağlantısını host ve port bilgileriyle kuruyoruz
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
            _configuration = configuration;
        }
        public async Task<bool> SetSessionIdAsync(string userId, List<string> values)
        {
            if(await _db.KeyExistsAsync(userId))
            {
               
            }
            if (string.IsNullOrEmpty(userId) || values == null || values.Count == 0)
            {
                return false; // Geçersiz giriş durumunda false döndür
            }

            // Mevcut listenin uzunluğunu al
            long listLength = await _db.ListLengthAsync(userId);

            for (int i = 0; i < values.Count; i++)
            {
                if (i < listLength)
                {
                    // Mevcut bir elemanı güncelle
                    await _db.ListSetByIndexAsync(userId, i, values[i]);
                    await _db.KeyExpireAsync(userId, ExpireTime);
                }
                else
                {
                    // Eğer indis mevcut listenin uzunluğundan büyükse, listenin sonuna ekle
                    await _db.ListRightPushAsync(userId, values[i]);
                    await _db.KeyExpireAsync(userId, ExpireTime);
                }
            }

            return true; // Başarılı işlem durumunda true döndür
        }


        // Belirtilen userId'ye göre sessionId'leri alır (GetSessionIdAsync)
        public string GetSessionIdAsync(string userId)
        {
            // Kullanıcı ID'si kontrolü
            if (string.IsNullOrEmpty(userId))
            {
                return null; // Eğer userId boş ise, geçersiz değer döndür.
            }

            // Redis listesindeki ilk elemanı getir
            var items =  _db.ListRange(userId, 0, 0);
            if (items.Length > 0)
            {
                // İlk elemanı döndür (sessionId)
                return items[0].ToString();
            }

            // Eğer liste boşsa veya sessionId bulunamazsa null döndür
            return null;
        }
        public List<string> GetAsync(string userId)
        {
            // Kullanıcı ID'si kontrolü
            if (string.IsNullOrEmpty(userId))
            {
                return null; // Eğer userId boş ise, geçersiz değer döndür.
            }

            // Redis listesindeki tüm elemanları getir
            var items = _db.ListRange(userId);
            if (items.Length > 0)
            {
                // Elemanları string listesi olarak dönüştür ve döndür
                return items.Select(item => item.ToString()).ToList();
            }

            // Eğer liste boşsa null döndür
            return null;
        }
        // Belirtilen anahtara ait veriyi siler (Clear)
        public async Task<bool> Clear(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            // Redis'teki ilgili anahtarı sil
            return await _db.KeyDeleteAsync(key);
        }

        // Tüm verileri temizler (ClearAll)
        public void ClearAll()
        {
            var server = _db.Multiplexer.GetServer(_db.Multiplexer.GetEndPoints()[0]);

            foreach (var key in server.Keys())
            {
                _db.KeyDelete(key);
            }
        }
        
        public void Connect() => _redis = ConnectionMultiplexer.Connect($"{_host}:{_port}");

       
    }
}
