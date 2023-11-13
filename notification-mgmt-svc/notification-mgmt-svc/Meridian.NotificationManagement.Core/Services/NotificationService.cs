using System;
using System.Collections.Generic;
using Meridian.NotificationManagement.Core.Interfaces.Services;
using Meridian.NotificationManagement.Core.Interfaces.Repositories;

using Meridian.NotificationManagement.Core.Models;

namespace Meridian.NotificationManagement.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotoficationRepository _notificationRepository;

        public NotificationService(INotoficationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public Template CreateTemplate(Template template)
        {
            throw new NotImplementedException();
        }

        public Template GetTemplateDetail(string templateid)
        {
           return _notificationRepository.GetTemplateDetail(templateid);
        }
    }
}
