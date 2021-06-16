using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using DecisionRules.Exceptions;
using System.Net;

namespace DecisionRules
{
    public class DecisionRulesService
    {
        private readonly HttpClient client = new HttpClient();
        private readonly RequestOption globalOptions;

        public DecisionRulesService(RequestOption options)
        {
            globalOptions = options;
        }

        public U Solve<T, U>(String ruleId, T inputData, String version= default)
        {
            string url = UrlGenerator(ruleId, version);

            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", globalOptions.ApiKey);

                var jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = false };

                var request = JsonSerializer.Serialize(inputData);

                var response = client.PostAsync(url, new StringContent(request, Encoding.UTF8, "application/json")).Result;

                validateResponse(response);

                return JsonSerializer.Deserialize<U>(response.Content.ReadAsStringAsync().Result);
            }
            catch (NoUserException e)
            {
                throw e;
            }
            catch (TooManyApiCallsException e)
            {
                throw e;
            }
            catch (NotPublishedException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public U Solve<U>(String ruleId, string inputData, String version = default)
        {
            string url = UrlGenerator(ruleId, version);

            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", globalOptions.ApiKey);

                var response = client.PostAsync(url, new StringContent(inputData, Encoding.UTF8, "application/json")).Result;

                validateResponse(response);

                var test = response.Content.ReadAsStringAsync().Result;

                return JsonSerializer.Deserialize<U>(test);
            }
            catch (NoUserException e)
            {
                throw e;
            }
            catch (TooManyApiCallsException e)
            {
                throw e;
            }
            catch (NotPublishedException e)
            {
                throw e;
            }
            catch (ServerErrorException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private String UrlGenerator(String ruleId, String version)
        {
            String url;

            try
            {
                if (this.globalOptions.Geoloc != default)
                {
                    url = $"http://{this.globalOptions.Geoloc}.api.decisionrules.io/rule/solve/";
                }
                else
                {
                    url = "http://api.decisionrules.io/rule/solve/";
                }

                if (version != default)
                {
                    url += $"{ruleId}/{version}";

                }
                else
                {
                    url += $"{ruleId}";
                }

                return url;
            } 
            catch (Exception e)
            {
                throw e;
            }
            
        }

        private void validateResponse (HttpResponseMessage response)
        {
            if (response.StatusCode.Equals(HttpStatusCode.BadRequest))
            {
                throw new NotPublishedException();
            }
            else if (response.StatusCode.Equals(HttpStatusCode.UpgradeRequired))
            {
                throw new TooManyApiCallsException();
            }
            else if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                throw new NoUserException();
            }
            else if (response.StatusCode.Equals(HttpStatusCode.InternalServerError) || response.StatusCode.Equals(HttpStatusCode.ServiceUnavailable))
            {
                throw new ServerErrorException();
            }
        }
    }
}
