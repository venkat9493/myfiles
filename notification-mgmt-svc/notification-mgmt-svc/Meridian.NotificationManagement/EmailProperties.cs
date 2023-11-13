using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meridian.NotificationManagement
{

    public class EmailProperties
    {

        public string TargetMethod { get; set; }
        public string FromEmail { get; set; }

        //; separated
        public string ToEmail { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Templateid { get; set; }
        public List<TemplateProp> jsonArray { get; set; }

    }

    public class TemplateProp
    {
        public string prop { get; set; }
        public string value { get; set; }
           
    }

    public class NotificationMessage
    {
        public string TargetMethod { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string TemplateId { get; set; }
        public DonarRecipientImagesDetails DonarRecipientImageList { get; set; }

    }

    public class DonarRecipientImagesDetails
    {
        public string DonorUserId { get; set; }
        public string DonorUserDisplayName { get; set; }
        public string RecipientUserDisplayName { get; set; }
        public string Organization { get; set; }
        public string ContentDescription { get; set; }
        public string LinkToContentInMeridian { get; set; }
        public string ImageURLList { get; set; }

    }

}
