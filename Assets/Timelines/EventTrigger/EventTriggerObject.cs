
using UnityEngine;



/// <summary>
/// EventTrigger를 받을 GameObject에 본 클래스를 컴포넌트로 추가해준다.
/// EventTrigger 리스너인 IEventTrigger를 해당 클래스에서 구현해준다.
/// 타임라인 트랙에 EventTrigger 트랙을 생성하고, 바인딩 오브젝트로 EventTriggerObject 컴포넌트가 포함되어 있는
/// GameObject를 바인딩 해준다.
/// EventTriggerClip을 이용하여 원하는 시점에 이벤트가 발생할 수 있도록 위치를 지정하고, 
/// 필요한 Trigger ID, 필요한 값(int, double, float, string)을 입력하여 사용한다.
/// 이벤트는 클립당 최초 실행시 1회만 발생하도록 되어 있다.
/// EventTriggerClip은 블랜딩 되지 않는다.
/// </summary>
public class EventTriggerObject : MonoBehaviour
{
    /// <summary>
    /// Trigger ID 및 이벤트 파라메터 전달
    /// </summary>
    /// <param name="trigger_id"></param>
    /// <param name="val"></param>
    public void SendEventTrigger(string trigger_id, EventTriggerValue val)
    {
        var evt_trigger = GetComponent<IEventTrigger>();
        evt_trigger?.TriggerEventListener(trigger_id, val);
    }
}
