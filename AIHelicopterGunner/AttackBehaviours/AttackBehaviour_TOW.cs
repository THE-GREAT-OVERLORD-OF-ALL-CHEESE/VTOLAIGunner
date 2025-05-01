using AIHelicopterGunner.AIHelpers;
using AIHelicopterGunner.AIStates;
using AIHelicopterGunner.Components;
using System.Linq;

namespace AIHelicopterGunner.AttackBehaviours
{
    public class AttackBehaviour_TOW : AttackBehaviour_ATGM
    {
        public AttackBehaviour_TOW(AIGunner gunner, WeaponManager weaponManager, State_Sequence sequence, float damage) : base(gunner, weaponManager, sequence, damage)
        {
        }

        public override string Name => "TOW Missile";

        public override bool CanAttackImmediately(Actor actor)
        {
            return Gunner.InHFov(actor.position, AIGunnerConsts.towImmediateFov)
                && Gunner.InRange(actor.position, AIGunnerConsts.towMinRange, AIGunnerConsts.towMaxRange);
        }

        public override bool HaveAmmo()
        {
            return WeaponManager.equips.Any(e => e is HPEquipOpticalML ml
            && MissileHelper.IsTow(ml)
            && ml.armed
            && ml.GetCount() > 0);
        }
    }
}
