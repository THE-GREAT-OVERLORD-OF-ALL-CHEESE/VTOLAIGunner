using AIHelicopterGunner.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIHelicopterGunner.AIStates.WM
{
    public class State_WMFireGuidedRocketSalvo : AITryState
    {
        private AIGunner gunner;
        private WeaponManager wm;
        private TargetingMFDPage tgpMfd;

        public override string Name => "Firing Guided Rocket Salvo";
        public override float WarmUp => 0.5f;
        public override float CoolDown => 0.5f;

        private HPEquipOpticalML missileLauncher;

        private const int maxSalvoSize = 5;
        private int salvoLeft;
        private const float shotCoolDown = 0.3f;
        private float shotCoolDownTimer;

        private List<Missile> missiles = new List<Missile>();

        public State_WMFireGuidedRocketSalvo(AIGunner gunner, WeaponManager wm, TargetingMFDPage tgpMfd)
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

            return error < AIGunnerConsts.guidedRocketImmediateFov
                && range < AIGunnerConsts.guidedRocketMaxRange
                && range > AIGunnerConsts.guidedRocketMinRange
                //&& missileLauncher.LaunchAuthorized()
                && gunner.missileHelper.GetMissilesForTarget(tgpMfd.opticalTargeter.lockedActor) == 0
                && !missiles.Any(m => m != null && m.fired);
        }

        public override void StartState()
        {
            missiles.Clear();
            if (tgpMfd.opticalTargeter.lockedActor == null)
            {
                return;
            }

            salvoLeft = Mathf.Clamp(Mathf.CeilToInt(tgpMfd.opticalTargeter.lockedActor.health.maxHealth / 30), 1, maxSalvoSize);
        }

        public override void UpdateState()
        {
            shotCoolDownTimer -= Time.deltaTime;
            if (shotCoolDownTimer < 0)
            {
                shotCoolDownTimer = shotCoolDown;
                salvoLeft--;

                missiles.Add(missileLauncher.ml.GetNextMissile());
                wm.StartFire();
                wm.EndFire();
            }
        }

        public override bool IsOver()
        {
            return salvoLeft <= 0
                || tgpMfd.opticalTargeter.lockedActor == null
                || tgpMfd.opticalTargeter.lockedActor.team == Teams.Allied
                || !tgpMfd.opticalTargeter.lockedActor.alive;
        }

        public override void EndState()
        {

        }
    }
}
