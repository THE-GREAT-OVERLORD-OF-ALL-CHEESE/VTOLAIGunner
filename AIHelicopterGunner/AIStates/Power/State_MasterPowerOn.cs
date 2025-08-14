using AIHelicopterGunner.Character;
using CheeseMods.AIHelicopterGunner.Character;

namespace CheeseMods.AIHelicopterGunner.AIStates.Power;

public class State_MasterPowerOn : AITryState
{
    private Battery battery;

    public override string Name => "Switching Master Power On";
    public override float WarmUp => 1f;
    public override float CoolDown => 1f;

    public Callout masterPowerCallout;

    public State_MasterPowerOn(Battery battery, IVoice voice)
    {
        this.battery = battery;
        masterPowerCallout = new Callout(5f, 10f, 1, () => voice.Say("Master power on"));
    }

    public override bool CanStart()
    {
        return !battery.connected;
    }

    public override void StartState()
    {
        battery.ToggleConnection();
        masterPowerCallout.SayCallout();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
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
