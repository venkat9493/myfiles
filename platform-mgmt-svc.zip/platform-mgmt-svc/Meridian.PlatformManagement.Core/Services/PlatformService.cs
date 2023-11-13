using System;
using System.Collections.Generic;
using Meridian.PlatformManagement.Core.Interfaces.Services;
using Meridian.PlatformManagement.Core.Interfaces.Repositories;

using Meridian.PlatformManagement.Core.Models;

namespace Meridian.PlatformManagement.Infrastructure.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly IPlatformRepository _platformRepository;

        public PlatformService(IPlatformRepository platformRepository)
        {
            _platformRepository = platformRepository;
        }

        public Platform GetPlatformDetail(int platformId)
        {
            return _platformRepository.GetPlatformDetail(platformId);
        }
    }
}
