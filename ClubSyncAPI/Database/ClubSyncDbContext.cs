using Microsoft.EntityFrameworkCore;
namespace ClubSyncAPI.Database
{
    public class ClubSyncDbContext : DbContext
    {
        public ClubSyncDbContext(DbContextOptions<ClubSyncDbContext> options) : base(options)
        {

        }
    }
}