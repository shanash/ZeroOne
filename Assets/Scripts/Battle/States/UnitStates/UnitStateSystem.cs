public class UnitStateSystem<N, B, U> : StateSystemBase<UNIT_STATES>
    where N : class
    where B : class
    where U : class
{
    /*
    private N Unit { get => Components[0] as N; }
    private B Battle_Mng { get => Components[1] as B; }
    private U UiMng { get => Components[2] as U; }
    */

    public void Lazy_Init_Setting(N unit, B mng, U ui, UNIT_STATES trans)
    {
        Lazy_Init_Setting(trans, unit, mng, ui);
    }
}
