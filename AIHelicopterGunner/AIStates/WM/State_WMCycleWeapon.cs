using System;

namespace CheeseMods.AIHelicopterGunner.AIStates.WM;

public class State_WMCycleWeapon : AITryState
{
    protected WeaponManager wm;
    protected Type weaponType;

    public override string Name => $"Cycle to {weaponType}";
    public override float WarmUp => 0.2f;
    public override float CoolDown => 0.3f;

    public State_WMCycleWeapon(WeaponManager wm, Type weaponType)
    {
        this.wm = wm;
        this.weaponType = weaponType;
    }

    public override bool CanStart()
    {
        HPEquippable equip = wm.GetEquip(wm.weaponIdx);
        return equip.GetType() != weaponType || equip.GetCount() <= 0;
    }

    public override void StartState()
    {
        wm.CycleActiveWeapons();
    }

    public override void UpdateState()
    {
        throw new NotImplementedException();
    }

    public override bool IsOver()
    {
        return true;
    }

    public override void EndState()
    {

    }
}
