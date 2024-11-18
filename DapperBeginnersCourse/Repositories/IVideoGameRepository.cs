using DapperBeginnersCourse.Models;

namespace DapperBeginnersCourse.Repositories
{
    public interface IVideoGameRepository
    {
        Task<List<VideoGame>> GetAllAsync();
        Task<VideoGame> GetByIdAsync(int id);
        Task AddAsync(VideoGame videoGame);
        Task UpdateAsync(VideoGame videoGame);
        Task DeleteAsync(int id);
    }
}
