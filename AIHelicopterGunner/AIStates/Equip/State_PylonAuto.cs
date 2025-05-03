using System;
using VTOLVR.DLC.Rotorcraft;

namespace CheeseMods.AIHelicopterGunner.AIStates.Equip;

internal class State_PylonAuto : AITryState
{
    private ArticulatingHardpoint autopylon;

    public override string Name => "Pylon Elevation Set To Auto";
    public override float WarmUp => 0.5f;
    public override float CoolDown => 0.5f;

    public State_PylonAuto(ArticulatingHardpoint autopylon)
    {
        this.autopylon = autopylon;
    }

    public override bool CanStart()
    {
        return autopylon != null && autopylon.autoMode == false;
    }

    public override void StartState()
    {
        autopylon.autoMode = true;
    }

    public override void UpdateState()
    {
        throw new NotImplementedException();
    }

    public override bool IsOver()
    {
        return true;
    }

    public override void EndState()
    {

    }
}
