using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ProtocolShared.Proto.Base;

namespace ProtocolShared.FDHttpClient
{
    public enum HttpMethod
    {
        GET,
        POST,
        PUT,
        DELETE,
    }
    internal class FDHttpClient
    {
        //private static string? _accessToken = null;
        //private static string? _refeshToken = null;
        // private HttpClient? _httpClient = null;
        public FDHttpClient()
        {

        }

        //public HttpClient CreateHttpClient(bool connectionClose = true)
        //{
        //    HttpClientHandler handler = new HttpClientHandler()
        //    {
        //        AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
        //    };

        //    var client = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(10) };
        //    client.DefaultRequestHeaders.ConnectionClose = connectionClose;
        //    client.DefaultRequestHeaders.ExpectContinue = true;

        //    return client;
        //}

        public async Task<ResponseData<T>>? HttpRequest<T, U>(string url, string? token, U? data, HttpMethod httpMethod) where T : ResponseBase
        {
            
            try
            {
                HttpClient httpClient = new HttpClient();

                if (token != null)
                {
                    if (false == httpClient.DefaultRequestHeaders.Contains("Authorization"))
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }
                }

                HttpResponseMessage? response = null;
                CancellationTokenSource cancellation = new CancellationTokenSource();
                cancellation.CancelAfter(10000);

                string body = JsonConvert.SerializeObject(data);

                switch (httpMethod)
                {
                    case HttpMethod.GET:
                        response = await httpClient.GetAsync(url, cancellation.Token);
                        break;
                    case HttpMethod.POST:
                        response = await httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"), cancellation.Token);
                        break;
                    case HttpMethod.PUT:
                        response = await httpClient.PutAsync(url, new StringContent(body, Encoding.UTF8, "application/json"), cancellation.Token);
                        break;
                    case HttpMethod.DELETE:
                        response = await httpClient.DeleteAsync(url, cancellation.Token);
                        break;
                    default:
                        break;
                }

                if (response == null)
                {
                    return new ResponseData<T> { resCode = ResCode.NullResponse };
                }

                if (response.IsSuccessStatusCode == false)
                {
                    return new ResponseData<T> { resCode = (ResCode)response.StatusCode };
                }

                byte[] results = await response.Content.ReadAsByteArrayAsync();
                var bodyString = Encoding.UTF8.GetString(results);

                ResponseData<T> bodyObj = JsonConvert.DeserializeObject<ResponseData<T>>(bodyString) ?? new ResponseData<T> { resCode = ResCode.EmptyBody };

                if (bodyObj == null)
                {
                    return new ResponseData<T> { resCode = ResCode.JsonParseFailed };
                }

                return bodyObj;
            }
            catch (Exception)
            {

                return new ResponseData<T> { resCode = ResCode.Exception };
            }
        }
    }
}
