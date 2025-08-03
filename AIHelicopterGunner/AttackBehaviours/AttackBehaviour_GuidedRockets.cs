using CheeseMods.AIHelicopterGunner.AIHelpers;
using CheeseMods.AIHelicopterGunner.AIStates;
using CheeseMods.AIHelicopterGunner.Components;
using CheeseMods.AIHelicopterGunner.Spotting;
using System.Linq;

namespace CheeseMods.AIHelicopterGunner.AttackBehaviours;

public class AttackBehaviour_GuidedRockets : AttackBehaviour_ATGM
{
    public AttackBehaviour_GuidedRockets(AIGunner gunner, WeaponManager weaponManager, FlightInfo flightInfo, State_Sequence sequence, float damage) : base(gunner, weaponManager, flightInfo, sequence, damage)
    {
    }

    public override string Name => "Guided Rocket";

    public override bool CanAttackImmediately(TargetMetaData target)
    {
        return Gunner.InHFov(target.Actor.position, AIGunnerConsts.guidedRocketImmediateFov)
            && Gunner.InRange(target.Actor.position, AIGunnerConsts.guidedRocketMinRange, AIGunnerConsts.guidedRocketMaxRange);
    }

    public override bool HaveAmmo()
    {
        return WeaponManager.equips.Any(e => e is HPEquipOpticalML ml
        && MissileHelper.IsGuidedRocketLauncher(ml)
        && ml.armed
        && ml.GetCount() > 0);
    }
}
