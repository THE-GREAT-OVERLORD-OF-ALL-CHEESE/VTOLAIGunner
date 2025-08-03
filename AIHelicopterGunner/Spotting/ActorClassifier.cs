using UnityEngine;

namespace CheeseMods.AIHelicopterGunner.Spotting
{
    public static class ActorClassifier
    {
        public static TargetTypes CalculateTargetType(Actor target, out TargetTypeSpecial special)
        {
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
    }
}
