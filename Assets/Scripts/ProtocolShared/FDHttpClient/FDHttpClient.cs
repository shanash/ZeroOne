using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using ProtocolShared.Proto.Base;
using ProtocolShared.Proto;
using PixelCrushers.DialogueSystem;

#nullable disable

namespace ProtocolShared.FDHttpClient
{
    public enum HttpMethod
    {
        GET,
        POST,
        PUT,
        DELETE,
    }

    public class FDHttpClient
    {
        // private HttpClient? _httpClient = null;
        public FDHttpClient()
        {

        }

        public HttpClient CreateHttpClient(bool connectionClose = true)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
            };

            var client = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(10) };
            client.DefaultRequestHeaders.ConnectionClose = connectionClose;
            client.DefaultRequestHeaders.ExpectContinue = true;

            return client;
        }

        readonly int _retryCount = 3;
        public string _refrashUrl = null;
        public string _accessToken = null;
        public string _refrashToken = null;

        public void SetTokenInfo(string refrashUrl, string accessToken, string refrashToken)
        {
            _refrashUrl = refrashUrl;
            _accessToken = accessToken;
            _refrashToken = refrashToken;
        }

        public async Task HttpRequest<TRequest, TResponse>(string url, TRequest data, HttpMethod httpMethod, Action<ResponseData<TResponse>> onComplete) where TResponse : class
        {
            ResponseData<TResponse> res = await HttpRequest<TRequest, TResponse>(url, data, httpMethod);
            onComplete?.Invoke(res);
        }

        public async Task<ResponseData<TResponse>> HttpRequest<TRequest, TResponse>(string url, TRequest data, HttpMethod httpMethod) where TResponse : class
        {
            try
            {
                int tryCount = 0;
                while (tryCount < _retryCount)
                {
                    //TODO:이후에 필드에 저장해놓고 재활용하는 걸 시간날때...ㅋ
                    HttpClient httpClient = new HttpClient();

                    if (_accessToken != null)
                    {
                        if (false == httpClient.DefaultRequestHeaders.Contains("Authorization"))
                        {
                            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);
                        }
                    }

                    HttpResponseMessage response = null;
                    CancellationTokenSource cancellation = new CancellationTokenSource();
                    cancellation.CancelAfter(10000);

                    string body = data != null ? JsonConvert.SerializeObject(data) : "";

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
                        return new ResponseData<TResponse> { ResType = RESPONSE_TYPE.NULL_RESPONSE };
                    }

                    if (response.IsSuccessStatusCode == false)
                    {
                        return new ResponseData<TResponse> { ResType = (RESPONSE_TYPE)response.StatusCode };
                    }

                    byte[] results = await response.Content.ReadAsByteArrayAsync();
                    var bodyString = Encoding.UTF8.GetString(results);

                    ResponseData<TResponse> bodyObj = JsonConvert.DeserializeObject<ResponseData<TResponse>>(bodyString) ?? new ResponseData<TResponse> { ResType = RESPONSE_TYPE.EMPTY_BODY };

                    if (bodyObj == null)
                    {
                        return new ResponseData<TResponse> { ResType = RESPONSE_TYPE.JSON_PARSE_FAILED };
                    }

                    if (bodyObj.ResType == RESPONSE_TYPE.DUPLICATION_REQUEST)
                    {
                        // 중복 요청으로 인한 1초 대기 후 재요청
                        ++tryCount;
                        await Task.Delay(1000);
                        continue;
                    }

                    if (bodyObj.ResType == RESPONSE_TYPE.EXPIRED_ACCESS_TOKEN)
                    {
                        // accessToken 이 만료되어 refreshToken 으로 갱신 요청
                        ResponseData<RefreshTokenResponse> tokenRes = await HttpRequest<RefreshTokenRequest, RefreshTokenResponse> (_refrashUrl, new RefreshTokenRequest { RefreshToken = _refrashToken }, HttpMethod.POST);

                        if (tokenRes.ResType == RESPONSE_TYPE.SUCCESS)
                        {
                            _accessToken = tokenRes.Data.AccessToken;
                            ++tryCount;
                            // 갱신 성공하면 이전 요청 다시 요청
                            continue;
                        }
                        else
                        {
                            // 실패하면 에러 코드 리턴 (다시 로그인 해야 됨)
                            return new ResponseData<TResponse> { ResType = tokenRes.ResType };
                        }
                    }

                    return bodyObj;
                }
                return new ResponseData<TResponse> { ResType = RESPONSE_TYPE.EXCEEDED_RETRY_COUNT };
            }
            catch (Exception)
            {
                return new ResponseData<TResponse> { ResType = RESPONSE_TYPE.EXCEPTION };
            }
        }
    }
}
