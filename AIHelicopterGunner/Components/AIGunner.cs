using CheeseMods.AIHelicopterGunner.AIHelpers;
using CheeseMods.AIHelicopterGunner.AIStates;
using CheeseMods.AIHelicopterGunner.AIStates.Equip;
using CheeseMods.AIHelicopterGunner.AIStates.MFDStates;
using CheeseMods.AIHelicopterGunner.AIStates.Power;
using CheeseMods.AIHelicopterGunner.AIStates.TGP;
using CheeseMods.AIHelicopterGunner.AIStates.WM;
using CheeseMods.AIHelicopterGunner.AttackBehaviours;
using CheeseMods.AIHelicopterGunner.Character;
using CheeseMods.AIHelicopterGunner.Spotting;
using CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects;
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
    private FlightInfo flightInfo;
    private MFD mfd;
    private TargetingMFDPage tgpMfd;
    private HelicopterRotor rotor;
    private RotorBrake rotorBrake;
    private List<TurbineStarterMotor> starterMotors;
    private HeliPowerGovernor governor;

    private ArticulatingHardpoint autoPylon;

    private State_Sequence sequence;

    private State_Sequence cannonSequence;
    private State_Sequence opticalMissile;
    private State_Sequence opticalMissileFaf;
    private State_Sequence guidedRocket;
    private State_Sequence tow;

    public TargetMetaDataManager metaDataManager = new TargetMetaDataManager();
    public GunnerTargetSpotter targetSpotter;
    public Actor target;
    public MissileHelper missileHelper = new MissileHelper();
    public List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();

    private Actor lastTarget;
    private AttackBehaviour lastAttackBehviour;

    public IVoice voice;

    private void Awake()
    {
        Debug.Log("Setting up gunner!");

        battery = GetComponentInChildren<Battery>(true);
        apu = GetComponentInChildren<AuxilliaryPowerUnit>(true);
        wm = GetComponentInChildren<WeaponManager>(true);
        flightInfo = GetComponentInChildren<FlightInfo>(true);
        mfd = GetComponentInChildren<MFD>(true);
        tgpMfd = GetComponentInChildren<TargetingMFDPage>(true);
        starterMotors = GetComponentsInChildren<TurbineStarterMotor>(true).ToList();
        rotor = GetComponentInChildren<HelicopterRotor>(true);
        rotorBrake = GetComponentInChildren<RotorBrake>(true);
        governor = GetComponentInChildren<HeliPowerGovernor>(true);
        ContactReporter contactReporter = gameObject.AddComponent<ContactReporter>();
        DamageReporter damageReporter = gameObject.AddComponent<DamageReporter>();
        damageReporter.voice = voice;
        autoPylon = GetComponentInChildren<ArticulatingHardpoint>(true);

        targetSpotter = new GunnerTargetSpotter(metaDataManager, transform, missileHelper);
        voice = new RadioCommsVoice(AssetLoader.voice);

        State_Sequence ensurePower = new State_Sequence(
            new List<AITryState>
            {
                new State_APUStart(apu, voice),
                new State_MasterPowerOn(battery, voice),
                new State_MasterArmOn(wm, voice),

                new State_EngineThrottleIdle(governor, voice),
                new State_EngineStart(starterMotors[0], new List<TurbineStarterMotor>(), voice, "Engine 1", LineType.Engine1),
                new State_EngineStart(starterMotors[1], new List<TurbineStarterMotor>(){ starterMotors[0] }, voice, "Engine 2", LineType.Engine2),
                new State_RotorUnfold(rotor, voice),
                new State_RotorBrakeOff(rotorBrake, voice),
                new State_EngineThrottleFull(governor, voice),
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
                new State_TGPPagePowerOn(tgpMfd, voice),
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
                new State_WMFireGunTPGLock(wm, tgpMfd, voice),
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
                new State_WMFireOpticalMissile(this, wm, tgpMfd, voice),
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
                new State_WMFireOpticalMissileFaf(this, wm, tgpMfd, voice),
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
                new State_WMFireGuidedRocketSalvo(this, wm, tgpMfd, voice),
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
                new State_WMFireOpticalMissile(this, wm, tgpMfd, voice),
            },
            "Engage with TOW Missile",
            0f,
            0f
        );

        attackBehaviours = new List<AttackBehaviour>()
        {
            new AttackBehaviour_TOW(this, wm, flightInfo, tow, 100f),
            new AttackBehaviour_GuidedRockets(this, wm, flightInfo, guidedRocket, 20f),
            new AttackBehaviour_OpticalMissile(this, wm, flightInfo, opticalMissile, 100f),
            new AttackBehaviour_OpticalMissileFaf(this, wm, flightInfo, opticalMissileFaf, 100f),
            new AttackBehaviour_Guns(this, wm, cannonSequence, 10f)
        };

        sequence = cannonSequence;
    }

    private void Update()
    {
        targetSpotter.UpdateVisibleTargets();
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

                    voice.Say($"{Main.aiGunnerName}: Targeting: {target.actorName}({offset.magnitude}m)(radius {target.physicalRadius}m)({angularSize} degrees ({angularSize / GunnerAIConfig.minimumTargetSizeAngular * 100f}%)) with {attackBehaviour.Name}");

                    Debug.Log($"{Main.aiGunnerName}: New strat is to engage {target.actorName}({offset.magnitude}m)(radius {target.physicalRadius}m)({angularSize} degrees ({angularSize / GunnerAIConfig.minimumTargetSizeAngular * 100f}%)) with {attackBehaviour.Name}");
                    Debug.Log($"{wm.equips.Count(e => e is HPEquipGunTurret turret)} gun turrets were available");
                    Debug.Log($"{wm.equips.Count(e => e is HPEquipOpticalML ml && MissileHelper.IsNotOpticalFaf(ml))} optical launchers were available");
                    Debug.Log($"{wm.equips.Count(e => e is HPEquipOpticalML ml && MissileHelper.IsOpticalFaf(ml))} optical faf launchers were available");
                    Debug.Log($"{wm.equips.Count(e => e is HPEquipOpticalML ml && MissileHelper.IsGuidedRocketLauncher(ml))} guided rocket launchers were available");
                    Debug.Log($"{wm.equips.Count(e => e is HPEquipOpticalML ml && MissileHelper.IsTow(ml))} tow were available");


                    IEnumerable<TargetMetaData> sortedTargets = targetSpotter.visibleTargets
                        .Where(u => u.Actor != null
                            && u.Alive
                            && missileHelper.GetMissilesForTarget(u.Actor) == 0)
                        .OrderByDescending(u => u.CanAttack && u.Hostile)
                        .ThenByDescending(u => u.SpecialFlags.HasFlag(TargetTypeSpecial.Radar))
                        .ThenByDescending(u => InRange(u.Actor.position, 0f, u.MaxRange))
                        .ThenByDescending(u => u.SpecialFlags.HasFlag(TargetTypeSpecial.Missile))
                        .ThenByDescending(u => u.Value)
                        .ThenByDescending(u => CalculateThreat(u.Actor.position));

                    Debug.Log($"Desired engagement order is ");
                    foreach (TargetMetaData targetMetaData in sortedTargets)
                    {
                        Debug.Log($"Target {targetMetaData.Actor.actorName}:");
                        Debug.Log($"Can Attack: {targetMetaData.CanAttack}, Hostile: {targetMetaData.Hostile}, Has Radar: {targetMetaData.SpecialFlags.HasFlag(TargetTypeSpecial.Radar)}, In Range: {InRange(targetMetaData.Actor.position, 0f, targetMetaData.MaxRange)}, Has Missile: {targetMetaData.SpecialFlags.HasFlag(TargetTypeSpecial.Missile)}, Max Range: {targetMetaData.MaxRange}, Range {Range(targetMetaData.Actor.position)}, Value: {targetMetaData.Value}");
                    }
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
            attackBehaviour = attackBehaviourShortlist.FirstOrDefault(a => a.AppropriateTarget(tgpMfd.opticalTargeter.lockedActor, metaDataManager)
                && a.CanAttackTarget(tgpMfd.opticalTargeter.lockedActor, metaDataManager)
                && a.CanAttackImmediately(tgpMfd.opticalTargeter.lockedActor, metaDataManager));
            if (attackBehaviour != null)
            {
                outTtarget = tgpMfd.opticalTargeter.lockedActor;
                outAttackBehaviour = attackBehaviour;
                return;
            }

            attackBehaviour = attackBehaviourShortlist.FirstOrDefault(a => a.CanAttackTarget(tgpMfd.opticalTargeter.lockedActor, metaDataManager)
                && a.CanAttackImmediately(tgpMfd.opticalTargeter.lockedActor, metaDataManager));
            if (attackBehaviour != null)
            {
                outTtarget = tgpMfd.opticalTargeter.lockedActor;
                outAttackBehaviour = attackBehaviour;
                return;
            }
        }

        IEnumerable<TargetMetaData> sortedTargets = targetSpotter.visibleTargets
            .Where(u => u.Actor != null
                && u.Alive
                && missileHelper.GetMissilesForTarget(u.Actor) == 0)
            .OrderByDescending(u => u.CanAttack && u.Hostile)
            .ThenByDescending(u => u.SpecialFlags.HasFlag(TargetTypeSpecial.Radar))
            .ThenByDescending(u => InRange(u.Actor.position, 0f, u.MaxRange))
            .ThenByDescending(u => u.SpecialFlags.HasFlag(TargetTypeSpecial.Missile))
            .ThenByDescending(u => u.Value)
            .ThenByDescending(u => CalculateThreat(u.Actor.position));

        TargetMetaData target = sortedTargets.FirstOrDefault(u => attackBehaviourShortlist.Any(a => a.AppropriateTarget(u) && a.CanAttackTarget(u) && a.CanAttackImmediately(u)));
        if (target != null)
        {
            outTtarget = target.Actor;
            outAttackBehaviour = attackBehaviourShortlist.FirstOrDefault(a => a.AppropriateTarget(target) && a.CanAttackTarget(target) && a.CanAttackImmediately(target));
            return;
        }

        target = sortedTargets.FirstOrDefault(u => attackBehaviourShortlist.Any(a => a.CanAttackTarget(u) && a.CanAttackImmediately(u)));
        if (target != null)
        {
            outTtarget = target.Actor;
            outAttackBehaviour = attackBehaviourShortlist.FirstOrDefault(a => a.CanAttackTarget(target) && a.CanAttackImmediately(target));
            return;
        }

        outTtarget = null;
        outAttackBehaviour = null;
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
        float range = Range(position);
        return range > minRange && range < maxRange;
    }

    private float CalculateThreat(Vector3 position)
    {
        return 1f / Range(position);
    }

    public float Range(Vector3 position)
    {
        Vector3 offset = position - transform.position;
        return offset.magnitude;
    }
}
