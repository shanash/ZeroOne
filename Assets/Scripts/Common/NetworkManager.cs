using System;
using ProtocolShared.FDHttpClient;
using ProtocolShared.Proto;
using ProtocolShared.Proto.Base;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using FluffyDuck.Util;
using System.Net.NetworkInformation;

public class NetworkManager : Singleton<NetworkManager>
{
    const int PING_MILLISECONDS = 200;
    const string PROTOCOL = "http";
    const string HOST = "dev-01.fluffyduck.co.kr";
    const string PORT = default;
    const string NAME_SPACE = "dev";

    Uri Base_Uri;
    FDHttpClient Client;

    // 요청 함수를 일단 집어넣고 차례대로 처리하기 위한 큐
    ConcurrentQueue<Func<Task>> Request_Queue;

    // 요청 함수를 한번에 하나씩만 실행하기 위한 객체
    SemaphoreSlim Semaphore;

    // 유저 정보를 일단 여기다 저장해놓자
    // TODO: 나중에 정리합시다
    string User_Id;
    string Access_Token;
    string Refresh_Token;

    NetworkManager() { }

    protected override void Initialize()
    {
        Base_Uri = new Uri($"{PROTOCOL}://{HOST}{(string.IsNullOrEmpty(PORT) ? "" : $":{PORT}")}/{NAME_SPACE}/");

        Client = new FDHttpClient();
        Request_Queue = new ConcurrentQueue<Func<Task>>();
        Semaphore = new SemaphoreSlim(1, 1); // 한번에 하나씩만 실행하니까 1 1

        User_Id = string.Empty;
        Access_Token = string.Empty;
        Refresh_Token = string.Empty;

        // 서브 스레드에서 돌리기 때문에 직접적인 콜백호출은 금지
        Task.Run(() => ProcessQueue());
    }

    protected override void OnDispose() { }

    /// <summary>
    /// 개발용 로그인을 요청하기 위한 메소드
    /// </summary>
    /// <param name="mac_address">개발디바이스의 맥 어드레스</param>
    /// <param name="callback">콜백</param>
    public void RequestDevLogin(Action<ResponseData<DevLoginResponse>> callback)
    {
        string mac_address = GetMacAddress();
        if (string.IsNullOrEmpty(mac_address))
        {
            throw new Exception("원인 불명의 이유로 맥 어드레스를 정상적으로 가져올 수 없습니다");
        }

        SendRequest(
            HttpMethod.POST,
            "account/login/dev", 
            new DevLoginRequest { macAddress = mac_address },
            (ResponseData<DevLoginResponse> res) => {
                User_Id = res.Data.userId;
                Access_Token = res.Data.accessToken;
                Refresh_Token = res.Data.refreshToken;

                callback(res);
            });
    }

    /// <summary>
    /// 게임서버에 요청을 위한 메소드
    /// </summary>
    /// <typeparam name="TRequest">요청 데이터</typeparam>
    /// <typeparam name="TResponse">응답 데이터</typeparam>
    /// <param name="Method">메소드 종류</param>
    /// <param name="path">경로</param>
    /// <param name="request">요청 데이터</param>
    /// <param name="callback">콜백</param>
    public void SendRequest<TRequest, TResponse>(HttpMethod Method, string path, TRequest request, Action<ResponseData<TResponse>> callback) where TResponse : class, new()
    {
        if (string.IsNullOrEmpty(Access_Token) && typeof(DevLoginRequest).IsAssignableFrom(typeof(TRequest)) == false)
        {
            throw new Exception("로그인을 먼저 해야 합니다");
        }

        Request_Queue.Enqueue(() => ExecuteRequest(Method, path, request, callback));
    }

    /// <summary>
    /// 실제로 요청을 실행하는 부분
    /// </summary>
    /// <typeparam name="TRequest">요청 데이터</typeparam>
    /// <typeparam name="TResponse">응답 데이터</typeparam>
    /// <param name="Method">메소드 종류</param>
    /// <param name="Method_Path">경로</param>
    /// <param name="request">요청 데이터</param>
    /// <param name="callback">콜백</param>
    /// <returns></returns>
    async Task ExecuteRequest<TRequest, TResponse>(HttpMethod Method, string Method_Path, TRequest request, Action<ResponseData<TResponse>> callback) where TResponse : class, new()
    {
        // 백그라운드 스레드에서 돌기 때문에 직접적으로 콜백호출을 하면 안된다
        await Semaphore.WaitAsync();
        try
        {
            bool retry;
            do
            {
                retry = false;

                ResponseData<TResponse> response = await Client.HttpRequest<TResponse, TRequest>(
                    $"{Base_Uri}{((Method_Path[0].Equals('/')) ? Method_Path[1..] : Method_Path)}",
                    null, request, Method);

                if (response.ResCode != ResCode.Successed
                    && ((int)response.ResCode < 200 || (int)response.ResCode >= 300))
                {
                    MainThreadDispatcher.Instance.Enqueue(() => callback?.Invoke(response));
                    while (!retry)
                    {
                        await Task.Delay(PING_MILLISECONDS);
                    }
                }
                else
                {
                    // 그래서 요렇게 MainThreadDispatcher에 넣어서 유니티 Monobehaviour의 업데이트에서 호출해줍니다
                    MainThreadDispatcher.Instance.Enqueue(() => callback?.Invoke(response));
                }
            } while (retry);
        }
        finally
        {
            Semaphore.Release();
        }
    }

    /// <summary>
    /// 요청 큐를 돌리는 메소드
    /// </summary>
    async void ProcessQueue()
    {
        while (!Disposed)
        {
            // 일단 있으면 빼고
            if (Request_Queue.TryDequeue(out var requestTask))
            {
                // 
                await requestTask();
            }
            else
            {
                await Task.Delay(PING_MILLISECONDS);
            }
        }
    }

    static string GetMacAddress()
    {
        foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
        {
            // 네트워크 인터페이스 상태가 Up(활성 상태)인 경우에만 MAC 주소를 출력
            if (nic.OperationalStatus == OperationalStatus.Up)
            {
                return nic.GetPhysicalAddress().ToString();
            }
        }

        return string.Empty;
    }
}
