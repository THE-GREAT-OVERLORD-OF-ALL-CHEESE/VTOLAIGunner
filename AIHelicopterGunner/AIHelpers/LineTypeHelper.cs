using CheeseMods.AIHelicopterGunner.Spotting;
using CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects;

namespace CheeseMods.AIHelicopterGunner.AIHelpers
{
    public static class LineTypeHelper
    {
        public static LineType CountToLineType(int count)
        {
            switch (count)
            {
                case 0:
                    break;
                case 1:
                    return LineType.Count1;
                case 2:
                    return LineType.Count2;
                case 3:
                    return LineType.Count3;
                case 4:
                    return LineType.Count4;
                case 5:
                    return LineType.Count5;
            }
            return LineType.CountMany;
        }

        public static LineType TargetTypeToLineType(TargetTypes type)
        {
            switch (type)
            {
                case TargetTypes.Target:
                    return LineType.Target;
                case TargetTypes.Unknown:
                    return LineType.Unknown;
                case TargetTypes.Soldier:
                    return LineType.Soldier;
                case TargetTypes.RifleSoldier:
                    return LineType.RifleSoldier;
                case TargetTypes.MANPAD:
                    return LineType.MANPAD;
                case TargetTypes.Vehicle:
                    return LineType.Vehicle;
                case TargetTypes.Car:
                    return LineType.Car;
                case TargetTypes.Truck:
                    return LineType.Truck;
                case TargetTypes.Armoured:
                    return LineType.Armoured;
                case TargetTypes.Tank:
                    return LineType.Tank;
                case TargetTypes.APC:
                    return LineType.APC;
                case TargetTypes.IFV:
                    return LineType.IFV;
                case TargetTypes.SPAA:
                    return LineType.SPAA;
                case TargetTypes.AAA:
                    return LineType.AAA;
                case TargetTypes.SPG:
                    return LineType.SPG;
                case TargetTypes.MRLS:
                    return LineType.MRLS;
                case TargetTypes.SAM:
                    return LineType.SAM;
                case TargetTypes.SAMRadar:
                    return LineType.SAMRadar;
                case TargetTypes.SamLauncher:
                    return LineType.SamLauncher;
                case TargetTypes.Jammer:
                    return LineType.Jammer;
                case TargetTypes.Boat:
                    return LineType.Boat;
                case TargetTypes.GunBoat:
                    return LineType.GunBoat;
                case TargetTypes.Ship:
                    return LineType.Ship;
                case TargetTypes.Cruiser:
                    return LineType.Cruiser;
                case TargetTypes.Carrier:
                    return LineType.Carrier;
                case TargetTypes.LHA:
                    return LineType.LHA;
                case TargetTypes.Aircraft:
                    return LineType.Aircraft;
                case TargetTypes.Helicopter:
                    return LineType.Helicopter;
                case TargetTypes.Attack:
                    return LineType.Attack;
                case TargetTypes.Fighter:
                    return LineType.Fighter;
                case TargetTypes.Bomber:
                    return LineType.Bomber;
                case TargetTypes.AWACS:
                    return LineType.AWACS;
                case TargetTypes.RefuelPlane:
                    return LineType.RefuelPlane;
                case TargetTypes.Structure:
                    return LineType.Structure;
                case TargetTypes.Tent:
                    return LineType.Tent;
                case TargetTypes.Bunker:
                    return LineType.Bunker;
            }
            return LineType.Unknown;
        }
    }
}
