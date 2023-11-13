using System;
using System.Collections.Generic;

using Meridian.PlatformManagement.Core.Models;

namespace Meridian.PlatformManagement.Core.Interfaces.Repositories
{
    public interface IPlatformRepository
    {
        Platform GetPlatformDetail(int platformId);
    }
}
