using CheeseMods.AIHelicopterGunner.AIStates;
using CheeseMods.AIHelicopterGunner.Components;
using CheeseMods.AIHelicopterGunner.Spotting;
using System.Linq;

namespace CheeseMods.AIHelicopterGunner.AttackBehaviours;

public class AttackBehaviour_Guns : AttackBehaviour
{
    public AttackBehaviour_Guns(AIGunner gunner, WeaponManager weaponManager, State_Sequence sequence, float damage) : base(gunner, weaponManager, sequence)
    {
        this.damage = damage;
    }

    public override string Name => "Guns";

    private float damage;

    public override bool AppropriateTarget(TargetMetaData target)
    {
        return CanAttackTarget(target);
    }

    public override bool CanAttackImmediately(TargetMetaData target)
    {
        return Gunner.InHFov(target.Actor.position, AIGunnerConsts.gunFov)
            && Gunner.InRange(target.Actor.position, AIGunnerConsts.gunMinRange, AIGunnerConsts.gunMaxRange);
    }

    public override bool CanAttackTarget(TargetMetaData target)
    {
        return target.Actor.healthMinDamage < damage;
    }

    public override bool HaveAmmo()
    {
        return WeaponManager.equips.Any(e => e is HPEquipGunTurret turret
        && turret.armed
        && turret.GetCount() > 0);
    }
}
