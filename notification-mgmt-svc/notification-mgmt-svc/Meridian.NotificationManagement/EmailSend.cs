using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Meridian.NotificationManagement.Controllers;
using Meridian.NotificationManagement.Model;
using System.Text;
using System.Net.Mime;
using Meridian.NotificationManagement.Core.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Meridian.NotificationManagement.API;

namespace Meridian.NotificationManagement
{
    public class EmailSend
    {
        static object _lock = new object();
        public bool SendEmail(NotificationMessage emailProperties)
        {
            var config = new ConfigurationBuilder()
                   .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                   .AddJsonFile("appsettings.json").Build();

            //string smtpadd = config.GetValue<string>("AppSettings:smtpurl");
            // string smtpusername = config.GetValue<string>("AppSettings:smtpusername");
            //read froim appsettings.json
            string smtpUserID = config.GetValue<string>("AppSettings:smtpusername");
            string smtppwd = config.GetValue<string>("AppSettings:smtppwd");
            string smtpurl = config.GetValue<string>("AppSettings:smtpurl");
            MailMessage mailmsg = new MailMessage();
            string [] toEmailList = emailProperties.ToEmail.Split(";");
            foreach (string email in toEmailList)
            {
                mailmsg.To.Add(email);
            }

            if (!string.IsNullOrEmpty(emailProperties.CC))
            {
                string[] ccEmailList = emailProperties.CC.Split(";");
                foreach (string email1 in ccEmailList)
                {
                    mailmsg.CC.Add(email1);
                }
            }

            if (!string.IsNullOrEmpty(emailProperties.BCC))
            {
                string[] bccEmailList = emailProperties.BCC.Split(";");
                foreach (string email2 in bccEmailList)
                {
                    mailmsg.Bcc.Add(email2);
                }
            }

            mailmsg.From = new MailAddress(smtpUserID);
            mailmsg.Body = emailProperties.Body;
            mailmsg.Subject = emailProperties.Subject;
            mailmsg.IsBodyHtml = true;

            try
            {

                SmtpClient smtpClient = new SmtpClient(smtpurl, 587)
                {
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(smtpUserID, smtppwd),
                    EnableSsl = true

                };
                OutboundAPI outAPI = new OutboundAPI();
                Template templateobj = outAPI.GetTemplatedataAPI(emailProperties);
                //string replacedtextbody = ReplaceTemplatedatawithtext(emailProperties, templateobj.templatedata);

                string replacedtextbody = ReplaceEmailTemplateText(emailProperties, templateobj.TemplateBody);

                string replacedtextsubject = ReplaceEmailTemplateText(emailProperties, templateobj.TemplateSubject);

                string emailsubject = replacedtextsubject + " " + emailProperties.Subject;
                string emailbody = replacedtextbody + emailProperties.Body + "\n";
                mailmsg.Subject = emailsubject;
                mailmsg.Body = emailbody;

                string currentdir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                mailmsg.AlternateViews.Add(CreateHtmlMessage(currentdir, emailbody));//for adding images

                lock (_lock)
                {
                    smtpClient.Send(mailmsg);
                    mailmsg.Dispose();
                    smtpClient.Dispose();
                }
                return true;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                return false;
            }

        }
       

        public AlternateView CreateHtmlMessage(string imagepath,string emailBody)
        {
            string htmlmessage = string.Format(@"<html><body><img src = 'cid:header'/>{0}<br/><br/><img src = 'cid:footer' style='width:100%;height:100%' /></body></html>",emailBody);

            AlternateView altview = AlternateView.CreateAlternateViewFromString(htmlmessage, Encoding.UTF8, MediaTypeNames.Text.Html);
            var footer = new LinkedResource(imagepath+@"/image/footer.png");
            footer.ContentId = "footer";
            footer.ContentLink = new Uri("cid:" + footer.ContentId);
            footer.ContentType.Name = footer.ContentId;
            altview.LinkedResources.Add(footer);

            var header = new LinkedResource(imagepath + @"/image/link message.png");
            header.ContentLink = new Uri("cid:" + header.ContentId);
            header.ContentId = "header";
            header.ContentType.Name = header.ContentId;
            altview.LinkedResources.Add(header);

            return altview;

        }

        private string ReplaceTemplatedatawithtext(EmailProperties emailprop, string texttoreplace)
        {
            //replace placeholders with the text


            string templatetext = texttoreplace;
            MatchCollection matchcoll = Regex.Matches(templatetext, @"\[([^]]+)\]");
            List<string> bracketstringlist = new List<string>();

            foreach (Match match in matchcoll)
            {
                //string exttext = match.Value;
                bracketstringlist.Add(match.Value);
                //Get string without spaces and square brackets
                string modifiedtext = match.Value;

                string truncatedtext = Regex.Replace(modifiedtext, @"\s+", "");
                truncatedtext = truncatedtext.Replace("[", "").Replace("]", ""); //remove braces;

                TemplateProp templateprop = emailprop.jsonArray.FirstOrDefault(x => x.prop.ToLower() == truncatedtext.ToLower());

                if (templateprop != null)
                    templatetext = templatetext.Replace(modifiedtext, templateprop.value);
                


            }

            return templatetext;

        }

        private string ReplaceEmailTemplateText(NotificationMessage notificationMessage , string replaceText)
        {


            DonarRecipientImagesDetails dridetails = notificationMessage.DonarRecipientImageList;

            string modifiedText = replaceText;

            //replace all the text in the template
            modifiedText = modifiedText.Replace("[Donor User Display Name]", dridetails.DonorUserDisplayName, StringComparison.OrdinalIgnoreCase);
            modifiedText = modifiedText.Replace("[Recipient User Display Name]", dridetails.RecipientUserDisplayName,
                                                                                 StringComparison.OrdinalIgnoreCase);
            modifiedText = modifiedText.Replace("[Organization]", dridetails.Organization,StringComparison.OrdinalIgnoreCase);
            modifiedText = modifiedText.Replace("[Content Description]", dridetails.ContentDescription,
                                                                         StringComparison.OrdinalIgnoreCase);
            modifiedText = modifiedText.Replace("[Link to Content in Meridian]", dridetails.LinkToContentInMeridian, 
                                                                                 StringComparison.OrdinalIgnoreCase);
            modifiedText = modifiedText.Replace("[Image URL List]", dridetails.ImageURLList, 
                                                                                    StringComparison.OrdinalIgnoreCase);

            return modifiedText;


        }

        string GetHtmlEmailtemplate(string emailtemplatype,EmailProperties eprop)
        {
            //TemplateDbcontext dbContext = EmailNotificationController._dbContext;

            //var list = dbContext.templateinfo.ToList();

           // var emailtemp = dbContext.templateinfo.FirstOrDefault(x => x.templateid == eprop.TemplateId);


            //if (emailtemp == null)
            {
                //return "Template not found";
            }

            //string emailtemplatetype;
            string path = string.Format(@"C:\Project\DataStore\{0}.txt", emailtemplatype);
            string templatetext = File.ReadAllText(path);
            string outtext = ReplaceTempletewithText(templatetext,eprop);
            return outtext;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputtext"></param>
        /// <param name="eprop"></param>
        /// <returns></returns>
        private string  ReplaceTempletewithText(string inputtext,EmailProperties eprop)
        {
            //string outputdata = "";
            

            //[Role] [Organization] [User Display Name]
            //replace 
            //string modtext = inputtext.Replace("[Role]", eprop.Role);
            //modtext = modtext.Replace("[Organization]", eprop.Org);
            //modtext = modtext.Replace("[User Display Name]", eprop.DisplayName);
            //modtext = modtext.Replace("[Product Name]", eprop.ProductName);

            return "";
        }

       
    }
}
