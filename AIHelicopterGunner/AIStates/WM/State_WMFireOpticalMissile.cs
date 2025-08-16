﻿using CheeseMods.AIHelicopterGunner.Character;
using CheeseMods.AIHelicopterGunner.Components;
using CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects;
using System;
using UnityEngine;

namespace CheeseMods.AIHelicopterGunner.AIStates.WM;

public class State_WMFireOpticalMissile : AITryState
{
    private AIGunner gunner;
    private WeaponManager wm;
    private TargetingMFDPage tgpMfd;

    private Report targetDestroyed;
    private Report missileTrashed;

    public override string Name => "Firing Optical Missile";
    public override float WarmUp => 0.5f;
    public override float CoolDown => 0.5f;

    private HPEquipOpticalML missileLauncher;
    private Missile launchedMissile;

    private Actor currentTarget;

    public State_WMFireOpticalMissile(AIGunner gunner, WeaponManager wm, TargetingMFDPage tgpMfd, IVoice voice)
    {
        this.gunner = gunner;
        this.wm = wm;
        this.tgpMfd = tgpMfd;

        targetDestroyed = new Report(10f,
            () => false,
            () => currentTarget == null || currentTarget.alive == false,
            () => voice.Say(LineType.Splash));

        missileTrashed = new Report(10f,
            () => currentTarget == null || currentTarget.alive == false,
            () => launchedMissile == null,
            () => voice.Say(LineType.Trashed));
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

        return error < AIGunnerConsts.omImmediateFov
            && range < AIGunnerConsts.omMaxRange
            && range > AIGunnerConsts.omMinRange
            //&& missileLauncher.LaunchAuthorized()
            && (launchedMissile == null
            || !launchedMissile.fired)
            && gunner.missileHelper.GetMissilesForTarget(tgpMfd.opticalTargeter.lockedActor) == 0;
    }

    public override void StartState()
    {
        currentTarget = tgpMfd.opticalTargeter.lockedActor;
        launchedMissile = missileLauncher.ml.GetNextMissile();
        wm.StartFire();

        SharedCallouts.rifleCallout.SayCallout();
        targetDestroyed.StartReport();
        missileTrashed.StartReport();
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
