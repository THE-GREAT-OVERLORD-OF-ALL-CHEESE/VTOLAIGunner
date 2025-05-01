using AIHelicopterGunner.AIHelpers;
using AIHelicopterGunner.AIStates;
using AIHelicopterGunner.Components;
using System.Linq;

namespace AIHelicopterGunner.AttackBehaviours
{
    public class AttackBehaviour_OpticalMissile : AttackBehaviour_ATGM
    {
        public AttackBehaviour_OpticalMissile(AIGunner gunner, WeaponManager weaponManager, State_Sequence sequence, float damage) : base(gunner, weaponManager, sequence, damage)
        {
        }

        public override string Name => "Optical Missile";

        public override bool CanAttackImmediately(Actor actor)
        {
            return Gunner.InHFov(actor.position, AIGunnerConsts.omImmediateFov)
                && Gunner.InRange(actor.position, AIGunnerConsts.omMinRange, AIGunnerConsts.omMaxRange);
        }

        public override bool HaveAmmo()
        {
            return WeaponManager.equips.Any(e => e is HPEquipOpticalML ml
            && MissileHelper.IsNotOpticalFaf(ml)
            && ml.armed
            && ml.GetCount() > 0);
        }
    }
}
