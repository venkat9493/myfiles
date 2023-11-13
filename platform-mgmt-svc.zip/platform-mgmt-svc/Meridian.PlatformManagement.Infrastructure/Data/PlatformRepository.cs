using System;
using System.Collections.Generic;
using System.Linq;
using Meridian.PlatformManagement.Core.Interfaces.Repositories;
using Meridian.PlatformManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Meridian.PlatformManagement.Infrastructure.Data
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly AppDbContext _dbContext;

        public PlatformRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Platform GetPlatformDetail(int platformId)
        {
            return _dbContext.Platform.Find(platformId);
        }       

    }
}
