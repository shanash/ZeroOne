using FluffyDuck.Memorial;
using UnityEngine;

public class TestMemorial : MonoBehaviour
{
    void Start()
    {
        Factory.Create<Producer>(10000200, MEMORIAL_TYPE.MAIN_LOBBY, this.transform);
    }
}
