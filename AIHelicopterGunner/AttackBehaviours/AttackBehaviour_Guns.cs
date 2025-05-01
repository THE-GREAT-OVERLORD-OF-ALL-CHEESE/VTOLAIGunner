using AIHelicopterGunner.AIStates;
using AIHelicopterGunner.Components;
using System.Linq;

namespace AIHelicopterGunner.AttackBehaviours
{
    public class AttackBehaviour_Guns : AttackBehaviour
    {
        public AttackBehaviour_Guns(AIGunner gunner, WeaponManager weaponManager, State_Sequence sequence, float damage) : base(gunner, weaponManager, sequence)
        {
            this.damage = damage;
        }

        public override string Name => "Guns";

        private float damage;

        public override bool AppropriateTarget(Actor actor)
        {
            return CanAttackTarget(actor);
        }

        public override bool CanAttackImmediately(Actor actor)
        {
            return Gunner.InHFov(actor.position, AIGunnerConsts.gunFov)
                && Gunner.InRange(actor.position, AIGunnerConsts.gunMinRange, AIGunnerConsts.gunMaxRange);
        }

        public override bool CanAttackTarget(Actor actor)
        {
            return actor.healthMinDamage < damage;
        }

        public override bool HaveAmmo()
        {
            return WeaponManager.equips.Any(e => e is HPEquipGunTurret turret
            && turret.armed
            && turret.GetCount() > 0);
        }
    }
}
