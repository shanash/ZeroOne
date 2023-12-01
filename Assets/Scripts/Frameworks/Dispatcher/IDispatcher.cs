using System;

namespace FluffyDuck.Util
{
    public interface IDispatcher
    {
        void AddAction(Action fn);
    }
}
