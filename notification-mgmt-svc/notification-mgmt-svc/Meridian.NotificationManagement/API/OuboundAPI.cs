using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Meridian.NotificationManagement.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Meridian.NotificationManagement.API
{

    //Call all outbound APIs from here
    public class OutboundAPI
    {

        public Template GetTemplatedataAPI(NotificationMessage emailProperties)
        {
            HttpClient client = new HttpClient();

            //client.BaseAddress = new Uri("http://localhost:33570/template");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //http get
            //List<TemplateProp> lsttemplate = emailProperties.jsonArray;
            //TemplateProp tprop = lsttemplate.FirstOrDefault(x => x.prop == "Templateid");
            string templateid = emailProperties.TemplateId;
            HttpResponseMessage response = client.GetAsync("http://aafd34436cd60442c8ad699d8d0a0a1a-41185143.us-west-2.elb.amazonaws.com/api/Template/" + templateid).Result;

            Template emailtemplateobj = null;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                emailtemplateobj = response.Content.ReadAsAsync<Template>().Result;

                //var template = JsonConvert.DeserializeObject<Template>(response.Content.ReadAsStringAsync().Result);


            }

            return emailtemplateobj;

        }

        public string GetfromAWSSecret(string secretName)
        {
            var client = new AmazonSecretsManagerClient("AKIA2S2JIPDP4DCUE354", "vjP0Ag7fYDuyO+49pqs0r5GJ3vo1UZOFgQsS4VHX", 
                                                             RegionEndpoint.APSoutheast2);

            var request = new GetSecretValueRequest
            {
                // this gets your secret name, 'web-api/passwords/database' in our case
                SecretId = secretName
            };

            GetSecretValueResponse response = null;
            string secretString = "";
                
            try
            {
                response = client.GetSecretValueAsync(request).Result;
                
            }           
           
            catch (DecryptionFailureException e)
            {
                // Secrets Manager can't decrypt the protected secret text using the provided KMS key.
                throw e;
            }
            MemoryStream memoryStream = new MemoryStream();
            if (response.SecretString != null)
            {
                secretString = response.SecretString;
            }
            else
            {
                memoryStream = response.SecretBinary;
                StreamReader reader = new StreamReader(memoryStream);
                secretString = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
            }
            return secretString;

        }

    }
}
