using FluffyDuck.Util;
using ProtocolShared.Proto.Base;
using ProtocolShared.Proto;
using System;
using System.Net.Mail;
using ProtocolShared.FDHttpClient;

public class HttpUser : Singleton<HttpUser>
{
    protected override void Initialize()
    {

    }

    public void RequestGetUserInfo(Action<ResponseData<PlayerInfoResponse>> callback)
    {
        NetworkManager.Instance.SendRequest<EmptyRequest, PlayerInfoResponse>(HttpMethod.GET, "game/user", null, (ResponseData<PlayerInfoResponse> res) => { callback(res); });
    }

    public void RequestCreateUser(CreatePlayerRequest request, Action<ResponseData<PlayerInfoResponse>> callback)
    {
        NetworkManager.Instance.SendRequest<CreatePlayerRequest, PlayerInfoResponse>(HttpMethod.POST, "game/user", request, (ResponseData<PlayerInfoResponse> res) => { callback(res); });
    }
}
