using CheeseMods.AIHelicopterGunner.AIHelpers;

namespace CheeseMods.AIHelicopterGunner.AIStates.WM;

public class State_WMCycleWeaponOpticalMissileFaf : State_WMCycleWeapon
{
    public override string Name => $"Cycle to {weaponType} (FaF)";

    public State_WMCycleWeaponOpticalMissileFaf(WeaponManager wm) : base(wm, typeof(HPEquipOpticalML))
    {

    }

    public override bool CanStart()
    {
        HPEquippable equip = wm.currentEquip;
        return equip.GetCount() <= 0
            || equip is not HPEquipOpticalML opticalML
            || !MissileHelper.IsOpticalFaf(opticalML);
    }
}
