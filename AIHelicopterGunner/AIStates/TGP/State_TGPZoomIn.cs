using CheeseMods.AIHelicopterGunner.Components;
using System;
using UnityEngine;

namespace CheeseMods.AIHelicopterGunner.AIStates.TGP;

public class State_TGPZoomIn : AITryState
{
    private TargetingMFDPage tgpMfd;
    private AIGunner aiGunner;

    public override string Name => "TPG Zoom In";
    public override float WarmUp => 0.2f;
    public override float CoolDown => 0.3f;

    public State_TGPZoomIn(TargetingMFDPage tgpMfd, AIGunner aiGunner)
    {
        this.tgpMfd = tgpMfd;
        this.aiGunner = aiGunner;
    }

    public override bool CanStart()
    {
        if (aiGunner?.target == null)
        {
            return false;
        }

        Vector3 offset = aiGunner.target.position - tgpMfd.opticalTargeter.cameraTransform.position;
        float error = Vector3.Angle(offset, tgpMfd.opticalTargeter.cameraTransform.forward);

        return tgpMfd.fovIdx != tgpMfd.fovs.Length - 1
            && error < tgpMfd.fovs[tgpMfd.fovIdx] * 0.2f * 0.5f
            && (tgpMfd.opticalTargeter.lockedActor == null
            || tgpMfd.opticalTargeter.lockedActor.team == Teams.Allied);
    }

    public override void StartState()
    {
        tgpMfd.ZoomIn();
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
