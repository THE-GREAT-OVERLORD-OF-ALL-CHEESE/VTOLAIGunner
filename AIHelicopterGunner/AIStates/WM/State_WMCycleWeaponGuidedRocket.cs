using CheeseMods.AIHelicopterGunner.AIHelpers;

namespace CheeseMods.AIHelicopterGunner.AIStates.WM;

public class State_WMCycleWeaponGuidedRocket : State_WMCycleWeapon
{
    public override string Name => $"Cycle to {weaponType} (Guided Rocket)";

    public State_WMCycleWeaponGuidedRocket(WeaponManager wm) : base(wm, typeof(HPEquipOpticalML))
    {

    }

    public override bool CanStart()
    {
        HPEquippable equip = wm.currentEquip;
        return equip.GetCount() <= 0
            || equip is not HPEquipOpticalML opticalML
            || !MissileHelper.IsGuidedRocketLauncher(opticalML);
    }
}
