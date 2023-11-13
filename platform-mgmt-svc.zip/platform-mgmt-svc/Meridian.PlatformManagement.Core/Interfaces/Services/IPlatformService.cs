using System;
using System.Collections.Generic;

using Meridian.PlatformManagement.Core.Models;

namespace Meridian.PlatformManagement.Core.Interfaces.Services
{
    public interface IPlatformService
    {
        Platform GetPlatformDetail(int platformId);
    }
}
