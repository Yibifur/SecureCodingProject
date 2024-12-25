namespace OneDose.FirstProject.WebAPI.Security.Abstract
{
    public interface ITokenBlackListService
    {
        Task BlacklistTokenAsync(string token, TimeSpan expireTime);
        Task<bool> IsTokenBlacklistedAsync(string token);
    }
}
