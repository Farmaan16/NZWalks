using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
       Task<Walk> createAsync(Walk walk);

    }
}
