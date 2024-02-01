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

    public void RequestGetUserInfo(Action<ResponseData<UserInfoResponse>> callback)
    {
        NetworkManager.Instance.SendRequest<EmptyRequest, UserInfoResponse>(HttpMethod.GET, "game/user", null, (ResponseData<UserInfoResponse> res) => { callback(res); });
    }

    public void RequestCreateUser(CreateUserRequest request, Action<ResponseData<UserInfoResponse>> callback)
    {
        NetworkManager.Instance.SendRequest<CreateUserRequest, UserInfoResponse>(HttpMethod.POST, "game/user", request, (ResponseData<UserInfoResponse> res) => { callback(res); });
    }
}
