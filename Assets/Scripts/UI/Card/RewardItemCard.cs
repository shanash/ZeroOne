using TMPro;
using UnityEngine;

public class RewardItemCard : RewardCardBase
{
    [SerializeField, Tooltip("Recv Complete Check Box")]
    protected RectTransform Recv_Complete;


    Vector2 Init_Scale = new Vector2(0.66f, 0.66f);


    public void SetRecvComplete(bool complete)
    {
        Recv_Complete.gameObject.SetActive(complete);
    }


    public override void Spawned()
    {
        base.Spawned();
        Recv_Complete.gameObject.SetActive(false);

        this.transform.localScale = Init_Scale;
    }

}
