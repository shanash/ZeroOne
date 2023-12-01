using UnityEngine;
using UnityEngine.UI;

namespace FluffyDuck.UI
{
    public class PopupButton : MonoBehaviour
    {
        [SerializeField, Tooltip("팝업 버튼 컴포넌트")]
        protected Button Btn;

        [SerializeField, Tooltip("버튼 이미지")]
        protected Image Btn_Image;

        [SerializeField, Tooltip("버튼 텍스트")]
        protected Text Btn_Text;

        protected PopupBase Popup;

        Popup_Button_Data Btn_Data;

        public void SetPopupBase(PopupBase p, Popup_Button_Data data)
        {
            this.Popup = p;
            Btn_Data = data;
        }

        public void AddButtonEvent(System.Action<PopupBase, object> action)
        {
            if (action == null)
            {
                return;
            }
            Btn.onClick.AddListener(() => action?.Invoke(Popup, Btn_Data.Action_Value));
        }

        public void SetButtonText(string txt)
        {
            Btn_Text.text = txt;
        }
    }

}
