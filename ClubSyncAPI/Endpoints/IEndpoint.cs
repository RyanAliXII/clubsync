namespace ClubSyncAPI.Endpoints
{
    public interface IEndpoint
    {
        abstract static void Map(IEndpointRouteBuilder builder);
    }
}