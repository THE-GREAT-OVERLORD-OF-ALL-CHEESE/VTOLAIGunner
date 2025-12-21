using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VTOLVR.DLC.EW;

namespace CheeseMods.AIHelicopterGunner.Spotting
{
    public static class ActorClassifier
    {
        public static TargetTypes CalculateTargetType(Actor target, out TargetTypeSpecial special)
        {
            Debug.Log($"");
            Debug.Log($"Determining target type of {target.gameObject.name}");
            /*
            if (target.parentActor != null)
            {
                target = target.parentActor;
            }
            */
            UnitSpawn unitSpawn = target.gameObject.GetComponent<UnitSpawn>();

            if (unitSpawn == null)
            {
                Debug.Log($"{Main.aiGunnerName}: No unit spawn on this actor? ({target.gameObject.name})");
            }

            special = TargetTypeSpecial.None;
            if (target.GetComponentInChildren<MissileLauncher>() != null
                || target.GetComponentInChildren<SAMLauncher>() != null
                || target.GetComponentInChildren<IRSamLauncher>() != null)
            {
                special |= TargetTypeSpecial.Missile;
            }
            if (target.GetComponentInChildren<Radar>() != null)
            {
                special |= TargetTypeSpecial.Radar;
            }

            string filteredName = target.gameObject.name.Replace("-", "").Replace("/", "").Replace("\\", "").ToLower();
            var result = knownTargets.FirstOrDefault(n => filteredName.Contains(n.name));
            if (!string.IsNullOrEmpty(result.name))
            {
                Debug.Log($"Target is a known named target {result.name} ({result.type})");
                special |= result.special;
                return result.type;
            }

            Debug.Log($"Target is not a known named target, inferring type...");

            TargetTypes inferedType = InferTargetType(target, ref special, unitSpawn);

            Debug.Log($"Infered that this unit is a {inferedType} from its attributes");

            return inferedType;
        }

        private static TargetTypes InferTargetType(Actor target, ref TargetTypeSpecial special, UnitSpawn unitSpawn)
        {
            // if we have a parent we are a vehicle turret or something
            if (target.parentActor != null)
            {
                if (target.parentActor.unitSpawn is AIDroneCarrierSpawn && target.GetComponent<AIPilot>() != null)
                {
                    special |= TargetTypeSpecial.Radar;
                    return TargetTypes.Attack;
                }
                if (target.GetComponentInChildren<SAMLauncher>() != null)
                {
                    special |= TargetTypeSpecial.Radar | TargetTypeSpecial.Missile;
                    return TargetTypes.SamLauncher;
                }
                if (target.GetComponentInChildren<IRSamLauncher>() != null)
                {
                    special |= TargetTypeSpecial.Missile;
                    return TargetTypes.SamLauncher;
                }
                if (target.GetComponentInChildren<GunTurretAI>() != null)
                {
                    return TargetTypes.AAA;
                }
                if (target.GetComponentInChildren<Radar>() != null)
                {
                    special |= TargetTypeSpecial.Radar;
                    return TargetTypes.SAMRadar;
                }
            }

            if (unitSpawn is AIDecoyLauncherSpawn)
            {
                special |= TargetTypeSpecial.EWAR;
                return TargetTypes.Truck; // not technically correct
            }
            if (unitSpawn is AIMissileSilo)
            {
                special |= TargetTypeSpecial.Missile;
                return TargetTypes.SamLauncher; // not technically correct
            }
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
                if (groundUnitSpawn is RocketArtilleryUnitSpawn)
                {
                    return TargetTypes.MRLS;
                }
                if (groundUnitSpawn is ArtilleryUnitSpawn)
                {
                    return TargetTypes.SPG;
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
                if (groundUnitSpawn is AIGroundECMSpawn)
                {
                    special |= TargetTypeSpecial.EWAR;
                    return TargetTypes.Jammer; // not technically correct
                }
                if (groundUnitSpawn is AIGroundMWSSpawn)
                {
                    special |= TargetTypeSpecial.EWAR;
                    return TargetTypes.Truck; // not technically correct
                }
                if (groundUnitSpawn.GetComponent<SAAWUnit>() != null)
                {
                    special |= TargetTypeSpecial.Missile;
                    return TargetTypes.SPAA;
                }
                if (groundUnitSpawn.GetComponent<Soldier>() is Soldier soldier)
                {
                    switch (soldier.soldierType)
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
                // last resort
                string filteredName2 = target.gameObject.name.Replace("-", "").Replace("/", "").Replace("\\", "").ToLower();
                var result2 = lastResortGroundNames.FirstOrDefault(n => filteredName2.Contains(n.name));
                if (!string.IsNullOrEmpty(result2.name))
                {
                    Debug.Log($"Target is a known named target {result2.name} ({result2.type})");
                    special |= result2.special;
                    return result2.type;
                }
                if (target.GetComponentInChildren<SAMLauncher>() != null)
                {
                    special |= TargetTypeSpecial.Radar | TargetTypeSpecial.Missile;
                    return TargetTypes.SamLauncher;
                }
                if (target.GetComponentInChildren<IRSamLauncher>() != null)
                {
                    special |= TargetTypeSpecial.Missile;
                    return TargetTypes.SamLauncher;
                }
                if (target.GetComponentInChildren<GunTurretAI>() != null)
                {
                    return TargetTypes.AAA;
                }
                if (groundUnitSpawn.health.minDamage > 0)
                {
                    return TargetTypes.Armoured;
                }
                return TargetTypes.Vehicle;
            }
            if (unitSpawn is AISeaUnitSpawn seaUnitSpawn)
            {
                if (seaUnitSpawn is AICarrierSpawn)
                {
                    return TargetTypes.Carrier;
                }
                if (seaUnitSpawn is AIDroneCarrierSpawn)
                {
                    if (target.parentActor != null)
                    {
                        special |= TargetTypeSpecial.Radar | TargetTypeSpecial.Missile;
                        return TargetTypes.Attack;
                    }
                    return TargetTypes.Carrier;
                }
                // last resort
                if (seaUnitSpawn.GetComponentInChildren<SAMLauncher>() != null)
                {
                    return TargetTypes.Cruiser;
                }
                if (seaUnitSpawn.GetComponentInChildren<GunTurretAI>() != null)
                {
                    return TargetTypes.GunBoat;
                }
                return TargetTypes.Boat;
            }
            if (unitSpawn is AIAircraftSpawn aircraftSpawn)
            {
                if (aircraftSpawn is AIAWACSSpawn awacs)
                {
                    special |= TargetTypeSpecial.Radar;
                    return TargetTypes.AWACS;
                }
                if (aircraftSpawn is AIAirTankerSpawn tanker)
                {
                    return TargetTypes.RefuelPlane;
                }

                AIPilot pilot = target.gameObject.GetComponent<AIPilot>();
                switch (pilot.combatRole)
                {
                    case AIPilot.CombatRoles.Fighter:
                        special |= TargetTypeSpecial.Radar | TargetTypeSpecial.Missile;
                        return TargetTypes.Fighter;
                    case AIPilot.CombatRoles.FighterAttack:
                        special |= TargetTypeSpecial.Radar | TargetTypeSpecial.Missile;
                        return TargetTypes.Attack;
                    case AIPilot.CombatRoles.Attack:
                        special |= TargetTypeSpecial.Missile;
                        return TargetTypes.Attack;
                    case AIPilot.CombatRoles.Bomber:
                        return TargetTypes.Bomber;
                }
            }

            // hopefully we were able to guess the player aircraft from name earlier, if not, assume a fighter
            if (unitSpawn is MultiplayerSpawn playerSpawn)
            {
                special |= TargetTypeSpecial.Radar | TargetTypeSpecial.Missile;
                return TargetTypes.Fighter;
            }

            // last resort
            if (target.GetComponentInChildren<MissileLauncher>() != null)
            {
                return TargetTypes.SamLauncher;
            }
            if (target.GetComponentInChildren<GunTurretAI>() != null)
            {
                return TargetTypes.AAA;
            }
            string filteredName = target.gameObject.name.Replace("-", "").Replace("/", "").Replace("\\", "").ToLower();
            var result = lastResortGroundNames.FirstOrDefault(n => filteredName.Contains(n.name));
            if (!string.IsNullOrEmpty(result.name))
            {
                Debug.Log($"Target is a known named target {result.name} ({result.type})");
                special |= result.special;
                return result.type;
            }
            return TargetTypes.Unknown;
        }

        public static List<(string name, TargetTypes type, TargetTypeSpecial special)> knownTargets
            = new List<(string name, TargetTypes type, TargetTypeSpecial special)>
        {
            // helicopters
            ("ah94", TargetTypes.Helicopter, TargetTypeSpecial.None),

            // fixedwing
            ("av42", TargetTypes.Attack, TargetTypeSpecial.None),
            ("fa26", TargetTypes.Fighter, TargetTypeSpecial.None),
            ("f45", TargetTypes.Fighter, TargetTypeSpecial.None),
            ("t55", TargetTypes.Fighter, TargetTypeSpecial.None),
            ("ef24", TargetTypes.Fighter, TargetTypeSpecial.EWAR),
            // player altnames
            ("vtol4", TargetTypes.Fighter, TargetTypeSpecial.None),
            ("sevtf", TargetTypes.Fighter, TargetTypeSpecial.None),

            ("asf30", TargetTypes.Fighter, TargetTypeSpecial.None),
            ("asf33", TargetTypes.Fighter, TargetTypeSpecial.None),
            ("asf58", TargetTypes.Fighter, TargetTypeSpecial.None),
            ("gav25", TargetTypes.Attack, TargetTypeSpecial.None),

            //popular mods
            ("ah6", TargetTypes.Helicopter, TargetTypeSpecial.None),
            ("a10", TargetTypes.Attack, TargetTypeSpecial.None),
            ("f16", TargetTypes.Fighter, TargetTypeSpecial.None),
            ("f22", TargetTypes.Fighter, TargetTypeSpecial.None)
        };

        public static List<(string name, TargetTypes type, TargetTypeSpecial special)> lastResortGroundNames
            = new List<(string name, TargetTypes type, TargetTypeSpecial special)>
        {
            //assumptions based on name
            ("mbt", TargetTypes.Tank, TargetTypeSpecial.None),
            ("tank", TargetTypes.Tank, TargetTypeSpecial.None),
            ("spaa", TargetTypes.SPAA, TargetTypeSpecial.Missile),
            ("saaw", TargetTypes.SPAA, TargetTypeSpecial.Missile),
            ("cram", TargetTypes.AAA, TargetTypeSpecial.None),
            ("cwis", TargetTypes.AAA, TargetTypeSpecial.None),
            ("manpad", TargetTypes.MANPAD, TargetTypeSpecial.Missile), // TODO: there has to be a way to determine if a target is a manpad without doing this...
            ("truck", TargetTypes.Truck, TargetTypeSpecial.None),
            ("car", TargetTypes.Car, TargetTypeSpecial.None),
        };
    }
}
