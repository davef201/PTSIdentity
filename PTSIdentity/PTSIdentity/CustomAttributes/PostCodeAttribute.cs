using Newtonsoft.Json;
using PTSIdentity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace PTSIdentity.CustomAttributes
{
    public class PostCodeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string apiKey = ConfigurationManager.AppSettings["getAddressKey"];

            HttpClient httpClient = new HttpClient();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var builder = new UriBuilder("https://api.getAddress.io/find/" + value.ToString());
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["api-key"] = apiKey;

            builder.Query = query.ToString();
            string url = builder.ToString();
            HttpResponseMessage m = httpClient.GetAsync(url).Result;
            if (m.IsSuccessStatusCode)
            {
                var addresses = m.Content.ReadAsStringAsync().Result;
                AddressIOResponse addressList = JsonConvert.DeserializeObject<AddressIOResponse>(addresses);
                if (addressList.addresses.Count() > 0)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("The postcode supplied does not exist");
                }
            }
            else
            {
                return new ValidationResult("The postcode supplied does not exist");
            }

        }
    }
}
