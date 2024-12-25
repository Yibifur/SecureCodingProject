namespace OneDose.FirstProject.WebAPI.Caching
{
    public interface IRedisCacheService
    {
        Task<bool> SetSessionIdAsync(string userId, List<string> values);

        string GetSessionIdAsync(string userId);
        Task<bool> Clear(string key);
        void ClearAll();
        public List<string> GetAsync(string userId);
    }
}
