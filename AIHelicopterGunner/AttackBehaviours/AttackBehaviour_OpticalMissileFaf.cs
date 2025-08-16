using CheeseMods.AIHelicopterGunner.AIHelpers;
using CheeseMods.AIHelicopterGunner.AIStates;
using CheeseMods.AIHelicopterGunner.Components;
using CheeseMods.AIHelicopterGunner.Spotting;
using System.Linq;

namespace CheeseMods.AIHelicopterGunner.AttackBehaviours;

public class AttackBehaviour_OpticalMissileFaf : AttackBehaviour_ATGM
{
    public AttackBehaviour_OpticalMissileFaf(AIGunner gunner, WeaponManager weaponManager, FlightInfo flightInfo, State_Sequence sequence, float damage) : base(gunner, weaponManager, flightInfo, sequence, damage)
    {
    }

    public override string Name => "Optical Missile FAF";

    public override bool CanAttackImmediately(TargetMetaData target)
    {
        return Gunner.InHFov(target.Actor.position, AIGunnerConsts.omImmediateFov)
            && Gunner.InRange(target.Actor.position, AIGunnerConsts.omMinRange, AIGunnerConsts.omMaxRange);
    }

    public override bool HaveAmmo()
    {
        return WeaponManager.equips.Any(e => e is HPEquipOpticalML ml
        && MissileHelper.IsOpticalFaf(ml)
        && ml.armed
        && ml.GetCount() > 0);
    }
}
