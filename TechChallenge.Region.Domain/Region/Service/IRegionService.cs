using TechChallange.Region.Domain.Region.Entity;

namespace TechChallange.Region.Domain.Region.Service
{
    public interface IRegionService
    {
        Task CreateAsync(RegionEntity regionEntity);
    }
}
