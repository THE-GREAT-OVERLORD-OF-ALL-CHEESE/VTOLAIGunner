using CheeseMods.AIHelicopterGunner.AIStates;
using CheeseMods.AIHelicopterGunner.Components;
using CheeseMods.AIHelicopterGunner.Spotting;

namespace CheeseMods.AIHelicopterGunner.AttackBehaviours;

public abstract class AttackBehaviour
{
    public abstract string Name { get; }
    public AIGunner Gunner { get; }
    public WeaponManager WeaponManager { get; }
    public State_Sequence Sequence { get; }

    public AttackBehaviour(AIGunner gunner, WeaponManager weaponManager, State_Sequence sequence)
    {
        Gunner = gunner;
        WeaponManager = weaponManager;
        Sequence = sequence;
    }

    public abstract bool CanAttackTarget(TargetMetaData target);
    public abstract bool AppropriateTarget(TargetMetaData target);
    public abstract bool CanAttackImmediately(TargetMetaData target);
    public abstract bool HaveAmmo();

    public bool CanAttackTarget(Actor actor, TargetMetaDataManager manager)
    {
        return CanAttackTarget(manager.GetMetaData(actor));
    }
    public bool AppropriateTarget(Actor actor, TargetMetaDataManager manager)
    {
        return AppropriateTarget(manager.GetMetaData(actor));
    }
    public bool CanAttackImmediately(Actor actor, TargetMetaDataManager manager)
    {
        return CanAttackImmediately(manager.GetMetaData(actor));
    }
}
