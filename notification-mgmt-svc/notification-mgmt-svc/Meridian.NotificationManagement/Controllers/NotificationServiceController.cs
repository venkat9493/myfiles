using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Meridian.NotificationManagement.Core.Models;
using System.Text.RegularExpressions;

namespace Meridian.NotificationManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationServiceController : ControllerBase
    {
        [HttpPost("SendNotification")]
        public IActionResult Post([FromBody] EmailProperties emailProperties)
        {


            if (emailProperties.TargetMethod.Equals("Email",StringComparison.OrdinalIgnoreCase))
            {
                Template templateobj = GetTemplatedata(emailProperties);

                string replacedtextbody = ReplaceTemplatedatawithtext(emailProperties, templateobj.TemplateBody);
                string replacedtextsubject = ReplaceTemplatedatawithtext(emailProperties, templateobj.TemplateSubject);

                string content1 = replacedtextsubject + emailProperties.Subject + "\n";
                string content2 = replacedtextbody + emailProperties.Body + "\n";


                //System.IO.File.AppendAllText(@"C:\Project\DataStore\EmailContent.txt", content1);
                //System.IO.File.AppendAllText(@"C:\Project\DataStore\EmailContent.txt", content2);

                //Loop a json array
                //List<TemplateProp> jsonlist = emailProperties.jsonArray;
                ////Write json array properties to a file
                //foreach(TemplateProp templateProp in jsonlist)
                //{
                //    string contents = templateProp.prop + ":" + templateProp.value + "\n";
                //    System.IO.File.AppendAllText(@"C:\Project\DataStore\jsonprop.txt", contents);
                //}



                //put in the message queue
                //Call the email API
                //call the template api
                //string url = "http://localhost:33573";
            }

            return Ok();

        }
        private void CallEmailAPI(EmailProperties emailProperties)
        {
            HttpClient client = new HttpClient();
           
        }

        private string ReplaceTemplatedatawithtext(EmailProperties emailprop, string texttoreplace)
        {
            //replace placeholders with the text

           
            string templatetext = texttoreplace;
            MatchCollection matchcoll = Regex.Matches(templatetext, @"\[([^]]+)\]");
            List<string> bracketstringlist = new List<string>();
            
            foreach(Match match in matchcoll)
            {
                //string exttext = match.Value;
                bracketstringlist.Add(match.Value);
                //Get string without spaces and square brackets
                string modifiedtext = match.Value;

                string truncatedtext = Regex.Replace(modifiedtext, @"\s+", "");
                truncatedtext = truncatedtext.Replace("[", "").Replace("]", ""); //remove braces;

                TemplateProp templateprop =  emailprop.jsonArray.FirstOrDefault(x => x.prop.ToLower() == truncatedtext.ToLower());

                if (templateprop != null)
                    templatetext = templatetext.Replace(modifiedtext, templateprop.value);

            }

            return templatetext;

        }

        private Template GetTemplatedata(EmailProperties emailProperties)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:33573/api/Template/GetTemplate");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //http get
            List<TemplateProp> lsttemplate = emailProperties.jsonArray;
            TemplateProp tprop =  lsttemplate.FirstOrDefault(x => x.prop == "Templateid");
            string templateid = emailProperties.Templateid;
            HttpResponseMessage response = client.GetAsync("?templateid=" + templateid).Result;
            Template emailtemplateobj = null;
            if (response.IsSuccessStatusCode)
            {
                emailtemplateobj = JsonConvert.DeserializeObject<Template>(response.Content.ReadAsStringAsync().Result);
            }

            return emailtemplateobj;

        }

      
    }
}
