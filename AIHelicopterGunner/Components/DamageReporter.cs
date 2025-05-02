using AIHelicopterGunner.AIHelpers;
using AIHelicopterGunner.AIStates;
using AIHelicopterGunner.AIStates.MFDStates;
using AIHelicopterGunner.AIStates.Power;
using AIHelicopterGunner.AIStates.TGP;
using AIHelicopterGunner.AIStates.WM;
using AIHelicopterGunner.AttackBehaviours;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VTOLVR.DLC.Rotorcraft;
using VTOLVR.Multiplayer;

namespace AIHelicopterGunner.Components;

public class DamageReporter : MonoBehaviour
{
    private Health.DamageTypes DamageType;
    private Health.DamageEvent DamageEvent;
    private Health.DamageCreditDelegate DamageCreditDelegate;
    private DamageEngineOnRotorCollision DamageEngineOnRotorCollision;
    private RotorRPMObjectSwitch.DamageGroup RotorRPMObjectSwitch;
    private ConstantDamage ConstantDamage;
    private VehiclePart.DamageStepEvent vehicleDamageStepEvent;
    private DamageSync DamageSync;
    private RotorDamageSync RotorDamageSync;
}