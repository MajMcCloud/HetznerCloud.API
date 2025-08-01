using HetznerCloudApi.Object.Exceptions;
using HetznerCloudApi.Object.Universal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HetznerCloudApi
{
    public class Core
    {
        public static long PerPage = 50;

        private const string ApiServer = "https://api.hetzner.cloud/v1";

        public static async Task<string> SendGetRequest(string token, string url)
        {
            HttpResponseMessage httpResponseMessage;
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(new HttpMethod("GET"), $"{ApiServer}{url}"))
                {
                    httpRequestMessage.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                    httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                }
            }

            // Response
            string json = await httpResponseMessage.Content.ReadAsStringAsync();

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.OK:
                    break;

                default:
                    // Get Error
                    JObject result = JObject.Parse(json);
                    Error error = JsonConvert.DeserializeObject<Error>($"{result["error"]}") ?? new Error();

                    //Check error
                    if (error.Message.Contains("with ID") && error.Message.Contains("not found"))
                    {
                        // The error is due to the resource not being found. Let's make it return empty instead of an error.
                        json = "{}";
                    }
                    else if (error.Message.Contains("not found"))
                    {
                        throw new ResourceNotFoundException($"{error.Code} - {error.Message}"); 
                    }
                    else
                    {
                        // If it's a genuine error
                        throw new Exception($"{error.Code} - {error.Message}");
                    }
                    break;
            }

            return json;
        }

        public static async Task<T> SendPostRequest<T>(string token, string url)
            where T : class
        {
            var jsonResponse = await SendPostRequest(token, url);

            return JsonConvert.DeserializeObject<T>(jsonResponse) ?? default(T);
        }

        public static async Task<T> SendPostRequest<T>(string token, string url, object content)
            where T : class
        {
            var raw = JsonConvert.SerializeObject(content);

            var jsonResponse = await SendPostRequest(token, url, raw);

            return JsonConvert.DeserializeObject<T>(jsonResponse) ?? default(T);
        }

        public static async Task<string> SendPostRequest(string token, string url, string content)
        {
            HttpResponseMessage httpResponseMessage;
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(new HttpMethod("POST"), $"{ApiServer}{url}"))
                {
                    httpRequestMessage.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                    httpRequestMessage.Content = new StringContent(content);
                    httpRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                }
            }

            // Response
            string json = await httpResponseMessage.Content.ReadAsStringAsync();

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.Created:
                    break;

                default:
                    // Get Error
                    JObject result = JObject.Parse(json);
                    Error error = JsonConvert.DeserializeObject<Error>($"{result["error"]}") ?? new Error();

                    //Check error
                    if (error.Message.Contains("with ID") && error.Message.Contains("not found"))
                    {
                        // The error is due to the resource not being found. Let's make it return empty instead of an error.
                        json = "{}";
                    }
                    else if (error.Message.Contains("not found"))
                    {
                        throw new ResourceNotFoundException($"{error.Code} - {error.Message}");
                    }
                    else
                    {
                        // If it's a genuine error
                        throw new Exception($"{error.Code} - {error.Message}");
                    }
                    break;
            }

            return json;
        }

        public static async Task<string> SendPostRequest(string token, string url)
        {
            HttpResponseMessage httpResponseMessage;
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(new HttpMethod("POST"), $"{ApiServer}{url}"))
                {
                    httpRequestMessage.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                    httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                }
            }

            // Response
            string json = await httpResponseMessage.Content.ReadAsStringAsync();

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.Created:
                    break;

                default:
                    // Get Error
                    JObject result = JObject.Parse(json);
                    Error error = JsonConvert.DeserializeObject<Error>($"{result["error"]}") ?? new Error();

                    //Check error
                    if (error.Message.Contains("with ID") && error.Message.Contains("not found"))
                    {
                        // The error is due to the resource not being found. Let's make it return empty instead of an error.
                        json = "{}";
                    }
                    else if (error.Message.Contains("not found"))
                    {
                        throw new ResourceNotFoundException($"{error.Code} - {error.Message}");
                    }
                    else
                    {
                        // If it's a genuine error
                        throw new Exception($"{error.Code} - {error.Message}");
                    }
                    break;
            }

            return json;
        }

        public static async Task<T> SendPutRequest<T>(string token, string url, object content)
            where T : class
        {
            var raw = JsonConvert.SerializeObject(content);

            var jsonResponse = await SendPutRequest(token, url, raw);

            return JsonConvert.DeserializeObject<T>(jsonResponse) ?? default(T);
        }

        public static async Task<string> SendPutRequest(string token, string url, string content)
        {
            HttpResponseMessage httpResponseMessage;
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(new HttpMethod("PUT"), $"{ApiServer}{url}"))
                {
                    httpRequestMessage.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                    httpRequestMessage.Content = new StringContent(content);
                    httpRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                }
            }

            // Response
            string json = await httpResponseMessage.Content.ReadAsStringAsync();

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.OK:
                    break;

                default:
                    // Get Error
                    JObject result = JObject.Parse(json);
                    Error error = JsonConvert.DeserializeObject<Error>($"{result["error"]}") ?? new Error();

                    //Check error
                    if (error.Message.Contains("with ID") && error.Message.Contains("not found"))
                    {
                        // The error is due to the resource not being found. Let's make it return empty instead of an error.
                        json = "{}";
                    }
                    else if (error.Message.Contains("not found"))
                    {
                        throw new ResourceNotFoundException($"{error.Code} - {error.Message}");
                    }
                    else
                    {
                        // If it's a genuine error
                        throw new Exception($"{error.Code} - {error.Message}");
                    }
                    break;
            }

            return json;
        }

        public static async Task SendDeleteRequest(string token, string url)
        {
            HttpResponseMessage httpResponseMessage;
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("DELETE"), $"{ApiServer}{url}"))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                    httpResponseMessage = await httpClient.SendAsync(request);
                }
            }

            // Response
            string json = await httpResponseMessage.Content.ReadAsStringAsync();

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.NoContent:
                case HttpStatusCode.OK:
                    break;

                default:
                    JObject result = JObject.Parse(json);
                    Error error = JsonConvert.DeserializeObject<Error>($"{result["error"]}") ?? new Error();
                    throw new Exception($"{error.Code} - {error.Message}");
            }
        }
    }
}
