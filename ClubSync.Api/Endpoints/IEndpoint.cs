namespace ClubSync.Api.Endpoints
{
    public interface IEndpoint
    {
        abstract static void Map(IEndpointRouteBuilder builder);
    }
}