using AIHelicopterGunner.AIHelpers;

namespace AIHelicopterGunner.AIStates.WM
{
    public class State_WMCycleWeaponTOW :  State_WMCycleWeapon
    {
        public override float CoolDown => 1f;

        public override string Name => $"Cycle to {weaponType} (TOW)";

        public State_WMCycleWeaponTOW(WeaponManager wm) : base(wm, typeof(HPEquipOpticalML))
        {

        }

        public override bool CanStart()
        {
            HPEquippable equip = wm.currentEquip;
            return equip.GetCount() <= 0
                || equip is not HPEquipOpticalML opticalML
                || !MissileHelper.IsTow(opticalML);
        }
    }
}
