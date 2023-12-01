using UnityEngine;
using UnityEngine.Rendering;


namespace FluffyDuck.Util
{
    public enum ZORDER_INDEX
    {
        BACKGROUND = 0,                 //  가장 뒷 배경쪽에 그려진다
        HERO_BACK_EFFECT = 49,          //  지정 영웅보다 바로 뒤에서 그려진다
        HERO = 50,                      //  영웅의 기본 인덱스
        HERO_FRONT_EFFECT = 51,         //  지정 영웅보다 바로 앞에서 그려진다(영웅을 가림)
        ALL_ROUND = 70                  //  모든 영웅을 가린다
    }
    public class RendererSortingZ : MonoBehaviour
    {
        SortingGroup Sort_Group;

        [SerializeField, Tooltip("Z Order Enum")]
        ZORDER_INDEX ZOrder;

        int Default_Order;

        private void Start()
        {
            Sort_Group = GetComponent<SortingGroup>();
            SetZorderIndex(ZOrder);
        }

        public void SetZorderIndex(ZORDER_INDEX zindex)
        {
            ZOrder = zindex;
            Default_Order = (int)zindex;
        }
        // Update is called once per frame
        void Update()
        {
            //  최소 값은 0, 0보다 작으면 안됨
            //  가까울수록 큰 수, 멀수록 작은 수
            Vector3 pos = this.transform.position;

            int order = this.Default_Order - (int)pos.z;
            if (order < 0)
            {
                order = 0;
            }
            Sort_Group.sortingOrder = order;
        }
    }

}
