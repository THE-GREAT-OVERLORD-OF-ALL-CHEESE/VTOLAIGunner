using System.Collections.Generic;
using UnityEngine;

namespace CheeseMods.AIHelicopterGunner.Spotting
{
    public static class ActorClassifier
    {
        public static TargetTypes CalculateTargetType(Actor target, out TargetTypeSpecial special)
        {
            Debug.Log($"Determining target type of {target.gameObject.name}");
            if (target.parentActor != null)
            {
                target = target.parentActor;
            }
            UnitSpawn unitSpawn = target.gameObject.GetComponent<UnitSpawn>();

            if (unitSpawn == null)
            {
                Debug.Log($"{Main.aiGunnerName}: No unit spawn on this actor? ({target.gameObject.name})");
            }

            special = TargetTypeSpecial.None;
            if (target.GetComponentInChildren<MissileLauncher>() != null)
            {
                special |= TargetTypeSpecial.Missile;
            }
            if (target.GetComponentInChildren<Radar>() != null)
            {
                special |= TargetTypeSpecial.Radar;
            }

            string filteredName = target.gameObject.name.Replace("-", "").Replace("/", "").Replace("\\", "").ToLower();
            foreach (var knownTarget in knownTargets)
            {
                if (filteredName.Contains(knownTarget.name))
                {
                    Debug.Log($"Target is a known named target {knownTarget.name} ({knownTarget.type})");
                    return knownTarget.type;
                }
            }

            Debug.Log($"Target is not a known named target, inferring type...");

            if (unitSpawn is GroundUnitSpawn groundUnitSpawn)
            {
                if (groundUnitSpawn is AIJTACSpawn jtacSpawn)
                {
                    switch (jtacSpawn.soldier.soldierType)
                    {
                        case Soldier.SoldierTypes.Standard:
                            return TargetTypes.RifleSoldier;
                        case Soldier.SoldierTypes.IRMANPAD:
                            special |= TargetTypeSpecial.Missile;
                            return TargetTypes.MANPAD;
                        default:
                            return TargetTypes.Soldier;
                    }
                }
                if (groundUnitSpawn is ArtilleryUnitSpawn)
                {
                    return TargetTypes.SPG;
                }
                if (groundUnitSpawn is RocketArtilleryUnitSpawn)
                {
                    return TargetTypes.MRLS;
                }
                if (groundUnitSpawn is IFVSpawn)
                {
                    return TargetTypes.IFV;
                }
                if (groundUnitSpawn is APCUnitSpawn)
                {
                    return TargetTypes.APC;
                }
                if (groundUnitSpawn is AILockingRadarSpawn)
                {
                    special |= TargetTypeSpecial.Radar;
                    return TargetTypes.SAMRadar;
                }
                if (groundUnitSpawn is AIFixedSAMSpawn)
                {
                    special |= TargetTypeSpecial.Missile;
                    return TargetTypes.SamLauncher;
                }
                return TargetTypes.Vehicle;
            }
            if (unitSpawn is AISeaUnitSpawn seaUnitSpawn)
            {
                if (seaUnitSpawn is AICarrierSpawn)
                {
                    return TargetTypes.Carrier;
                }
                return TargetTypes.Boat;
            }
            if (unitSpawn is AIAircraftSpawn aircraftSpawn)
            {
                AIPilot pilot = target.gameObject.GetComponent<AIPilot>();
                switch (pilot.combatRole)
                {
                    case AIPilot.CombatRoles.Fighter:
                        special |= TargetTypeSpecial.Radar & TargetTypeSpecial.Missile;
                        return TargetTypes.Fighter;
                    case AIPilot.CombatRoles.FighterAttack:
                        special |= TargetTypeSpecial.Radar & TargetTypeSpecial.Missile;
                        return TargetTypes.Attack;
                    case AIPilot.CombatRoles.Attack:
                        special |= TargetTypeSpecial.Missile;
                        return TargetTypes.Attack;
                    case AIPilot.CombatRoles.Bomber:
                        return TargetTypes.Bomber;
                }
            }
            if (unitSpawn is MultiplayerSpawn playerSpawn)
            {
                special |= TargetTypeSpecial.Radar & TargetTypeSpecial.Missile;
                return TargetTypes.Fighter;
            }
            return TargetTypes.Unknown;
        }

        public static List<(string name, TargetTypes type)> knownTargets
            = new List<(string name, TargetTypes type)>
        {
            // helicopters
            ("ah94", TargetTypes.Helicopter),

            // fixedwing
            ("av42", TargetTypes.Attack),
            ("fa26", TargetTypes.Fighter),
            ("f45", TargetTypes.Fighter),
            ("t55", TargetTypes.Fighter),
            ("ef24", TargetTypes.Fighter),
            // player altnames
            ("vtol4", TargetTypes.Fighter),
            ("sevtf", TargetTypes.Fighter),

            ("asf30", TargetTypes.Fighter),
            ("asf33", TargetTypes.Fighter),
            ("asf58", TargetTypes.Fighter),
            ("gav25", TargetTypes.Attack),

            //popular mods
            ("ah6", TargetTypes.Helicopter),
            ("a10", TargetTypes.Attack),
            ("f16", TargetTypes.Fighter),
            ("f22", TargetTypes.Fighter),
        };
    }
}
