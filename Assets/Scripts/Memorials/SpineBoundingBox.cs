using Cysharp.Text;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;


public class SpineBoundingBox : MonoBehaviour, ICursorInteractable
{
    [SerializeField, Tooltip("Name")]
    string Attach_Name = null;

    [SerializeField, Tooltip("Actress")]
    ActressBase Actress;

    [SerializeField, Tooltip("Touch Body Type")]
    TOUCH_BODY_TYPE Body_Type = TOUCH_BODY_TYPE.NONE;

    /// <summary>
    /// 터치 가능한 부위 폴리곤
    /// </summary>
    PolygonCollider2D Polygon;
    
    /// <summary>
    /// Polygon2D는 스파인 내부적으로는 BoundingBox의 크기가 변경되고 있지만
    /// 실제 Polygon2D의 좌표가 변경되는 것은 아님.
    /// BoundingBox의 좌표에 따라 Polygon2D의 좌표도 업데이트 하기 위한 좌표 정보
    /// </summary>
    List<Vector2> Polygon_Points = new List<Vector2>();

    /// <summary>
    /// Bounding Box 접근 가능한 어태치 노드
    /// </summary>
    BoundingBoxAttachment Bounding_Box_Attach;

    /// <summary>
    /// BoundingBox를 보유하고 있는 어태치 슬롯
    /// </summary>
    Slot Attach_Slot;

    /// <summary>
    /// 해당 포인트가 있다면 넣어줍니다
    /// </summary>
    PointAttachment Point;

    Camera Main_Cam;

    public void SetTouchBodyType(TOUCH_BODY_TYPE btype) { Body_Type = btype; }
    public TOUCH_BODY_TYPE GetTouchBodyType() { return Body_Type; }

    private void Start()
    {
        InitBoundingBox();
        Main_Cam = Camera.main;
        Point = Actress.FindAttachment($"pt_{Attach_Name}", $"pt_{Attach_Name}") as PointAttachment;
    }

    public Vector2 GetPtDirection()
    {
        if (Point == null)
        {
            return Vector2.zero;
        }

        // 화면상의 포인트 - 본 위치 벡터
        Vector2 screen_dir = 
            Main_Cam.WorldToScreenPoint(this.transform.TransformPoint(new Vector3(Point.X, Point.Y, 0))) // Point의 화면기준 위치
            - Main_Cam.WorldToScreenPoint(this.transform.position); // 바운딩박스 == 본 의 화면기준 위치

        return screen_dir;
    }

    public void Set(ActressBase actress, string name)
    {
        Actress = actress;
        Attach_Name = name;
    }

    public void SetSlotAttach(string slot_name, string attach_name)
    {
        Attach_Slot = Actress.FindSlot(slot_name);
        Bounding_Box_Attach = (BoundingBoxAttachment)Actress.FindAttachment(slot_name, attach_name);
    }

    /// <summary>
    /// 폴리곤 노드를 가져오고, 
    /// 어태치 슬롯 및 바운딩 박스 어태치 정보 가져오기
    /// </summary>
    public void InitBoundingBox()
    {
        Polygon = this.gameObject.GetComponent<PolygonCollider2D>();
    }

    private void LateUpdate()
    {
        UpdateBoundingBoxLocalVerices();
    }

    /// <summary>
    /// 바운딩 박스의 크기 및 위치에 따라  Polygon2D 좌표 업데이트
    /// </summary>
    void UpdateBoundingBoxLocalVerices()
    {
        if (Bounding_Box_Attach == null || Attach_Slot == null || Polygon == null)
        {
            return;
        }
        Polygon_Points.Clear();
        var verts = Bounding_Box_Attach.GetLocalVertices(Attach_Slot, null);
        if (verts != null)
        {
            Polygon_Points.AddRange(verts);
            Polygon.SetPath(0, Polygon_Points);
        }
    }

    public override string ToString()
    {
        var sb = ZString.CreateStringBuilder();

        sb.AppendFormat("{0} <color=yellow>[{1}]</color>\n", nameof(Body_Type), Body_Type.ToString());
        sb.AppendFormat("{0} <color=yellow>[{1}]</color>", nameof(Attach_Slot), Attach_Slot.Attachment.Name);

        return sb.ToString();
    }
}
