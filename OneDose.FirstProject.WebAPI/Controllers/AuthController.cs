using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OneDose.FirstProject.BusinessLayer.Abstract;
using OneDose.FirstProject.DtoLayer.DoctorDtos;
using OneDose.FirstProject.DtoLayer.UserDtos;
using OneDose.FirstProject.EntityLayer.Concrete;
using OneDose.FirstProject.WebAPI.Caching;
using OneDose.FirstProject.WebAPI.Security;
using OneDose.FirstProject.WebAPI.Security.Abstract;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;


namespace OneDose.FirstProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
      private readonly IUserService _userService;
        private readonly IDoctorService _doctorService;
      private readonly ITokenHandler _tokenHandler;
        private readonly IRedisCacheService _redisCacheService;
        private readonly ITokenBlackListService _tokenBlackListService;
        private readonly IConfiguration _configuration;
        private TimeSpan ExpireTime => TimeSpan.FromMinutes(double.Parse(_configuration.GetSection("AppSettings:Expiration").Value));


        public AuthController(IUserService userService, ITokenHandler _tokenHandler, IDoctorService doctorService, IRedisCacheService redisCacheService, ITokenBlackListService tokenBlackListService, IConfiguration configuration)
        {
            this._userService = userService;

            this._tokenHandler = _tokenHandler;
            _doctorService = doctorService;
            _redisCacheService = redisCacheService;
            _tokenBlackListService = tokenBlackListService;
            _configuration = configuration;
        }



        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto model)
        {
            User user = new User()
            {
                Address = model.Address,
                Age = model.Age,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname,
                Role="kullanici"
                
                
               };  
                



           
            user.PasswordHash=BCrypt.Net.BCrypt.HashPassword(model.Password);
            await _userService.TInsertAsync(user);
            return Ok("User eklendi");

        }

        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser(LoginUserDto loginUser)
        {
            try
            {
                // Validate input
                if (loginUser == null || string.IsNullOrEmpty(loginUser.Email))
                {
                    return BadRequest("Geçersiz giriş. Email gerekli.");
                }

                var user = await _userService.TGetUserByEmailAsync(loginUser.Email);

                if (user == null)
                {
                    return BadRequest("Kullanıcı Bulunamadı");
                }

                if (!BCrypt.Net.BCrypt.Verify(loginUser.Password, user.PasswordHash))
                {
                    return BadRequest("Yanlış Şifre");
                }

                // Generate token
                string token = await _tokenHandler.CreateTokenAsync(user);

                // Validate token
                if (!await _tokenHandler.ValidateTokenAndSessionAsync(token))
                {
                    return Unauthorized("Token doğrulanamadı, oturum yetkisiz.");
                }

                return Ok(token);
            }
            catch (Exception ex)
            {
                // Log the error for debugging purposes
                Console.WriteLine($"Error in LoginUser: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Sunucu hatası.");
            }
        }


        [HttpPost("RegisterDoctor")]
        public async Task<IActionResult> RegisterDoctor(RegisterDoctorDto model)
        {
            Doctor doctor = new Doctor()
            {
                Address = model.Address,
                Age = model.Age,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname,
                Role = "doktor",
                LicenseNumber = model.LicenseNumber,
                Hospital=model.Hospital,
                Specialization = model.Specialization
            };





            doctor.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
           await _doctorService.TInsertAsync(doctor);
            return Ok("User eklendi");

        }
        [HttpPost("LoginDoctor")]
        public async Task<IActionResult> LoginUser(LoginDoctorDto loginDoctor)
        {

            var doctor = await _doctorService.TGetDoctorByEmailAsync(loginDoctor.Email);
            var isRegistered = false;
            if (doctor == null)
            {
                return BadRequest("Kullanıcı Bulunamadı");
            }
            if (!BCrypt.Net.BCrypt.Verify(loginDoctor.Password, doctor.PasswordHash))
            {

                return BadRequest("Yanlış Şifre");

            }
            if (BCrypt.Net.BCrypt.Verify(loginDoctor.Password, doctor.PasswordHash))
            {

                isRegistered = true;

            }





            string token = await _tokenHandler.CreateTokenDoctorAsync(doctor);
            return Ok(token);

        }

        //[HttpPost("LogoutUser")]
        //public async Task<IActionResult> Delete(string key)
        //{
        //    string fullKey = $"{key}";
        //    var json = _redisCacheService.GetAsync(key);
        //    //_tokenBlackListService.BlacklistTokenAsync(json[1], ExpireTime);

        //    bool isDeleted = await _redisCacheService.Clear(fullKey);

        //    if (isDeleted)
        //    {
        //        return Ok("Key başarıyla silindi.");
        //    }
        //    else
        //    {
        //        return Ok("Key silinemedi veya bulunamadı.");
        //    }


        //}


        [HttpPost("LogoutUser")]
        public async Task<IActionResult> Delete(string key)
        {
            try
            {
                return Ok("Key işlemi başarıyla tamamlandı.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("GetUser")]
        public async Task<IActionResult> Get(string key)
        {
            return Ok(_redisCacheService.GetSessionIdAsync(key));
        }

        [HttpPost("SetUser")]
        public async Task<IActionResult> Set(string key, List<string> value)
        {
            await _redisCacheService.SetSessionIdAsync(key, value);
            return Ok();
        }
        //[HttpPost("LogoutDoctor")]
        //public async Task<IActionResult> DeleteDoctor(string key)
        //{
        //    string fullKey = $"{key}";
        //    var json = _redisCacheService.GetAsync(key);
        //    _tokenBlackListService.BlacklistTokenAsync(json[1], ExpireTime);

        //    bool isDeleted = await _redisCacheService.Clear(fullKey);

        //    if (isDeleted)
        //    {
        //        return Ok("Key başarıyla silindi.");
        //    }
        //    else
        //    {
        //        return Ok("Key silinemedi veya bulunamadı.");
        //    }


        //}
        [HttpPost("LogoutDoctor")]
        public async Task<IActionResult> DeleteDoctor(string key)
        {
            try
            {
                return Ok("Key işlemi başarıyla tamamlandı.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("GetDoctor")]
        public async Task<IActionResult> GetDoctor(string key)
        {
            return Ok(_redisCacheService.GetSessionIdAsync(key));
        }

        [HttpPost("SetDoctor")]
        public async Task<IActionResult> SetDoctor(string key, List<string> value)
        {
            await _redisCacheService.SetSessionIdAsync(key, value);
            return Ok();
        }

    }
    
}
