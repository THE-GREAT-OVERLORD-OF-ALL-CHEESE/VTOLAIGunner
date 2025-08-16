using CheeseMods.AIHelicopterGunner.Character;
using CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects;

namespace CheeseMods.AIHelicopterGunner.AIStates.Power;

public class State_DoNothingCauseDead : AITryState
{
    private Battery battery;

    public override string Name => "Doing nothing as the aircraft died :(";

    public override float WarmUp => 9999f;

    public override float CoolDown => 9999f;

    public State_DoNothingCauseDead(Battery battery)
    {
        this.battery = battery;
    }

    public override bool CanStart()
    {
        return !battery.isAlive;
    }

    public override void StartState()
    {
        //Do Nothing
    }

    public override void UpdateState()
    {
        //Do Nothing
    }

    public override bool IsOver()
    {
        return true;
    }

    public override void EndState()
    {
        //Do Nothing
    }
}
