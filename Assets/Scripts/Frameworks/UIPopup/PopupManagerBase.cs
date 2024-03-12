using UnityEngine;
using FluffyDuck.Util;
using System;
using System.Collections.Generic;

namespace FluffyDuck.UI
{
    /// <summary>
    /// 팝업 구성이 변동될 수 있기 때문에 PopupContainer는 abstract로 따로 빼두고
    /// PopupManagerBase를 상속받은 클래스를 사용합니다
    /// </summary>
    public abstract class PopupManagerBase<T> : MonoSingleton<T> where T : MonoSingleton<T>
    {
        List<PopupBase> Popup_List;

        /// <summary>
        /// 모든 팝업이 사라지면 Root에 OnEnter()를 호출해준다.
        /// </summary>
        Action Root_On_Enter;
        /// <summary>
        /// Root 화면에서 1개 이상의 팝업이 추가되어 Root 화면을 가리게 되면 Root의 OnExit()를 호출해준다.
        /// </summary>
        Action Root_On_Exit;

        public abstract PopupContainerBase Container { get; }

        /// <summary>
        /// Scene이 변할때마다 재생성됩니다
        /// </summary>
        protected override bool ResetInstanceOnChangeScene => true;
        protected override bool Is_DontDestroyOnLoad => false;

        protected override void Initialize()
        {
            Popup_List = new List<PopupBase>();
            Root_On_Exit = null;
            Root_On_Enter = null;
        }

        /// <summary>
        /// 팝업을 추가하기 전 이전에 있던 팝업들을 Exit() 해준다.
        /// 만약 추가되어 있던 팝업이 하나도 없을 경우 Root.OnExit() 해준다.
        /// </summary>
        /// <param name="popup"></param>
        void AddOnEnter(PopupBase popup)
        {
            if (popup.GetPopupType() != POPUP_TYPE.NOTI_TYPE)
            {
                if (Popup_List.Count == 0)
                {
                    Root_On_Exit?.Invoke();
                }
                else
                {
                    Popup_List[Popup_List.Count - 1].OnExit();
                }
            }
            popup.OnEnter();
            Popup_List.Add(popup);
        }

        /// <summary>
        /// 가장 마지막의 팝업을 제거한 후 현재 보여지는 최상단의 팝업에 OnEnter()를 호출해준다.
        /// </summary>
        void LastPopupOnEnter()
        {
            if (Popup_List.Count > 0)
            {
                Popup_List[Popup_List.Count - 1].OnEnter();
            }
        }


        /// <summary>
        /// 팝업 오브젝트를 Pool에서 가져온다.
        /// is_use_asset을 사용할 경우 Addressable Asset 사용으로 다른 방식으로 GameObject를 가져와야 함
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <param name="is_use_asset"></param>
        /// <returns></returns>
        PopupBase AddPopup(string path, Transform parent)
        {
            var pool = GameObjectPoolManager.Instance;
            PopupBase popup = null;

            GameObject obj = pool.GetGameObject(path, parent);
            if (obj == null)
            {
                Debug.Assert(false);
                return null;
            }
            popup = obj.GetComponent<PopupBase>();
            //obj.GetComponent<RectTransform>().SetAsLastSibling();

            return popup;
        }

        void AddPopup(string path, Transform parent, System.Action<PopupBase> cb)
        {
            var pool = GameObjectPoolManager.Instance;
            pool.GetGameObject(path, parent, (obj) =>
            {
                if (obj == null)
                {
                    cb?.Invoke(null);
                    return;
                }
                PopupBase popup = obj.GetComponent<PopupBase>();
                //obj.GetComponent <RectTransform>().SetAsLastSibling();
                cb?.Invoke(popup);
            });
        }

        /// <summary>
        /// [부모를 다시 붙이는 문제때문에 타입을 명시하는 메소드로 변경]
        /// 비동기 팝업 추가
        /// 팝업의 타입에 따라 각기 다른 컨테이너에 담는다.
        /// 팝업을 생성한후 콜백에 전달
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cb"></param>
        [Obsolete]
        public void Add(string path, System.Action<PopupBase> cb)
        {
            Add(path, POPUP_TYPE.FULLPAGE_TYPE, cb);
        }

        /// <summary>
        /// 비동기 팝업 추가
        /// 팝업의 타입에 따라 각기 다른 컨테이너에 담는다.
        /// 팝업을 생성한후 콜백에 전달
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="cb"></param>
        public void Add(string path, POPUP_TYPE type, System.Action<PopupBase> cb)
        {
            RectTransform container = Container.Get[POPUP_TYPE.FULLPAGE_TYPE];
            if (Container.Get.ContainsKey(type))
            {
                container = Container.Get[type];
            }

            AddPopup(path, container, (popup) =>
            {
                if (popup != null)
                {
                    RectTransform rt_type = Container.Get[POPUP_TYPE.FULLPAGE_TYPE];
                    if (Container.Get.ContainsKey(popup.GetPopupType()))
                    {
                        rt_type = Container.Get[popup.GetPopupType()];
                    }

                    if (rt_type != container)
                    {
                        Debug.LogWarning($"입력한 타입과 맞지 않습니다. 입력 타입 : {type}, 프리팹 타입 : {popup.GetPopupType()}");
                        popup.transform.SetParent(rt_type);
                    }

                    //  최근 추가된 팝업이 가장 위에 오도록
                    popup.gameObject.GetComponent<RectTransform>().SetAsLastSibling();

                    //  기존에 있던 마지막 팝업의 키 이벤트를 받을 수 없도록 설정 (키 이벤트를 받을 필요가 있는건 마지막 팝업에서만 적용되도록)
                    //  키 이벤트가 기존의 팝업에도 적용될 경우 '취소' 키 터치시 적용된 모든 팝업이 닫힐 수 있기 때문
                    int cnt = Popup_List.Count;
                    if (cnt > 0)
                    {
                        Popup_List[cnt - 1].SetKeyEventEnable(false);
                    }
                    //  신규 팝업 추가
                    AddOnEnter(popup);
                }
                cb?.Invoke(popup);
            });
        }

        /// <summary>
        /// [본 프로젝트에서는 사용되지 않을 예정]
        /// 팝업 추가
        /// 팝업의 타입에 따라 각기 다른 컨테이너에 담기도록 구분
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public PopupBase AddFromResources(string path)
        {
            PopupBase popup = null;

            RectTransform container = Container.Get[POPUP_TYPE.FULLPAGE_TYPE];

            popup = AddPopup(path, container);

            //  팝업 오브젝트가 담길 컨테이너. 각 팝업 타입에 따라 각기 다른 컨테이너에 담긴다.
            if (Container.Get.ContainsKey(popup.GetPopupType()))
            {
                container = Container.Get[popup.GetPopupType()];
            }

            if (popup != null)
            {
                popup.transform.SetParent(container);
                //  최근 추가된 팝업이 가장 위에 오도록
                popup.gameObject.GetComponent<RectTransform>().SetAsLastSibling();

                //  기존에 있던 마지막 팝업의 키 이벤트를 받을 수 없도록 설정 (키 이벤트를 받을 필요가 있는건 마지막 팝업에서만 적용되도록)
                //  키 이벤트가 기존의 팝업에도 적용될 경우 '취소' 키 터치시 적용된 모든 팝업이 닫힐 수 있기 때문
                int cnt = Popup_List.Count;
                if (cnt > 0)
                {
                    Popup_List[cnt - 1].SetKeyEventEnable(false);
                }
                //  신규 팝업 추가
                AddOnEnter(popup);
            }

            return popup;
        }

        void LastKeyEventEnableCheck()
        {
            int cnt = Popup_List.Count;
            if (cnt > 0)
            {
                if (!Popup_List.Exists(x => x.IsKeyEventEnable()))
                {
                    Popup_List[cnt - 1].SetKeyEventEnable(true);
                }
                LastPopupOnEnter();
            }
            else
            {
                //  root onenter event need
                Root_On_Enter?.Invoke();
            }
        }
        /// <summary>
        /// 1개라도 오픈된 팝업이 있다면 true
        /// </summary>
        /// <returns></returns>
        public bool IsShow()
        {
            return Popup_List.Exists(x => x.IsShow());
        }

        /// <summary>
        /// 타입별로 오픈된 팝업이 있는지 여부 체크
        /// </summary>
        /// <param name="ptype"></param>
        /// <returns></returns>
        public bool IsShow(POPUP_TYPE ptype)
        {
            return Popup_List.Exists(x => x.IsShow() && x.GetPopupType() == ptype);
        }
        /// <summary>
        /// 가장 최근에 추가된 팝업 삭제
        /// </summary>
        public void RemoveLastPopup()
        {
            if (Popup_List.Count > 0)
            {
                var pop = Popup_List[Popup_List.Count - 1];
                pop.HidePopup();
            }
        }

        /// <summary>
        /// 가장 최근에 추가된 해당 타입의 팝업 삭제
        /// </summary>
        /// <param name="ptype"></param>
        public void RemoveLastPopupType(POPUP_TYPE ptype)
        {
            var popup = Popup_List.FindLast(x => x.GetPopupType() == ptype);
            if (popup != null)
            {
                popup.HidePopup();
            }
        }
        /// <summary>
        /// 요청 팝업 닫기
        /// </summary>
        /// <param name="popup"></param>
        public void RemovePopup(PopupBase popup)
        {
            if (Popup_List.Contains(popup))
            {
                popup.OnExit();
                GameObjectPoolManager.Instance.UnusedGameObject(popup.gameObject, false);
                Popup_List.Remove(popup);
                LastKeyEventEnableCheck();
            }
        }


        /// <summary>
        /// 요청 팝업 타입의 모든 팝업 닫기
        /// </summary>
        /// <param name="ptype"></param>
        public void CloseAllPopupType(POPUP_TYPE ptype)
        {
            var pool = GameObjectPoolManager.Instance;
            var temp_list = Popup_List.FindAll(x => x.GetPopupType() == ptype);
            int cnt = temp_list.Count;
            for (int i = 0; i < cnt; i++)
            {
                var p = temp_list[i];
                p.OnExit();
                pool.UnusedGameObject(p.gameObject, false);
            }
            Popup_List.RemoveAll(temp_list.Contains);
        }

        /// <summary>
        /// 모든 팝업/UI 닫기
        /// </summary>
        public void CloseAll()
        {
            CloseAllPopupType(POPUP_TYPE.MODAL_TYPE);
            CloseAllPopupType(POPUP_TYPE.DIALOG_TYPE);
            CloseAllPopupType(POPUP_TYPE.FULLPAGE_TYPE);
            CloseAllPopupType(POPUP_TYPE.NOTI_TYPE);

            LastKeyEventEnableCheck();
        }

        /// <summary>
        /// 모든 팝업 닫기(UI 제외)
        /// </summary>
        public void CloseAllPopups()
        {
            CloseAllPopupType(POPUP_TYPE.MODAL_TYPE);
            CloseAllPopupType(POPUP_TYPE.DIALOG_TYPE);
            LastKeyEventEnableCheck();
        }

        public void SetRootOnEnter(System.Action enter)
        {
            Root_On_Enter = enter;
        }
        public void SetRootOnExit(System.Action exit)
        {
            Root_On_Exit = exit;
        }
    }
}
