using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallange.Region.Domain.Region.Entity;
using TechChallange.Region.Domain.Region.Exception;
using TechChallange.Region.Domain.Region.Repository;

namespace TechChallange.Region.Domain.Region.Service
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository _regionRepository;

        public RegionService(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        public async Task CreateAsync(RegionEntity regionEntity)
        {
            var regionDb = await _regionRepository.GetByDddAsync(regionEntity.Ddd).ConfigureAwait(false);

            if (regionDb != null)
                throw new RegionAlreadyExistsException();

            await _regionRepository.AddAsync(regionEntity).ConfigureAwait(false);
        }
    }
}
