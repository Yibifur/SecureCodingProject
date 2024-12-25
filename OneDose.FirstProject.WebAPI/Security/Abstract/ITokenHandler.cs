using OneDose.FirstProject.EntityLayer.Concrete;

namespace OneDose.FirstProject.WebAPI.Security.Abstract
{
    public interface ITokenHandler
    {
        Task<string> CreateTokenAsync(User user);
        Task<string> CreateTokenDoctorAsync(Doctor doctor);
        Task<bool> ValidateTokenAndSessionAsync(string token);
        void CreateSessionForUser(string userId,List<string> values);
        
    }
}
