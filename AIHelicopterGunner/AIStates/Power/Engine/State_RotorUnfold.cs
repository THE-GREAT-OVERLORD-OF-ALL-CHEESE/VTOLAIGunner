using CheeseMods.AIHelicopterGunner.Character;
using VTOLVR.DLC.Rotorcraft;

namespace CheeseMods.AIHelicopterGunner.AIStates.Power;

public class State_RotorUnfold : AITryState
{
    private HelicopterRotor rotor;

    public override string Name => "Rotor Unfold";

    public override float WarmUp => 1f;

    public override float CoolDown => 3f;

    public Callout rotorUnfoldCallout;

    public State_RotorUnfold(HelicopterRotor rotor, IVoice voice)
    {
        this.rotor = rotor;
        rotorUnfoldCallout = new Callout(5f, 10f, 1, () => voice.Say("Unfolding rotor"));
    }

    public override bool CanStart()
    {
        return rotor.foldSwitch == 1;
    }

    public override void StartState()
    {
        rotor.SetFold(0);
        rotorUnfoldCallout.SayCallout();
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
