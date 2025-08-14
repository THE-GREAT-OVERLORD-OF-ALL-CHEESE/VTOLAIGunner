using AIHelicopterGunner.Character;
using CheeseMods.AIHelicopterGunner.Character;
using CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects;

namespace CheeseMods.AIHelicopterGunner.AIStates.Power;

public class State_APUStart : AITryState
{
    private AuxilliaryPowerUnit apu;
    private IVoice voice;

    private Callout apuStartCallout;

    public override string Name => "Switching on APU";

    public override float WarmUp => 1f;

    public override float CoolDown => 3f;

    public State_APUStart(AuxilliaryPowerUnit apu, IVoice voice)
    {
        this.apu = apu;
        this.voice = voice;

        apuStartCallout = new Callout(1f, 15f, 1, () => voice.SaySystemStarting(LineType.APU));
    }

    public override bool CanStart()
    {
        return !apu.powerEnabled && !apu.destroyed;
    }

    public override void StartState()
    {
        apu.PowerUp();
        apuStartCallout.SayCallout();
    }

    public override void UpdateState()
    {

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
