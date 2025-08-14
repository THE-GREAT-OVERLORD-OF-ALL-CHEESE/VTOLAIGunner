using CheeseMods.AIHelicopterGunner.Character;
using CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects;
using VTOLVR.DLC.Rotorcraft;

namespace CheeseMods.AIHelicopterGunner.AIStates.Power;

public class State_EngineThrottleIdle : AITryState
{
    private HeliPowerGovernor governor;
    private IVoice voice;

    public override string Name => "Set Power To Idle";

    public override float WarmUp => 1f;

    public override float CoolDown => 3f;

    public State_EngineThrottleIdle(HeliPowerGovernor governor, IVoice voice)
    {
        this.governor = governor;
        this.voice = voice;
    }

    public override bool CanStart()
    {
        return governor.currentThrottleLimit < governor.throttleIdleNotch;
    }

    public override void StartState()
    {
        governor.SetThrottleLimit(governor.throttleIdleNotch);

        voice.Say(LineType.ThrottleToIdle);
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
