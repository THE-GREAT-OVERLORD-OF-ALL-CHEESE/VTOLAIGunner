using CheeseMods.AIHelicopterGunner.AIStates;
using CheeseMods.AIHelicopterGunner.Components;

namespace CheeseMods.AIHelicopterGunner.AttackBehaviours;

public abstract class AttackBehaviour_ATGM : AttackBehaviour
{
    public AttackBehaviour_ATGM(AIGunner gunner, WeaponManager weaponManager, State_Sequence sequence, float damage) : base(gunner, weaponManager, sequence)
    {
        this.damage = damage;
    }

    public override string Name => "ATGM";

    private float damage;

    public override bool AppropriateTarget(Actor actor)
    {
        return actor.role == Actor.Roles.GroundArmor || actor.role == Actor.Roles.Ship;
    }

    public override bool CanAttackTarget(Actor actor)
    {
        return actor.healthMinDamage < damage && actor.velocity.magnitude < 40;
    }
}
