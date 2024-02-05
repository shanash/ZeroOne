using FluffyDuck.Util;
using ProtocolShared.Proto.Base;
using ProtocolShared.Proto;
using System;
using ProtocolShared.FDHttpClient;

public class HttpCharacter : Singleton<HttpCharacter>
{
    protected override void Initialize()
    {

    }

    public void RequestGetCharacterList(Action<ResponseData<CharacterListResponse>> callback)
    {
        NetworkManager.Instance.SendRequest<EmptyRequest, CharacterListResponse>(HttpMethod.GET, "game/character", null, (ResponseData<CharacterListResponse> res) => { callback(res); });
    }
}
