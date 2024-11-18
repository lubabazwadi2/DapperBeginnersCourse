using Dapper;
using DapperBeginnersCourse.Models;
using Npgsql;
using System.Data;

namespace DapperBeginnersCourse.Repositories
{
    public class VideoGameRepository : IVideoGameRepository
    {
        private readonly IConfiguration _configuration;

        public VideoGameRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task AddAsync(VideoGame videoGame)
        {
            using var connection = GetConnection();
            var query = @"
                INSERT INTO VideoGames (Title, Publisher, Developer, ReleaseDate) 
                VALUES (@Title, @Publisher, @Developer, @ReleaseDate) 
                RETURNING Id;";
            int newId = await connection.QuerySingleAsync<int>(query, videoGame);
            videoGame.Id = newId; // Update the object's ID with the newly generated ID
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = GetConnection();
            await connection.ExecuteAsync(
                "DELETE FROM VideoGames WHERE Id = @Id",
                new { Id = id });
        }

        public async Task<List<VideoGame>> GetAllAsync()
        {
            using var connection = GetConnection();
            var videoGames = await connection.QueryAsync<VideoGame>("SELECT * FROM VideoGames");
            return videoGames.ToList();
        }

        public async Task<VideoGame> GetByIdAsync(int id)
        {
            using var connection = GetConnection();
            var videoGame = await connection.QueryFirstOrDefaultAsync<VideoGame>(
                "SELECT * FROM VideoGames WHERE Id = @Id",
                new { Id = id });
            return videoGame;
        }

        public async Task UpdateAsync(VideoGame videoGame)
        {
            using var connection = GetConnection();
            await connection.ExecuteAsync(
                @"UPDATE VideoGames 
                  SET Title = @Title, 
                      Publisher = @Publisher, 
                      Developer = @Developer, 
                      ReleaseDate = @ReleaseDate 
                  WHERE Id = @Id",
                videoGame);
        }

        // Corrected GetConnection method to use NpgsqlConnection
        private IDbConnection GetConnection()
        {
            return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
