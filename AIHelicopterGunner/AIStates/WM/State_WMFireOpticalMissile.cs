using AIHelicopterGunner.Components;
using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace AIHelicopterGunner.AIStates.WM
{
    public class State_WMFireOpticalMissile : AITryState
    {
        private AIGunner gunner;
        private WeaponManager wm;
        private TargetingMFDPage tgpMfd;

        public override string Name => "Firing Optical Missile";
        public override float WarmUp => 0.5f;
        public override float CoolDown => 0.5f;

        private const float maxError = 15f;
        private const float minRange = 500f;
        private const float maxRange = 4000f;

        private HPEquipOpticalML missileLauncher;
        private Missile launchedMissile;

        public State_WMFireOpticalMissile(AIGunner gunner, WeaponManager wm, TargetingMFDPage tgpMfd)
        {
            this.gunner = gunner;
            this.wm = wm;
            this.tgpMfd = tgpMfd;
        }

        public override bool CanStart()
        {
            if (wm.currentEquip is not HPEquipOpticalML missileLauncher)
            {
                return false;
            }
            if (tgpMfd.opticalTargeter.lockedActor == null
                || tgpMfd.opticalTargeter.lockedActor.team == Teams.Allied
                || !tgpMfd.opticalTargeter.lockedActor.alive)
            {
                return false;
            }
            this.missileLauncher = missileLauncher;

            float error = Vector3.Angle(wm.transform.forward, tgpMfd.opticalTargeter.cameraTransform.forward);
            float range = (tgpMfd.opticalTargeter.lockedActor.position - wm.transform.position).magnitude;

            return error < maxError
                && range < maxRange
                && range > minRange
                //&& missileLauncher.LaunchAuthorized()
                && (launchedMissile == null
                || !launchedMissile.fired)
                && gunner.missileHelper.GetMissilesForTarget(tgpMfd.opticalTargeter.lockedActor) == 0;
        }

        public override void StartState()
        {
            launchedMissile = missileLauncher.ml.GetNextMissile();
            wm.StartFire();
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
            wm.EndFire();
        }
    }
}
