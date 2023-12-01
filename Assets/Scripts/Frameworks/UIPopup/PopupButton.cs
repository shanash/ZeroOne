using UnityEngine;
using UnityEngine.UI;

namespace FluffyDuck.UI
{
    public class PopupButton : MonoBehaviour
    {
        [SerializeField, Tooltip("�˾� ��ư ������Ʈ")]
        protected Button Btn;

        [SerializeField, Tooltip("��ư �̹���")]
        protected Image Btn_Image;

        [SerializeField, Tooltip("��ư �ؽ�Ʈ")]
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
