using System;
using System.Collections.Generic;

using Meridian.NotificationManagement.Core.Models;

namespace Meridian.NotificationManagement.Core.Interfaces.Services
{
    public interface INotificationService
    {
       Template GetTemplateDetail(string templateid);
        Template CreateTemplate(Template template);
    }
}
