namespace ClubSync.Api.Database.Seeders
{
    public interface ISeeder
    {
        abstract static Task RunAsync(IServiceProvider serviceProvider);
    }
}