using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ProtocolShared.Proto.Base;
using System.Net.Http;
using ProtocolShared.Proto;

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

        public async Task HttpRequest<T, U>(string url, U data, HttpMethod httpMethod, Action<ResponseData<T>> onComplete) where T : class
        {
            ResponseData<T> res = await HttpRequest<T, U>(url, data, httpMethod);

            onComplete?.Invoke(res);
        }

        public async Task<ResponseData<T>> HttpRequest<T, U>(string url, U data, HttpMethod httpMethod) where T : class
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
                        return new ResponseData<T> { ResCode = ResCode.NullResponse };
                    }

                    if (response.IsSuccessStatusCode == false)
                    {
                        return new ResponseData<T> { ResCode = (ResCode)response.StatusCode };
                    }

                    byte[] results = await response.Content.ReadAsByteArrayAsync();
                    var bodyString = Encoding.UTF8.GetString(results);

                    ResponseData<T> bodyObj = JsonConvert.DeserializeObject<ResponseData<T>>(bodyString) ?? new ResponseData<T> { ResCode = ResCode.EmptyBody };

                    if (bodyObj == null)
                    {
                        return new ResponseData<T> { ResCode = ResCode.JsonParseFailed };
                    }

                    if(bodyObj.ResCode == ResCode.DuplicationRequest)
                    {
                        // 중복 요청으로 인한 1초 대기 후 재요청
                        ++tryCount;
                        await Task.Delay(1000);
                        continue;
                    }

                    if(bodyObj.ResCode == ResCode.ExpiredAccessToken)
                    {
                        // accessToken 이 만료되어 refreshToken 으로 갱신 요청
                        ResponseData<RefreshTokenResponse> tokenRes = await HttpRequest<RefreshTokenResponse, RefreshTokenRequest>(_refrashUrl, new RefreshTokenRequest { refreshToken = _refrashToken }, HttpMethod.POST);

                        if(tokenRes.ResCode == ResCode.Successed)
                        {
                            _accessToken = tokenRes.Data.accessToken;
                            ++tryCount;
                            // 갱신 성공하면 이전 요청 다시 요청
                            continue;
                        }
                        else
                        {
                            // 실패하면 에러 코드 리턴 (다시 로그인 해야 됨)
                            return new ResponseData<T> { ResCode = tokenRes.ResCode };
                        }
                    }

                    return bodyObj;
                }
                return new ResponseData<T> { ResCode = ResCode.ExceededRetryCount };
            }
            catch (Exception)
            {
                return new ResponseData<T> { ResCode = ResCode.Exception };
            }
        }
    }
}
