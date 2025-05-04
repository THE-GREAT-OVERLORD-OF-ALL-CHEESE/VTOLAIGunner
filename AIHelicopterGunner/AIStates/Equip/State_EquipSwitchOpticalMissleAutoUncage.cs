using System;

namespace CheeseMods.AIHelicopterGunner.AIStates.Equip;

public class State_EquipSwitchOpticalMissleAutoUncage : AITryState
{
    private WeaponManager wm;
    private TargetingMFDPage tgpMfd;

    public override string Name => "Switch Optical Missile to Auto Uncage";
    public override float WarmUp => 2f;
    public override float CoolDown => 0.5f;

    private HPEquipOpticalML missileLauncher;

    public State_EquipSwitchOpticalMissleAutoUncage(WeaponManager wm, TargetingMFDPage tgpMfd)
    {
        this.wm = wm;
        this.tgpMfd = tgpMfd;
    }

    public override bool CanStart()
    {
        if (wm.currentEquip is not HPEquipOpticalML missileLauncher)
        {
            return false;
        }
        this.missileLauncher = missileLauncher;

        return !missileLauncher.autoUncage;
    }

    public override void StartState()
    {
        missileLauncher.ToggleAutoUncage();
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
