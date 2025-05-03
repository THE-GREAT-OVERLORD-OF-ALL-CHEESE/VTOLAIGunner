using System.Collections.Generic;
using System.Linq;
using AIHelicopterGunner.AIHelpers;
using AIHelicopterGunner.AIStates;
using AIHelicopterGunner.AIStates.MFDStates;
using AIHelicopterGunner.AIStates.Power;
using AIHelicopterGunner.AIStates.TGP;
using AIHelicopterGunner.AIStates.WM;
using AIHelicopterGunner.AttackBehaviours;
using UnityEngine;

namespace AIHelicopterGunner.Components;

public class AIGunner : MonoBehaviour
{
    public Actor target;
    private AuxilliaryPowerUnit apu;
    public List<AttackBehaviour> attackBehaviours = new();
    private Battery battery;

    private State_Sequence cannonSequence;
    private State_Sequence guidedRocket;
    private AttackBehaviour lastAttackBehviour;

    private Actor lastTarget;
    private MFD mfd;
    public MissileHelper missileHelper = new();
    private State_Sequence opticalMissile;
    private State_Sequence opticalMissileFaf;

    private State_Sequence sequence;
    private TargetingMFDPage tgpMfd;
    private State_Sequence tow;
    private WeaponManager wm;

    private void Awake()
    {
        Debug.Log("Setting up gunner!");

        battery = GetComponentInChildren<Battery>(true);
        apu = GetComponentInChildren<AuxilliaryPowerUnit>(true);
        wm = GetComponentInChildren<WeaponManager>(true);
        mfd = GetComponentInChildren<MFD>(true);
        tgpMfd = GetComponentInChildren<TargetingMFDPage>(true);

        gameObject.AddComponent<DamageReporter>();
        
        var ensurePower = new State_Sequence(
            new List<AITryState>
            {
                new State_APUStart(apu),
                new State_MasterPowerOn(battery),
                new State_MasterArmOn(wm)
            },
            "Ensure Power",
            1f,
            2f
        );

        var tgpSlewToTarget = new State_Sequence(
            new List<AITryState>
            {
                new State_MFDSwitchOn(mfd),
                new State_MFDSwitchTPGPage(mfd, tgpMfd),
                new State_TGPPagePowerOn(tgpMfd),
                new State_TGPSlewFwd(tgpMfd, this),
                new State_TGPZoomOut(tgpMfd, this),
                new State_TGPZoomIn(tgpMfd, this),
                new State_TGPSlewToTarget(tgpMfd, this)
            },
            "TGP Slew To Target Sequence",
            0f,
            0f
        );

        cannonSequence = new State_Sequence(
            new List<AITryState>
            {
                ensurePower,
                tgpSlewToTarget,
                new State_WMCycleWeapon(wm, typeof(HPEquipGunTurret)),
                new State_WMFireGunTPGLock(wm, tgpMfd),
                tgpSlewToTarget
            },
            "Engage with Cannon",
            0f,
            0f
        );

        opticalMissile = new State_Sequence(
            new List<AITryState>
            {
                ensurePower,
                tgpSlewToTarget,
                new State_WMCycleWeaponOpticalMissile(wm),
                new State_EquipSwitchOpticalMissleAutoUncage(wm, tgpMfd),
                new State_WMFireOpticalMissile(this, wm, tgpMfd)
            },
            "Engage with Optical Missile",
            0f,
            0f
        );

        opticalMissileFaf = new State_Sequence(
            new List<AITryState>
            {
                ensurePower,
                tgpSlewToTarget,
                new State_WMCycleWeaponOpticalMissileFaf(wm),
                new State_EquipSwitchOpticalMissleAutoUncage(wm, tgpMfd),
                new State_WMFireOpticalMissileFaf(this, wm, tgpMfd)
            },
            "Engage with Optical Missile FaF",
            0f,
            0f
        );

        guidedRocket = new State_Sequence(
            new List<AITryState>
            {
                ensurePower,
                tgpSlewToTarget,
                new State_WMCycleWeaponGuidedRocket(wm),
                new State_EquipSwitchOpticalMissleAutoUncage(wm, tgpMfd),
                new State_WMFireGuidedRocketSalvo(this, wm, tgpMfd)
            },
            "Engage with Guided Rockets",
            0f,
            0f
        );

        tow = new State_Sequence(
            new List<AITryState>
            {
                ensurePower,
                tgpSlewToTarget,
                new State_WMCycleWeaponTOW(wm),
                new State_EquipSwitchOpticalMissleAutoUncage(wm, tgpMfd),
                new State_WMFireOpticalMissile(this, wm, tgpMfd)
            },
            "Engage with TOW Missile",
            0f,
            0f
        );

        attackBehaviours = new List<AttackBehaviour>
        {
            new AttackBehaviour_TOW(this, wm, tow, 100f),
            new AttackBehaviour_GuidedRockets(this, wm, guidedRocket, 20f),
            new AttackBehaviour_OpticalMissile(this, wm, opticalMissile, 100f),
            new AttackBehaviour_OpticalMissileFaf(this, wm, opticalMissileFaf, 100f),
            new AttackBehaviour_Guns(this, wm, cannonSequence, 10f)
        };

        sequence = guidedRocket;
    }

    private void Update()
    {
        missileHelper.UpdateMissilePerTarget();

        DetermineTarget(out var target, out var attackBehaviour);
        this.target = target;

        if (attackBehaviour != null) sequence = attackBehaviour.Sequence;

        if (target != lastTarget || attackBehaviour != lastAttackBehviour)
        {
            lastTarget = target;
            lastAttackBehviour = attackBehaviour;

            TutorialLabel.instance.HideLabel();
            TutorialLabel.instance.DisplayLabel(
                $"{Main.aiGunnerName}: Targeting: {target.actorName ?? "nothing"} with {attackBehaviour.Name}",
                null,
                5f);

            Debug.Log(
                $"{Main.aiGunnerName}: New strat is to engage {target.actorName} with {attackBehaviour.Name}({(target.position - transform.position).magnitude}m)");
            Debug.Log($"{wm.equips.Count(e => e is HPEquipGunTurret turret)} gun turrets were available");
            Debug.Log(
                $"{wm.equips.Count(e => e is HPEquipOpticalML ml && MissileHelper.IsNotOpticalFaf(ml))} optical launchers were available");
            Debug.Log(
                $"{wm.equips.Count(e => e is HPEquipOpticalML ml && MissileHelper.IsOpticalFaf(ml))} optical faf launchers were available");
            Debug.Log(
                $"{wm.equips.Count(e => e is HPEquipOpticalML ml && MissileHelper.IsGuidedRocketLauncher(ml))} guided rocket launchers were available");
            Debug.Log(
                $"{wm.equips.Count(e => e is HPEquipOpticalML ml && MissileHelper.IsTow(ml))} tow were available");

            /*
            foreach (AttackBehaviour attackBehaviour in attackBehaviourShortlist)
            {
                Debug.Log($"Behaviour {attackBehaviour.Name}, HasAmmo: {attackBehaviour.HaveAmmo()}, AppropriateTarget: {attackBehaviour.AppropriateTarget(target)}, CanAttackImmediately: {attackBehaviour.CanAttackImmediately(target)}");
            }
            */
        }

        sequence.UpdateState();
    }

    private void DetermineTarget(out Actor outTtarget, out AttackBehaviour outAttackBehaviour)
    {
        var attackBehaviourShortlist = attackBehaviours.Where(a => a.HaveAmmo());

        AttackBehaviour attackBehaviour = null;

        if (tgpMfd.opticalTargeter.lockedActor != null && tgpMfd.opticalTargeter.lockedActor.alive)
        {
            attackBehaviour = attackBehaviourShortlist.FirstOrDefault(a =>
                a.AppropriateTarget(tgpMfd.opticalTargeter.lockedActor)
                && a.CanAttackTarget(tgpMfd.opticalTargeter.lockedActor)
                && a.CanAttackImmediately(tgpMfd.opticalTargeter.lockedActor));
            if (attackBehaviour != null)
            {
                outTtarget = tgpMfd.opticalTargeter.lockedActor;
                outAttackBehaviour = attackBehaviour;
                return;
            }

            attackBehaviour = attackBehaviourShortlist.FirstOrDefault(a =>
                a.CanAttackTarget(tgpMfd.opticalTargeter.lockedActor)
                && a.CanAttackImmediately(tgpMfd.opticalTargeter.lockedActor));
            if (attackBehaviour != null)
            {
                outTtarget = tgpMfd.opticalTargeter.lockedActor;
                outAttackBehaviour = attackBehaviour;
                return;
            }
        }

        IEnumerable<Actor> sortedTarget = TargetManager.instance.enemyUnits
            .Where(u => u.alive
                        && missileHelper.GetMissilesForTarget(u) == 0
                        && !Physics.Raycast(transform.position,
                            u.position - transform.position,
                            out var hit,
                            (u.position - transform.position).magnitude - 5f,
                            1,
                            QueryTriggerInteraction.Ignore))
            .OrderByDescending(u => CalculateThreat(u.position));

        var target = sortedTarget.FirstOrDefault(u =>
            attackBehaviourShortlist.Any(a =>
                a.AppropriateTarget(u) && a.CanAttackTarget(u) && a.CanAttackImmediately(u)));
        if (target != null)
        {
            outTtarget = target;
            outAttackBehaviour = attackBehaviourShortlist.FirstOrDefault(a =>
                a.AppropriateTarget(target) && a.CanAttackTarget(target) && a.CanAttackImmediately(target));
            return;
        }

        target = sortedTarget.FirstOrDefault(u =>
            attackBehaviourShortlist.Any(a => a.CanAttackTarget(u) && a.CanAttackImmediately(u)));
        if (target != null)
        {
            outTtarget = target;
            outAttackBehaviour =
                attackBehaviourShortlist.FirstOrDefault(a =>
                    a.CanAttackTarget(target) && a.CanAttackImmediately(target));
            return;
        }

        outTtarget = null;
        outAttackBehaviour = null;
    }

    public bool InFov(Vector3 position, float fov)
    {
        var offset = position - transform.position;
        return Vector3.Angle(offset, transform.forward) < fov;
    }

    public bool InHFov(Vector3 position, float fov)
    {
        var offset = position - transform.position;
        return Vector3.Angle(Vector3.ProjectOnPlane(offset, Vector3.up),
            Vector3.ProjectOnPlane(transform.forward, Vector3.up)) < fov;
    }

    public bool InRange(Vector3 position, float minRange, float maxRange)
    {
        var offset = position - transform.position;
        return offset.magnitude > minRange && offset.magnitude < maxRange;
    }

    private float CalculateThreat(Vector3 position)
    {
        var offset = position - transform.position;
        return 1f / offset.magnitude;
    }
}