namespace CheeseMods.AIHelicopterGunner.AIStates.MFDStates;

public class State_MFDSwitchOn : AITryState
{
    private MFD mfd;

    public override string Name => "Switching On MFD";
    public override float WarmUp => 1f;
    public override float CoolDown => 0.5f;

    public State_MFDSwitchOn(MFD mfd)
    {
        this.mfd = mfd;
    }

    public override bool CanStart()
    {
        return !mfd.powerOn;
    }

    public override void StartState()
    {
        mfd.SetPower(1);
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

    }
}
