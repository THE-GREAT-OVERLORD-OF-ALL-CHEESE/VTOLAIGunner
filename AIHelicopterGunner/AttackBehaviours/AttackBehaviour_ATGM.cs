using CheeseMods.AIHelicopterGunner.AIStates;
using CheeseMods.AIHelicopterGunner.Components;
using CheeseMods.AIHelicopterGunner.Spotting;

namespace CheeseMods.AIHelicopterGunner.AttackBehaviours;

public abstract class AttackBehaviour_ATGM : AttackBehaviour
{
    public AttackBehaviour_ATGM(AIGunner gunner, WeaponManager weaponManager, FlightInfo flightInfo, State_Sequence sequence, float damage) : base(gunner, weaponManager, sequence)
    {
        this.damage = damage;
        this.flightInfo = flightInfo;
    }

    public override string Name => "ATGM";

    private FlightInfo flightInfo;
    private float damage;

    public override bool AppropriateTarget(TargetMetaData target)
    {
        return target.Actor.role == Actor.Roles.GroundArmor || target.Actor.role == Actor.Roles.Ship;
    }

    public override bool CanAttackTarget(TargetMetaData target)
    {
        return !flightInfo.isLanded && target.Actor.healthMinDamage < damage && target.Actor.velocity.magnitude < 40;
    }
}
