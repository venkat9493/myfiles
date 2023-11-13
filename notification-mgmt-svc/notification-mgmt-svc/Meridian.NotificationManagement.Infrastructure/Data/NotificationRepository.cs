using System;
using System.Collections.Generic;
using System.Linq;
using Meridian.NotificationManagement.Core.Interfaces.Repositories;
using Meridian.NotificationManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Meridian.NotificationManagement.Infrastructure.Data
{
    public class NotificationRepository : INotoficationRepository
    {
        private readonly AppDbContext _dbContext;

        public NotificationRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Template CreateTemplate(Template template)
        {
            _dbContext.Add(template);
            _dbContext.SaveChanges();
            return template;
        }

        public Template GetTemplateDetail(string templateid)
        {

            var data = _dbContext.EmailTemplate.ToList();


            var list = _dbContext.EmailTemplate.FirstOrDefault(x => x.TemplateId == templateid);
            return list;
        }
    }
}
