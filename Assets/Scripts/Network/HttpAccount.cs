using FluffyDuck.Util;
using ProtocolShared.FDHttpClient;
using ProtocolShared.Proto;
using ProtocolShared.Proto.Base;
using System;

public class HttpAccount : Singleton<HttpAccount>
{
    protected override void Initialize()
    {

    }

    public void DevLogin(DevLoginRequest request, Action<ResponseData<LoginResponse>> callback)
    {
        NetworkManager.Instance.SendRequest<DevLoginRequest, LoginResponse>(HttpMethod.GET, "account/login/dev", request, (ResponseData<LoginResponse> res) => { callback(res); });
    }
}
