using System;
using System.ComponentModel.DataAnnotations;

namespace Meridian.NotificationManagement.Core.Models
{
    public class Template
    {
        
        [Key]
        public string TemplateId { get; set; }
        public string TemplateBody { get; set; }
        public string TemplateSubject { get; set; }

    }
}