using TMPro;
using UnityEngine;

public class RewardItemCard : RewardCardBase
{
    [SerializeField, Tooltip("Recv Complete Check Box")]
    protected RectTransform Recv_Complete;

    [SerializeField, Tooltip("Count Box")]
    protected RectTransform Count_Box;
    [SerializeField, Tooltip("Count Text")]
    protected TMP_Text Count_Text;

    Vector2 Init_Scale = new Vector2(0.66f, 0.66f);


    public void SetRecvComplete(bool complete)
    {
        Recv_Complete.gameObject.SetActive(complete);
    }

    public void SetCount(int cnt)
    {
        Count_Box.gameObject.SetActive(cnt > 0);
        Count_Text.text = cnt.ToString("N0");
        
    }

    public override void Spawned()
    {
        base.Spawned();
        Recv_Complete.gameObject.SetActive(false);

        Count_Box.gameObject.SetActive(false);

        this.transform.localScale = Init_Scale;
    }

}
