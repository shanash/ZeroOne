using UnityEngine;

public class PopupContainer : MonoBehaviour
{
    [SerializeField, Tooltip("UI용 Page 팝업 컨테이너. 팝업 중 가장 하위에 있는 컨테이너")]
    RectTransform _FullPage_Container;
    [SerializeField, Tooltip("일반적인 팝업 컨테이너. UI 보다 상위에 있다.")]
    RectTransform _Dialog_Container;
    [SerializeField, Tooltip("모달 팝업. 최상위 팝업이다. 모달 팝업은 주로 메세지 등 강제적으로 안내해야 하는 경우에 사용")]
    RectTransform _Modal_Container;
    [SerializeField, Tooltip("노티 팝업. 모달 위에서도 보일 수 있는 팝업. 가장 최상위. 현재 UI의 기믹에는 영향을 주지 않고, 화면 일부에서 단순 메세지를 보여주기 위한 용도.")]
    RectTransform _Noti_Container;

    public RectTransform FullPage_Container => _FullPage_Container;
    public RectTransform Dialog_Container => _Dialog_Container;
    public RectTransform Modal_Container => _Modal_Container;
    public RectTransform Noti_Container => _Noti_Container;
}
