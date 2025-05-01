using AIHelicopterGunner.AIHelpers;
using AIHelicopterGunner.AIStates;
using AIHelicopterGunner.Components;
using System.Linq;

namespace AIHelicopterGunner.AttackBehaviours
{
    public class AttackBehaviour_GuidedRockets : AttackBehaviour_ATGM
    {
        public AttackBehaviour_GuidedRockets(AIGunner gunner, WeaponManager weaponManager, State_Sequence sequence, float damage) : base(gunner, weaponManager, sequence, damage)
        {
        }

        public override string Name => "Guided Rocket";

        public override bool CanAttackImmediately(Actor actor)
        {
            return Gunner.InHFov(actor.position, AIGunnerConsts.guidedRocketImmediateFov)
                && Gunner.InRange(actor.position, AIGunnerConsts.guidedRocketMinRange, AIGunnerConsts.guidedRocketMaxRange);
        }

        public override bool HaveAmmo()
        {
            return WeaponManager.equips.Any(e => e is HPEquipOpticalML ml
            && MissileHelper.IsGuidedRocketLauncher(ml)
            && ml.armed
            && ml.GetCount() > 0);
        }
    }
}
