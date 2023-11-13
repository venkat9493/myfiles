using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Meridian.NotificationManagement.Validation
{
    public class InputRequestValidation
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public InputRequestValidation ValidateInputParameter(NotificationMessage emailProperties)
        {
            //check for email validation 
            InputRequestValidation irvobj = new InputRequestValidation();
            try
            {
                string[] emailList = emailProperties.ToEmail.Split(";");
                foreach (string email in emailList)
                {
                    MailAddress mailaddress = new MailAddress(email);
                }
                if (!string.IsNullOrEmpty(emailProperties.CC))
                {
                    string[] emailList1 = emailProperties.ToEmail.Split(";");
                    foreach (string email1 in emailList1)
                    {
                        MailAddress mailaddress1 = new MailAddress(emailProperties.CC);
                    }
                }
                if (!string.IsNullOrEmpty(emailProperties.BCC))
                {
                    string[] emailList2 = emailProperties.ToEmail.Split(";");
                    foreach (string email2 in emailList2)
                    {
                        MailAddress mailaddress2 = new MailAddress(emailProperties.BCC);
                    }
                }

                ErrorCode = 200;
            }
            catch (Exception ex)
            {
                ErrorCode = 400;
                ErrorMessage = "EmailId not in correct format";
            }
            return irvobj;


        }
    }
}
