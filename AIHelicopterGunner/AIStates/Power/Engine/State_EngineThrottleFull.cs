using CheeseMods.AIHelicopterGunner.Character;
using VTOLVR.DLC.Rotorcraft;

namespace CheeseMods.AIHelicopterGunner.AIStates.Power;

public class State_EngineThrottleFull : AITryState
{
    private HeliPowerGovernor governor;
    private IVoice voice;

    public override string Name => "Set Power To Full";

    public override float WarmUp => 1f;

    public override float CoolDown => 3f;

    public State_EngineThrottleFull(HeliPowerGovernor governor, IVoice voice)
    {
        this.governor = governor;
        this.voice = voice;
    }

    public override bool CanStart()
    {
        return governor.currentThrottleLimit < 1f;
    }

    public override void StartState()
    {
        governor.SetThrottleLimit(1f);
        voice.Say($"Throttle to Full");
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
