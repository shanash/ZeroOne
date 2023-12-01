
namespace FluffyDuck.Util
{
    /// <summary>
    /// Prefab���� ����� GameObject�� MonoBehavior�� ���� ��� �޾Ƽ� �������ָ� �ȴ�.
    /// �ش� Interface�� ����� ��� ����/������ �ش� Spawned() / Despawned()�� �ڵ� ȣ�����־�
    /// �ʱ�ȭ �� ������ �ؾ��ϴ� �۾��� �߰��� �� �� �ִ�.
    /// </summary>
    public interface IPoolableComponent
    {
        void Spawned();
        void Despawned();
    }

}
