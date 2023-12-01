
/// <summary>
/// 트리거 이벤트를 받을 클래스(GameObject)에서 본 클래스를 상속 구현해줘야 한다.
/// </summary>
public interface IEventTrigger
{
    void TriggerEventListener(string trigger_id, EventTriggerValue evt_val);
}
