using System;
using System.Collections.Generic;

using Meridian.NotificationManagement.Core.Models;

namespace Meridian.NotificationManagement.Core.Interfaces.Repositories
{
    public interface INotoficationRepository
    {
        Template GetTemplateDetail(string templateid);
        //NotificationTemplate CreateTemplate(NotificationTemplate template);
    }
}
