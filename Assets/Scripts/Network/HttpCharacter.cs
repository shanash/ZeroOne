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

    /// <summary>
    /// 케릭터 리스트 정보 가져오기
    /// </summary>
    public void RequestGetCharacterList(Action<ResponseData<CharacterListResponse>> callback)
    {
        NetworkManager.Instance.SendRequest<EmptyRequest, CharacterListResponse>(HttpMethod.GET, "game/character", null, (ResponseData<CharacterListResponse> res) => { callback(res); });
    }

    /// <summary>
    /// 덱 정보 가져오기
    /// </summary>
    public void RequestGetDeckList(Action<ResponseData<DeckListResponse>> callback)
    {
        NetworkManager.Instance.SendRequest<EmptyRequest, DeckListResponse>(HttpMethod.GET, "game/character/deck", null, (ResponseData<DeckListResponse> res) => { callback(res); });
    }

    /// <summary>
    /// 덱 저장
    /// </summary>
    public void RequestSetDeckList(SetDeckRequest request, Action<ResponseData<ResponseBase>> callback)
    {
        NetworkManager.Instance.SendRequest<SetDeckRequest, ResponseBase>(HttpMethod.POST, "game/character/deck", request, (ResponseData<ResponseBase> res) => { callback(res); });
    }

    /// <summary>
    /// 케릭터 경험치 아이템 사용
    /// </summary>
    public void RequestUseCharacterExpItem(UseCharacterExpItemRequest request, Action<ResponseData<UseCharacterExpItemResponse>> callback)
    {
        NetworkManager.Instance.SendRequest<UseCharacterExpItemRequest, UseCharacterExpItemResponse>(HttpMethod.POST, "game/character/expitem", request, (ResponseData<UseCharacterExpItemResponse> res) => { callback(res); });
    }

    /// <summary>
    /// 케릭터 스킬 레벨업 아이템 사용
    /// </summary>
    public void RequestUseCharacterSkillItem(UseCharacterSkillItemRequest request, Action<ResponseData<UseCharacterSkillItemResponse>> callback)
    {
        NetworkManager.Instance.SendRequest<UseCharacterSkillItemRequest, UseCharacterSkillItemResponse>(HttpMethod.POST, "game/character/skillitem", request, (ResponseData<UseCharacterSkillItemResponse> res) => { callback(res); });
    }

}
