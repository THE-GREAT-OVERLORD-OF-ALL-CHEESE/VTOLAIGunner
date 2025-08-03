using CheeseMods.AIHelicopterGunner.Character;
using VTOLVR.DLC.Rotorcraft;

namespace CheeseMods.AIHelicopterGunner.AIStates.Power;

public class State_RotorUnfold : AITryState
{
    private HelicopterRotor rotor;
    private IVoice voice;

    public override string Name => "Rotor Unfold";

    public override float WarmUp => 1f;

    public override float CoolDown => 3f;

    public State_RotorUnfold(HelicopterRotor rotor, IVoice voice)
    {
        this.rotor = rotor;
        this.voice = voice;
    }

    public override bool CanStart()
    {
        return rotor.foldSwitch == 1;
    }

    public override void StartState()
    {
        rotor.SetFold(0);
        voice.Say("Unfolding rotor");
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
