using FluffyDuck.Util;
using ProtocolShared.Proto.Base;
using ProtocolShared.Proto;
using System;
using ProtocolShared.FDHttpClient;
using static PixelCrushers.DialogueSystem.QuestLogWindow;

public class HttpGame : Singleton<HttpGame>
{
    protected override void Initialize()
    {
    }


    public void RequestGameStart(GameStartRequest request, Action<ResponseData<GameStartResponse>> callback)
    {
        NetworkManager.Instance.SendRequest<GameStartRequest, GameStartResponse>(HttpMethod.POST, "game/start", null, (ResponseData<GameStartResponse> res) => { callback(res); });
    }

    public void RequestGameEnd(GameEndRequest request, Action<ResponseData<GameEndResponse>> callback)
    {
        NetworkManager.Instance.SendRequest<GameEndRequest, GameEndResponse>(HttpMethod.POST, "game/end", null, (ResponseData<GameEndResponse> res) => { callback(res); });
    }
}
