using AIHelicopterGunner.AIHelpers;
using UnityEngine;

namespace AIHelicopterGunner.AIStates.WM
{
    public class State_WMFireGunTPGLock : AITryState
    {
        private WeaponManager wm;
        private TargetingMFDPage tgpMfd;

        public override string Name => "Firing Gun";
        public override float WarmUp => 0.5f;
        public override float CoolDown => 3f;

        private const float maxError = 2f;
        private const float minRange = 50f;
        private const float maxRange = 2000f;

        private const float maxBurstLength = 0.3f;
        private const float minBurstLength = 1.5f;
        private float burstCoolDown;

        public State_WMFireGunTPGLock(WeaponManager wm, TargetingMFDPage tgpMfd)
        {
            this.wm = wm;
            this.tgpMfd = tgpMfd;
        }

        public override bool CanStart()
        {
            if (wm.currentEquip is not HPEquipGunTurret turret)
            {
                return false;
            }
            if (tgpMfd.opticalTargeter.lockedActor == null
                || tgpMfd.opticalTargeter.lockedActor.team == Teams.Allied
                || !tgpMfd.opticalTargeter.lockedActor.alive)
            {
                return false;
            }

            float error = Vector3.Angle(turret.turret.pitchTransform.forward, tgpMfd.opticalTargeter.cameraTransform.forward);
            float range = (tgpMfd.opticalTargeter.lockedActor.position - turret.turret.pitchTransform.position).magnitude;

            return error < maxError
                && range < maxRange
                && range > minRange;
        }

        public override void StartState()
        {
            wm.StartFire();
            burstCoolDown = RandomHelper.BellCurve(minBurstLength, maxBurstLength);
        }

        public override void UpdateState()
        {
            burstCoolDown -= Time.deltaTime;
        }

        public override bool IsOver()
        {
            return burstCoolDown < 0;
        }

        public override void EndState()
        {
            wm.EndFire();
        }
    }
}
