using AIHelicopterGunner.AIHelpers;
using AIHelicopterGunner.Components;
using System;
using UnityEngine;

namespace AIHelicopterGunner.AIStates.TGP
{
    public class State_TGPSlewToTarget : AITryState
    {
        private TargetingMFDPage tgpMfd;
        private AIGunner aiGunner;

        public override string Name => "Slewing TGP To Target";
        public override float WarmUp => 0.2f;
        public override float CoolDown => 0.5f;

        private float minSlewTime = 0.15f;
        private float maxSlewTime = 3f;
        private float slewTimer;

        private Vector3 currentSlew;

        public State_TGPSlewToTarget(TargetingMFDPage tgpMfd, AIGunner aiGunner)
        {
            this.tgpMfd = tgpMfd;
            this.aiGunner = aiGunner;
        }

        public override bool CanStart()
        {
            return aiGunner.target != null
                && (tgpMfd.opticalTargeter.lockedActor == null
                || tgpMfd.opticalTargeter.lockedActor.team == Teams.Allied
                || (tgpMfd.opticalTargeter.lockedActor != aiGunner.target
                && aiGunner.missileHelper.GetMissilesForTarget(tgpMfd.opticalTargeter.lockedActor) > 0));
        }

        public override void StartState()
        {
            Vector3 targetPos = aiGunner.target.position + aiGunner.target.velocity * 0.5f;
            Vector3 dirToTarget = targetPos - tgpMfd.opticalTargeter.cameraTransform.position;
            Vector3 aimDir = tgpMfd.opticalTargeter.cameraTransform.forward;

            Vector3 angleDif = new Vector3(
                Vector3.SignedAngle(
                    Vector3.ProjectOnPlane(aimDir, tgpMfd.opticalTargeter.cameraTransform.up),
                    Vector3.ProjectOnPlane(dirToTarget, tgpMfd.opticalTargeter.cameraTransform.up),
                tgpMfd.opticalTargeter.cameraTransform.up),
                -Vector3.SignedAngle(
                    Vector3.ProjectOnPlane(aimDir, tgpMfd.opticalTargeter.cameraTransform.right),
                    Vector3.ProjectOnPlane(dirToTarget, tgpMfd.opticalTargeter.cameraTransform.right),
                tgpMfd.opticalTargeter.cameraTransform.right),
                0);

            currentSlew = Quaternion.AngleAxis(RandomHelper.BellCurve(-10f, 10f), Vector3.forward) * angleDif.normalized;


            slewTimer = Mathf.Clamp(angleDif.magnitude / (tgpMfd.slewRate * (tgpMfd.fovs[tgpMfd.fovIdx] / 60f)) * RandomHelper.BellCurve(0.8f, 1.2f), minSlewTime, maxSlewTime);
            //tgpMfd.OnSetThumbstick(slewDir);

            Debug.Log($"TGP error {angleDif}, slewing {currentSlew} for {slewTimer} seconds");
        }

        public override void UpdateState()
        {
            tgpMfd.OnSetThumbstick(currentSlew);
            slewTimer -= Time.deltaTime;
        }

        public override bool IsOver()
        {
            return slewTimer < 0f;
        }

        public override void EndState()
        {
            tgpMfd.OnResetThumbstick();
        }
    }
}
