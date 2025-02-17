using Microsoft.EntityFrameworkCore;
namespace ClubSync.Api.Database
{
    public class ClubSyncDbContext : DbContext
    {
        public ClubSyncDbContext(DbContextOptions<ClubSyncDbContext> options) : base(options)
        {

        }
    }
}