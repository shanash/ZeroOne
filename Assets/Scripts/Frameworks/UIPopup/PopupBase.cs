using FluffyDuck.Util;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyDuck.UI
{
    public enum POPUP_TYPE
    {
        NONE = 0,
        FULLPAGE_TYPE,
        DIALOG_TYPE,
        MODAL_TYPE,
        NOTI_TYPE,
    }

    public class Popup_Button_Data
    {
        public string Prefab_Path;
        public string Btn_Text;
        public object Action_Value;
        public System.Action<PopupBase, object> Action;
    }

    public abstract class PopupBase : MonoBehaviour, IPoolableComponent
    {
        [SerializeField, Tooltip("팝업 박스. 해당 박스를 On/Off 하여 팝업을 보여주거나 감춘다.")]
        protected RectTransform Box_Rect = null;

        [SerializeField, Tooltip("팝업 타입. UI_TYPE, POPUP_TYPE, MODAL_TYPE에 따라 각각 다른 컨테이너에 구현된다.")]
        protected POPUP_TYPE Popup_Type = POPUP_TYPE.NONE;

        [SerializeField, Tooltip("팝업 애니메이션 컴포넌트")]
        protected UIEaseBase Ease_Base = null;

        /// <summary>
        /// 닫기 종료 후 호출되는 함수 - 팝업 내에서 사용됨.
        /// </summary>
        protected System.Action Popup_Hide_End_Callback = null;

        /// <summary>
        /// 닫기 완료 후 호출되는 함수 - 팝업을 호출한 클래스에서 사용됨
        /// </summary>
        protected System.Action Popup_Hide_Complete_Callback = null;

        /// <summary>
        /// 닫기 완료 후 경우에 따라 필요한 데이터를 받아야 할 경우가 있음
        /// 그럴 때 사용하기 위한 딜리게이트
        /// </summary>
        /// <param name="data"></param>
        public delegate void Popup_Closeed_Delegate(params object[] data);
        protected Popup_Closeed_Delegate Closed_Delegate = null;

        protected bool Is_Enable_Key_Event = false;
        protected bool Is_Enable_Esc_Key_Exit = false;

        

        protected virtual bool Initialize(object[] data)
        {
            return true;
        }

        public void SetEnableEscKeyExit(bool enable)
        {
            Is_Enable_Esc_Key_Exit = enable;
        }

        /// <summary>
        /// 등장/숨김 시 애니메이션을 사용하는지 여부 체크
        /// </summary>
        /// <returns></returns>
        protected bool IsUseAnimation()
        {
            return Ease_Base != null;
        }
        
        public virtual void HidePopup(System.Action cb = null)
        {
            Is_Enable_Key_Event = false;
            Popup_Hide_End_Callback = cb;
            if (Ease_Base != null)
            {
                Ease_Base?.StartMove(UIEaseBase.MOVE_TYPE.MOVE_OUT, HidePopupAniEndCallback);
            }
            else
            {
                Hide();
            }
        }

        protected virtual void Hide()
        {
            Closed_Delegate = null;

            Box_Rect?.gameObject.SetActive(false);
            PopupManager.Instance.RemovePopup(this);
            //  팝업 감추기 종료시 호출 - 팝업 내에서 사용
            Popup_Hide_End_Callback?.Invoke();

            //  팝업 감추기 완료 시 호출 - 팝업을 호출한 클래스에
            Popup_Hide_Complete_Callback?.Invoke();
        }

        /// <summary>
        /// 팝업 등장
        /// </summary>
        /// <param name="data"></param>
        public void ShowPopup(params object[] data)
        {
            if (!Initialize(data))
            {
                Debug.Assert(false, $"잘못된 {this.GetType().Name} 팝업 호출!!");
                Hide();
                return;
            }
            Box_Rect?.gameObject.SetActive(true);
            Ease_Base?.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN, ShowPopupAniEndCallback);
        }

        /// <summary>
        /// 애니메이션이 있을 경우, 팝업 등장 완료 후 호출되는 함수
        /// </summary>
        protected virtual void ShowPopupAniEndCallback() { }

        /// <summary>
        /// 애니메이션이 있는 경우, 팝업 숨김 완료 후 호출되는 함수
        /// </summary>
        protected virtual void HidePopupAniEndCallback()
        {
            Hide();
        }

        /// <summary>
        /// 팝업 초기 실행시 이 팝업에서 사용할 프리팹이 이미 로드되어 있는지 여부 체크
        /// 각 팝업마다 필요로하는 프리팹의 요소가 다르기 때문에
        /// 각 팝업마다 개별적인 구현이 필요하다.
        /// </summary>
        protected virtual void CheckPopupUsePrefabs() { }

        /// <summary>
        /// 실제 프리팹 요소를 체크하고, 콜백에 넘겨주기 위한 부분
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="complete_callback"></param>
        protected void CheckPreloadPrefabs(List<string> paths, System.Action complete_callback)
        {
            var pool = GameObjectPoolManager.Instance;
            var temp_list = pool.NotExistPoolByPrefabList(paths);
            if (temp_list != null && temp_list.Count > 0)
            {
                pool.PreloadGameObjectPrefabsAsync(temp_list, (loaded, total) =>
                {
                    if (loaded == total)
                    {
                        complete_callback?.Invoke();
                    }
                });
            }
            else
            {
                complete_callback?.Invoke();
            }
        }

        public void AddClosedCallbackDelegate(Popup_Closeed_Delegate d)
        {
            Closed_Delegate += d;
        }
        
        /// <summary>
        /// 팝업 등장 후 최초 1회만 실행하기 위한 함수.
        /// 경우에 따라 필요한 조건에 만족하면 한번 더 호출하여 사용할 수도 있겠지만
        /// 가급적 최초 1회만 호출하는 조건을 위반하지는 말자.
        /// </summary>
        protected virtual void FixedUpdatePopup() { }

        /// <summary>
        /// 팝업 내에서 필요시 자주 업데이트가 필요한 요소들을 업데이트 해주기 위한 함수
        /// 유저의 액션이나 어떤 조건에 의해 변경되는 부분들은 이 함수를 구현해서 사용하자
        /// </summary>
        public virtual void UpdatePopup()
        {
            if (!IsShow())
            {
                return;
            }
        }

        public virtual bool IsShow()
        {
            if (Box_Rect != null)
            {
                return Box_Rect.gameObject.activeSelf;
            }
            return false;
        }

        public void OnClickClosePopup()
        {
            HidePopup();

            //  click sound [todo]
        }

        public POPUP_TYPE GetPopupType()
        {
            return Popup_Type;
        }

        /// <summary>
        /// 팝업에서 키 이벤트를 사용할 수 있는지 여부를 설정
        /// </summary>
        /// <param name="is_enable"></param>
        public void SetKeyEventEnable(bool is_enable)
        {
            Is_Enable_Key_Event = is_enable;
        }

        ///// <summary>
        ///// 팝업 감추기 종료 시 호출(팝업 내에서만 사용됨)
        ///// </summary>
        ///// <param name="cb"></param>
        //void SetHideEndCallback(System.Action cb)
        //{
        //    Popup_Hide_End_Callback = cb;
        //}

        public void SetHideCompleteCallback(System.Action cb)
        {
            Popup_Hide_Complete_Callback = cb;
        }

        /// <summary>
        /// 키 이벤트가 사용 가능한지 여부 반환
        /// </summary>
        /// <returns></returns>
        public bool IsKeyEventEnable()
        {
            return Is_Enable_Key_Event;
        }

        public virtual void OnEnter()
        {

        }

        public virtual void OnExit()
        {

        }

        public virtual void Spawned()
        {
            Closed_Delegate = null;
            Is_Enable_Esc_Key_Exit = true;
            Is_Enable_Key_Event = true;
            Popup_Hide_End_Callback = null;
            Popup_Hide_Complete_Callback = null;

            // 스케일은 등장시 최소에서 커지는 애니메이션이니
            // 등장시 스케일을 0으로 잡아줍시다
            if (Ease_Base != null && Ease_Base is UIEaseScale)
            {
                Ease_Base.transform.localScale = new Vector2(0f, 0f);
            }
            else if (Ease_Base != null && Ease_Base is UIEaseSlide)
            {
                Ease_Base.transform.localPosition = new Vector2(3000f, 0f);
            }
        }

        public virtual void Despawned()
        {
            Closed_Delegate = null;
            Is_Enable_Key_Event = false;
            Is_Enable_Esc_Key_Exit = false;
        }

        protected virtual void OnUpdatePopup() { }

        protected void Update()
        {
            if (Is_Enable_Esc_Key_Exit)
            {
                if (Is_Enable_Key_Event)
                {
                    if (Input.GetKeyUp(KeyCode.Escape))
                    {
                        HidePopup();
                    }
                }
            }
            OnUpdatePopup();
        }

        public virtual void AddModalButtonEvent(params Popup_Button_Data[] btns) { }
    }
}
