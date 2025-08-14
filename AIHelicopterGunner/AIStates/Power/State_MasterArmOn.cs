using AIHelicopterGunner.Character;
using CheeseMods.AIHelicopterGunner.Character;

namespace CheeseMods.AIHelicopterGunner.AIStates.Power;

public class State_MasterArmOn : AITryState
{
    private WeaponManager wm;

    public override string Name => "Switching Master Arm On";
    public override float WarmUp => 1f;
    public override float CoolDown => 0.5f;

    public Callout weaponsCallout;

    public State_MasterArmOn(WeaponManager wm, IVoice voice)
    {
        this.wm = wm;
        weaponsCallout = new Callout(5f, 10f, 1, () => voice.Say("I have weapons control"));
    }

    public override bool CanStart()
    {
        return !wm.isMasterArmed;
    }

    public override void StartState()
    {
        wm.ToggleMasterArmed();
        weaponsCallout.SayCallout();
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
