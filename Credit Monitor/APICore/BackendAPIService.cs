using System.Net;
using System.Text;
using Credit_Monitor.Interfaces;
using Newtonsoft.Json;

namespace Credit_Monitor.APICore
{
    public class BackendAPIService : IBackendAPIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly JsonSerializerSettings _newtonsoftSettings;

        public BackendAPIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            var baseUrl = _configuration["BackendAPI:BaseUrl"];
            if (!string.IsNullOrEmpty(baseUrl))
            {
                _httpClient.BaseAddress = new Uri(baseUrl);
            }

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            // Configure Newtonsoft to handle $id and $values metadata automatically
            _newtonsoftSettings = new JsonSerializerSettings
            {
                // Use the explicit Newtonsoft Enum path
                MetadataPropertyHandling = Newtonsoft.Json.MetadataPropertyHandling.ReadAhead,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
            };
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            IgnoreSSL();
            var response = await _httpClient.GetAsync(endpoint);
            await HandleErrors(response);

            var jsonString = await response.Content.ReadAsStringAsync();

            // Newtonsoft doesn't care if $id is the first property or the last
            var result = JsonConvert.DeserializeObject<T>(jsonString, _newtonsoftSettings);

            if (result is null) throw new InvalidOperationException("API response content was null.");
            return result;
        }

        public async Task<T> PostAsync<T>(object payload, string endpoint)
        {
            IgnoreSSL();

            // Serialize payload with Newtonsoft
            var jsonPayload = JsonConvert.SerializeObject(payload, _newtonsoftSettings);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            await HandleErrors(response);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(jsonResponse, _newtonsoftSettings);

            if (result is null) throw new InvalidOperationException("API response content was null.");
            return result;
        }

        private static async Task HandleErrors(HttpResponseMessage response)
        {
            // If it's a success (200-299), we are good.
            if (response.IsSuccessStatusCode) return;

            // If it's a 401 (Unauthorized) or 404 (Not Found), 
            // do NOT throw an exception. We want to read the JSON body!
            if (response.StatusCode == HttpStatusCode.Unauthorized ||
                response.StatusCode == HttpStatusCode.NotFound ||
                response.StatusCode == HttpStatusCode.BadRequest)
            {
                return;
            }

            // For 500 (Internal Server Error) or others, throw the exception
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Critical API Error: {response.StatusCode} - {content}");
        }

        public static void IgnoreSSL()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

    }
}