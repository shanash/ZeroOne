using FluffyDuck.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupContainer : PopupContainerBase
{
    public enum Canvas_Name
    {
        FULLPAGE,
        ETCS,
    }

    [SerializeField, Tooltip("UI용 Page 팝업 캔버스")]
    Canvas _FullPage_Canvas;
    [SerializeField, Tooltip("기타등등 팝업 캔버스")]
    Canvas _Etcs_Canvas;

    [SerializeField, Tooltip("UI용 Page 팝업 컨테이너. 팝업 중 가장 하위에 있는 컨테이너")]
    RectTransform _FullPage_Container;
    [SerializeField, Tooltip("일반적인 팝업 컨테이너. UI 보다 상위에 있다.")]
    RectTransform _Dialog_Container;
    [SerializeField, Tooltip("모달 팝업. 최상위 팝업이다. 모달 팝업은 주로 메세지 등 강제적으로 안내해야 하는 경우에 사용")]
    RectTransform _Modal_Container;
    [SerializeField, Tooltip("노티 팝업. 모달 위에서도 보일 수 있는 팝업. 가장 최상위. 현재 UI의 기믹에는 영향을 주지 않고, 화면 일부에서 단순 메세지를 보여주기 위한 용도.")]
    RectTransform _Noti_Container;

    [SerializeField, Tooltip("UI용 Page Canvas")]
    Canvas _Full_Page_Canvas;
    [SerializeField, Tooltip("팝업용 Page Canvas")]
    Canvas _Etc_Page_Canvas;

    public RectTransform FullPage_Container => _FullPage_Container;
    public RectTransform Dialog_Container => _Dialog_Container;
    public RectTransform Modal_Container => _Modal_Container;
    public RectTransform Noti_Container => _Noti_Container;

    protected override void InitializeContainer(ref Dictionary<POPUP_TYPE, RectTransform> container)
    {
        container = new Dictionary<POPUP_TYPE, RectTransform>()
        {
            [POPUP_TYPE.FULLPAGE_TYPE] = _FullPage_Container,
            [POPUP_TYPE.DIALOG_TYPE] = _Dialog_Container,
            [POPUP_TYPE.MODAL_TYPE] = _Modal_Container,
            [POPUP_TYPE.NOTI_TYPE] = _Noti_Container,
        };
    }

    public override void SetUICamera()
    {
        var ui_cam_obj = GameObject.FindGameObjectWithTag("Battle_UI_Camera");
        if (ui_cam_obj != null)
        {
            var ui_cam = ui_cam_obj.GetComponent<Camera>();
            if (ui_cam != null)
            {
                _Full_Page_Canvas.worldCamera = ui_cam;
                _Etc_Page_Canvas.worldCamera = ui_cam;

                _FullPage_Canvas.sortingLayerName = "Popup";
                _Etc_Page_Canvas.sortingLayerName = "Popup";
            }
        }
    }

    public void SetEtcCanvasScaler(float match)
    {
        _Etcs_Canvas.GetComponent<CanvasScaler>().matchWidthOrHeight = match;
    }

    public override void Despawned()
    {
    }

    public override void Spawned()
    {
        /*
        _FullPage_Canvas.worldCamera = Camera.main;
        _FullPage_Canvas.sortingLayerName = "UI";
        _Etcs_Canvas.worldCamera = Camera.main;
        _Etcs_Canvas.sortingLayerName = "UI";
        */
    }
}
