using CheeseMods.AIHelicopterGunner.Character;
using System.Collections.Generic;
using System.Linq;
using VTOLVR.DLC.Rotorcraft;

namespace CheeseMods.AIHelicopterGunner.AIStates.Power;

public class State_EngineStart : AITryState
{
    private TurbineStarterMotor turbineStarter;
    private List<TurbineStarterMotor> prerequisiteStarters;

    public override string Name => $"Starting {engineID}";

    public override float WarmUp => 1f;

    public override float CoolDown => 1f;

    private IVoice voice;
    private string engineID;

    public State_EngineStart(TurbineStarterMotor turbineStarter, List<TurbineStarterMotor> prerequisiteStarters, IVoice voice, string engineID)
    {
        this.turbineStarter = turbineStarter;
        this.voice = voice;
        this.engineID = engineID;
        this.prerequisiteStarters = prerequisiteStarters;
    }

    public override bool CanStart()
    {
        return turbineStarter.turbine.outputRPM < turbineStarter.maxRPM * 0.75f && !turbineStarter.motorEnabled
            && (!prerequisiteStarters.Any() || prerequisiteStarters.All(e => e.turbine.outputRPM > turbineStarter.maxRPM * 0.75f && !turbineStarter.motorEnabled));
    }

    public override void StartState()
    {
        turbineStarter.SetMotorEnabled(2);
        voice.Say($"Starting {engineID}");
    }

    public override void UpdateState()
    {
        //Do Nothing
    }

    public override bool IsOver()
    {
        return !turbineStarter.motorEnabled;
    }

    public override void EndState()
    {
        //Do Nothing
    }
}
