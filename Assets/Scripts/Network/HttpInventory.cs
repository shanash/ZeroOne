using FluffyDuck.Util;
using ProtocolShared.Proto.Base;
using ProtocolShared.Proto;
using System;
using ProtocolShared.FDHttpClient;

public class HttpInventory : Singleton<HttpInventory>
{
    protected override void Initialize()
    {

    }

    public void RequestGetInventory(Action<ResponseData<InventoryResponse>> callback)
    {
        NetworkManager.Instance.SendRequest<EmptyRequest, InventoryResponse>(HttpMethod.GET, "game/inventory", null, (ResponseData<InventoryResponse> res) => { callback(res); });
    }
}
