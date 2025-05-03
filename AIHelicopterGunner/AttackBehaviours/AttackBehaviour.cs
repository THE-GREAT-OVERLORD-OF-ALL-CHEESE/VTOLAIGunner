using CheeseMods.AIHelicopterGunner.AIStates;
using CheeseMods.AIHelicopterGunner.Components;

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

    public abstract bool CanAttackTarget(Actor actor);
    public abstract bool AppropriateTarget(Actor actor);
    public abstract bool CanAttackImmediately(Actor actor);
    public abstract bool HaveAmmo();
}
