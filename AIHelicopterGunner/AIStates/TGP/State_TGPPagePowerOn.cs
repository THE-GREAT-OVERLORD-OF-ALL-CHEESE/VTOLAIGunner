using CheeseMods.AIHelicopterGunner.Character;
using System;

namespace CheeseMods.AIHelicopterGunner.AIStates.TGP;

public class State_TGPPagePowerOn : AITryState
{
    private TargetingMFDPage tgpMfd;


    public override string Name => "Switching On TGP";

    public override float WarmUp => 0.5f;
    public override float CoolDown => 0.5f;

    public Callout tgpCallout;

    public State_TGPPagePowerOn(TargetingMFDPage tgpMfd, IVoice voice)
    {
        this.tgpMfd = tgpMfd;
        tgpCallout = new Callout(5f, 10f, 1, () => voice.Say("I have TGP control"));
    }

    public override bool CanStart()
    {
        return !tgpMfd.powered;
    }

    public override void StartState()
    {
        tgpMfd.TGPPwrButton();
        tgpCallout.SayCallout();
    }

    public override void UpdateState()
    {
        throw new NotImplementedException();
    }

    public override bool IsOver()
    {
        return true;
    }

    public override void EndState()
    {

    }
}
