using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorManager : MonoBehaviour
{
    [SerializeField, Tooltip("Line Container")]
    RectTransform Behavior_Line_Container;

    Vector2 Start_Point;
    Vector2 Finish_Point;

    List<BehaviorNode> Icon_Node_List = new List<BehaviorNode>();

    void Start()
    {
        Start_Point = new Vector2(0, Behavior_Line_Container.rect.yMax);
        Finish_Point = new Vector2(0, Behavior_Line_Container.rect.yMin);
    }

    public void AddHeroList(List<HeroBase> list)
    {
        var pool = GameObjectPoolManager.Instance;
        for (int i = 0; i < list.Count; i++)
        {
            var h = list[i];
            var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/Behavior/Behavior_Hero_Icon_Node", Behavior_Line_Container);
            Vector2 pos = Vector2.zero;
            if (h.Team_Type == TEAM_TYPE.LEFT)
            {
                pos.x = -25f;
            }
            else
            {
                pos.x = 25f;
            }
            obj.transform.localPosition = pos;

            var icon = obj.GetComponent<BehaviorNode>();
            icon.SetHeroBase(h);

            Icon_Node_List.Add(icon);
        }
    }

    public void RemoveHeroIcon(HeroBase hero)
    {
        var find_icon = Icon_Node_List.Find(x => object.ReferenceEquals(x, hero));
        if (find_icon != null)
        {
            GameObjectPoolManager.Instance.UnusedGameObject(find_icon.gameObject);
            Icon_Node_List.Remove(find_icon);
        }
    }

    public void ClearHeroIcons()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Icon_Node_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var icon = Icon_Node_List[i];
            pool.UnusedGameObject(icon.gameObject);
        }
        Icon_Node_List.Clear();
    }

    public void UpdateHeroIconsOrder()
    {
        if (Icon_Node_List.Count == 0)
        {
            return;
        }
        //  누적 행동력이 높은 영웅 우선순위
        Icon_Node_List.Sort(delegate (BehaviorNode a, BehaviorNode b)
        {
            if (a.GetHeroAccumRapidityPoint() < b.GetHeroAccumRapidityPoint())
            {
                return 1;
            }
            return -1;
        });
        // 현재 영웅들의 최대 / 최소 행동력 값
        double max_value = Icon_Node_List[0].GetHeroAccumRapidityPoint();
        double min_value = Icon_Node_List[Icon_Node_List.Count - 1].GetHeroAccumRapidityPoint();

        double diff = max_value - min_value;
        float offset_y = 0;
        //  시작 위치와 종료 위치의 거리
        float distance = Vector2.Distance(Start_Point, Finish_Point);

        int cnt = Icon_Node_List.Count;

        for (int i = 0; i < cnt; i++)
        {
            var icon = Icon_Node_List[i];
            icon.transform.SetSiblingIndex((cnt - 1) - i);
            double rapidity = icon.GetHeroAccumRapidityPoint() - min_value;
            double moving = (rapidity / diff) * distance;
            float pos_y = offset_y - (float)moving;
            var hero = icon.GetHeroBase();
            
            //Debug.LogFormat("<color=#ffffff>[{0}] [{1}]</color> rapid [{2}], moving [{3}], pos_y [{4}]", hero.Team_Type, (int)hero.Position_Type, rapidity, moving, pos_y);

            icon.SetMovePositionY(pos_y);
        }
    }

    public BehaviorNode GetFirstBehaviorNode()
    {
        if (Icon_Node_List.Count > 0)
        {
            return Icon_Node_List[0];
        }
        return null;
    }
}
