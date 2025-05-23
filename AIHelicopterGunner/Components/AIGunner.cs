﻿using CheeseMods.AIHelicopterGunner.AIHelpers;
using CheeseMods.AIHelicopterGunner.AIStates;
using CheeseMods.AIHelicopterGunner.AIStates.Equip;
using CheeseMods.AIHelicopterGunner.AIStates.MFDStates;
using CheeseMods.AIHelicopterGunner.AIStates.Power;
using CheeseMods.AIHelicopterGunner.AIStates.TGP;
using CheeseMods.AIHelicopterGunner.AIStates.WM;
using CheeseMods.AIHelicopterGunner.AttackBehaviours;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VTOLVR.DLC.Rotorcraft;

namespace CheeseMods.AIHelicopterGunner.Components;

public class AIGunner : MonoBehaviour
{
    private Battery battery;
    private AuxilliaryPowerUnit apu;
    private WeaponManager wm;
    private MFD mfd;
    private TargetingMFDPage tgpMfd;

    private ArticulatingHardpoint autoPylon;

    private State_Sequence sequence;

    private State_Sequence cannonSequence;
    private State_Sequence opticalMissile;
    private State_Sequence opticalMissileFaf;
    private State_Sequence guidedRocket;
    private State_Sequence tow;

    public Actor target;
    public MissileHelper missileHelper = new MissileHelper();
    public List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();

    private Actor lastTarget;
    private AttackBehaviour lastAttackBehviour;

    private void Awake()
    {
        Debug.Log("Setting up gunner!");

        battery = GetComponentInChildren<Battery>(true);
        apu = GetComponentInChildren<AuxilliaryPowerUnit>(true);
        wm = GetComponentInChildren<WeaponManager>(true);
        mfd = GetComponentInChildren<MFD>(true);
        tgpMfd = GetComponentInChildren<TargetingMFDPage>(true);
        gameObject.AddComponent<DamageReporter>();
        autoPylon = GetComponentInChildren<ArticulatingHardpoint>(true);
        

        State_Sequence ensurePower = new State_Sequence(
            new List<AITryState>
            {
                new State_APUStart(apu),
                new State_MasterPowerOn(battery),
                new State_MasterArmOn(wm),
            },
            "Ensure Power",
            1f,
            2f
        );

        State_Sequence tgpSlewToTarget = new State_Sequence(
            new List<AITryState>
            {
                new State_MFDSwitchOn(mfd),
                new State_MFDSwitchTPGPage(mfd, tgpMfd),
                new State_TGPPagePowerOn(tgpMfd),
                new State_TGPSlewFwd(tgpMfd, this),
                new State_TGPZoomOut(tgpMfd, this),
                new State_TGPZoomIn(tgpMfd, this),
                new State_TGPSlewToTarget(tgpMfd, this),
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
                new State_PylonAuto(autoPylon),
                new State_WMFireOpticalMissile(this, wm, tgpMfd),
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
                new State_PylonAuto(autoPylon),
                new State_WMFireOpticalMissileFaf(this, wm, tgpMfd),
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
                new State_PylonAuto(autoPylon),
                new State_WMFireGuidedRocketSalvo(this, wm, tgpMfd),
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
                new State_PylonAuto(autoPylon),
                new State_WMFireOpticalMissile(this, wm, tgpMfd),
            },
            "Engage with TOW Missile",
            0f,
            0f
        );

        attackBehaviours = new List<AttackBehaviour>()
        {
            new AttackBehaviour_TOW(this, wm, tow, 100f),
            new AttackBehaviour_GuidedRockets(this, wm, guidedRocket, 20f),
            new AttackBehaviour_OpticalMissile(this, wm, opticalMissile, 100f),
            new AttackBehaviour_OpticalMissileFaf(this, wm, opticalMissileFaf, 100f),
            new AttackBehaviour_Guns(this, wm, cannonSequence, 10f)
        };

        sequence = cannonSequence;
    }

    private void Update()
    {
        missileHelper.UpdateMissilePerTarget();

        if (sequence.Idle)
        {
            DetermineTarget(out Actor target, out AttackBehaviour attackBehaviour);
            this.target = target;

            if (attackBehaviour != null)
            {
                sequence = attackBehaviour.Sequence;

                if (target != lastTarget || attackBehaviour != lastAttackBehviour)
                {
                    lastTarget = target;
                    lastAttackBehviour = attackBehaviour;

                    Vector3 offset = target.position - transform.position;
                    float angularSize = Mathf.Atan2(target.physicalRadius, offset.magnitude) * Mathf.Rad2Deg * 2f;

                    TutorialLabel.instance.HideLabel();
                    TutorialLabel.instance.DisplayLabel($"{Main.aiGunnerName}: Targeting: {target.actorName}({offset.magnitude}m)(radius {target.physicalRadius}m)({angularSize} degrees ({angularSize / GunnerAIConfig.minimumTargetSizeAngular * 100f}%)) with {attackBehaviour.Name}",
                        null,
                        5f);

                    Debug.Log($"{Main.aiGunnerName}: New strat is to engage {target.actorName}({offset.magnitude}m)(radius {target.physicalRadius}m)({angularSize} degrees ({angularSize / GunnerAIConfig.minimumTargetSizeAngular * 100f}%)) with {attackBehaviour.Name}");
                    Debug.Log($"{wm.equips.Count(e => e is HPEquipGunTurret turret)} gun turrets were available");
                    Debug.Log($"{wm.equips.Count(e => e is HPEquipOpticalML ml && MissileHelper.IsNotOpticalFaf(ml))} optical launchers were available");
                    Debug.Log($"{wm.equips.Count(e => e is HPEquipOpticalML ml && MissileHelper.IsOpticalFaf(ml))} optical faf launchers were available");
                    Debug.Log($"{wm.equips.Count(e => e is HPEquipOpticalML ml && MissileHelper.IsGuidedRocketLauncher(ml))} guided rocket launchers were available");
                    Debug.Log($"{wm.equips.Count(e => e is HPEquipOpticalML ml && MissileHelper.IsTow(ml))} tow were available");

                    /*
                    foreach (AttackBehaviour attackBehaviour in attackBehaviourShortlist)
                    {
                        Debug.Log($"Behaviour {attackBehaviour.Name}, HasAmmo: {attackBehaviour.HaveAmmo()}, AppropriateTarget: {attackBehaviour.AppropriateTarget(target)}, CanAttackImmediately: {attackBehaviour.CanAttackImmediately(target)}");
                    }
                    */
                }
            }
        }

        sequence.UpdateState();
    }

    private void DetermineTarget(out Actor outTtarget, out AttackBehaviour outAttackBehaviour)
    {
        IEnumerable<AttackBehaviour> attackBehaviourShortlist = attackBehaviours.Where(a => a.HaveAmmo());

        AttackBehaviour attackBehaviour = null;

        if (tgpMfd.opticalTargeter.lockedActor != null && tgpMfd.opticalTargeter.lockedActor.alive)
        {
            attackBehaviour = attackBehaviourShortlist.FirstOrDefault(a => a.AppropriateTarget(tgpMfd.opticalTargeter.lockedActor)
                && a.CanAttackTarget(tgpMfd.opticalTargeter.lockedActor)
                && a.CanAttackImmediately(tgpMfd.opticalTargeter.lockedActor));
            if (attackBehaviour != null)
            {
                outTtarget = tgpMfd.opticalTargeter.lockedActor;
                outAttackBehaviour = attackBehaviour;
                return;
            }

            attackBehaviour = attackBehaviourShortlist.FirstOrDefault(a => a.CanAttackTarget(tgpMfd.opticalTargeter.lockedActor)
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
                && DetermineVisible(u))
            .OrderByDescending(u => CalculateThreat(u.position));

        Actor target = sortedTarget.FirstOrDefault(u => attackBehaviourShortlist.Any(a => a.AppropriateTarget(u) && a.CanAttackTarget(u) && a.CanAttackImmediately(u)));
        if (target != null)
        {
            outTtarget = target;
            outAttackBehaviour = attackBehaviourShortlist.FirstOrDefault(a => a.AppropriateTarget(target) && a.CanAttackTarget(target) && a.CanAttackImmediately(target));
            return;
        }

        target = sortedTarget.FirstOrDefault(u => attackBehaviourShortlist.Any(a => a.CanAttackTarget(u) && a.CanAttackImmediately(u)));
        if (target != null)
        {
            outTtarget = target;
            outAttackBehaviour = attackBehaviourShortlist.FirstOrDefault(a => a.CanAttackTarget(target) && a.CanAttackImmediately(target));
            return;
        }

        outTtarget = null;
        outAttackBehaviour = null;
    }

    public bool DetermineVisible(Actor actor)
    {
        Vector3 offset = actor.position - transform.position;
        float angularSize = Mathf.Atan2(actor.physicalRadius, offset.magnitude) * Mathf.Rad2Deg * 2f;

        return angularSize > GunnerAIConfig.minimumTargetSizeAngular
            && !Physics.Raycast(transform.position,
                offset,
                out RaycastHit hit,
                offset.magnitude - 5f,
                1,
                QueryTriggerInteraction.Ignore)
            && !TargetManager.IsOccludedByClouds(transform.position, actor.position, 2.2f);
    }

    public bool InFov(Vector3 position, float fov)
    {
        Vector3 offset = position - transform.position;
        return Vector3.Angle(offset, transform.forward) < fov;
    }

    public bool InHFov(Vector3 position, float fov)
    {
        Vector3 offset = position - transform.position;
        return Vector3.Angle(Vector3.ProjectOnPlane(offset, Vector3.up), Vector3.ProjectOnPlane(transform.forward, Vector3.up)) < fov;
    }

    public bool InRange(Vector3 position, float minRange, float maxRange)
    {
        Vector3 offset = position - transform.position;
        return offset.magnitude > minRange && offset.magnitude < maxRange;
    }

    private float CalculateThreat(Vector3 position)
    {
        Vector3 offset = position - transform.position;
        return 1f / offset.magnitude;
    }
}
