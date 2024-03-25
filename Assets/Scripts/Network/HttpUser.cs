using FluffyDuck.Util;
using ProtocolShared.Proto.Base;
using ProtocolShared.Proto;
using System;
using System.Net.Mail;
using ProtocolShared.FDHttpClient;

public class HttpPlayer : Singleton<HttpPlayer>
{
    protected override void Initialize()
    {

    }

    public void RequestGetPlayerInfo(Action<ResponseData<PlayerInfoResponse>> callback)
    {
        NetworkManager.Instance.SendRequest<EmptyRequest, PlayerInfoResponse>(HttpMethod.GET, "game/player", null, (ResponseData<PlayerInfoResponse> res) => { callback(res); });
    }

    public void RequestCreatePlayer(CreatePlayerRequest request, Action<ResponseData<PlayerInfoResponse>> callback)
    {
        NetworkManager.Instance.SendRequest<CreatePlayerRequest, PlayerInfoResponse>(HttpMethod.POST, "game/player", request, (ResponseData<PlayerInfoResponse> res) => { callback(res); });
    }

    public void RequestChargeCheck(ChargeCheckRequest request, Action<ResponseData<ChargeCheckResponse>> callback)
    {
        NetworkManager.Instance.SendRequest<ChargeCheckRequest, ChargeCheckResponse>(HttpMethod.POST, "game/player/chargecheck", request, (ResponseData<ChargeCheckResponse> res) => { callback(res); });
    }

    public void RequestUsePlayerExpItem(UsePlayerExpItemRequest request, Action<ResponseData<UsePlayerExpItemResponse>> callback)
    {
        NetworkManager.Instance.SendRequest<UsePlayerExpItemRequest, UsePlayerExpItemResponse>(HttpMethod.POST, "game/player/expitem", request, (ResponseData<UsePlayerExpItemResponse> res) => { callback(res); });
    }
}
