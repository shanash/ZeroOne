using System;

/// <summary>
/// 팩토리로 생성하기 위해서는 이 추상클래스를 받아야 합니다
/// 이 추상클래스로 구현한 클래스에서는 생성자를 private로 고정해주시길
/// </summary>
public abstract class FactoryProductBase
{
    protected bool Initialize_On_Base(params object[] args)
    {
        if (!Factory.Is_Factory_Creating)
        {
            return false;
        }

        Initialize(args);

        return true;
    }

    protected abstract bool Initialize(params object[] args);
}
