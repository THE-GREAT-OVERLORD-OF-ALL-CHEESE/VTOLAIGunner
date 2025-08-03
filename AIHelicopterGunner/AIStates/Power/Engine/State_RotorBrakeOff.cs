using CheeseMods.AIHelicopterGunner.Character;
using VTOLVR.DLC.Rotorcraft;

namespace CheeseMods.AIHelicopterGunner.AIStates.Power;

public class State_RotorBrakeOff : AITryState
{
    private RotorBrake rotorBrake;
    private IVoice voice;

    public override string Name => "Rotor Brake Off";

    public override float WarmUp => 1f;

    public override float CoolDown => 3f;

    public State_RotorBrakeOff(RotorBrake rotorBrake, IVoice voice)
    {
        this.rotorBrake = rotorBrake;
        this.voice = voice;
    }

    public override bool CanStart()
    {
        return rotorBrake.brake;
    }

    public override void StartState()
    {
        rotorBrake.SetBrake(0);
        voice.Say("Rotor brake off");
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
