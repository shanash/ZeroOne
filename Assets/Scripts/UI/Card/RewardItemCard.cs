using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardItemCard : ItemCardBase
{
    [SerializeField, Tooltip("Recv Complete Check Box")]
    protected RectTransform Recv_Complete;

    public void SetRecvComplete(bool complete)
    {
        Recv_Complete.gameObject.SetActive(complete);
    }


    public override void Spawned()
    {
        base.Spawned();
        Recv_Complete.gameObject.SetActive(false);
    }

}
