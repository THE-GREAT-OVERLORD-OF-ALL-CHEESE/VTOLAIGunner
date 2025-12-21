using CheeseMods.AIHelicopterGunner.Spotting;
using CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

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

        public static List<Tuple<float, LineType>> bearings = new List<Tuple<float, LineType>>
        {
            new Tuple<float, LineType>((0 / 12f) * 360f, LineType.Clock12),
            new Tuple<float, LineType>((1 / 12f) * 360f, LineType.Clock1),
            new Tuple<float, LineType>((2 / 12f) * 360f, LineType.Clock2),
            new Tuple<float, LineType>((3 / 12f) * 360f, LineType.Clock3),
            new Tuple<float, LineType>((4 / 12f) * 360f, LineType.Clock5),
            new Tuple<float, LineType>((5 / 12f) * 360f, LineType.Clock5),
            new Tuple<float, LineType>((6 / 12f) * 360f, LineType.Clock6),
            new Tuple<float, LineType>((7 / 12f) * 360f, LineType.Clock7),
            new Tuple<float, LineType>((8 / 12f) * 360f, LineType.Clock8),
            new Tuple<float, LineType>((9 / 12f) * 360f, LineType.Clock9),
            new Tuple<float, LineType>((10 / 12f) * 360f, LineType.Clock10),
            new Tuple<float, LineType>((11 / 12f) * 360f, LineType.Clock11),
        };
        public static LineType BearingToLineType(float bearing)
        {
            if (bearing < 0)
                bearing += 360f;

            LineType closesLineType = LineType.Unknown;
            float closestDistance = float.MaxValue;
            foreach (Tuple<float, LineType> bearingLine in bearings)
            {
                float currentDistance = Mathf.Abs(bearingLine.Item1 - bearing);
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closesLineType = bearingLine.Item2;
                }
            }

            return closesLineType;
        }

        public static List<Tuple<float, LineType>> ranges = new List<Tuple<float, LineType>>
        {
            new Tuple<float, LineType>(0, LineType.RangeClose),
            new Tuple<float, LineType>(100, LineType.Range100),
            new Tuple<float, LineType>(200, LineType.Range200),
            new Tuple<float, LineType>(300, LineType.Range300),
            new Tuple<float, LineType>(400, LineType.Range400),
            new Tuple<float, LineType>(500, LineType.Range500),
            new Tuple<float, LineType>(750, LineType.Range750),
            new Tuple<float, LineType>(1000, LineType.Range1000),
            new Tuple<float, LineType>(2000, LineType.Range2000),
            new Tuple<float, LineType>(3000, LineType.Range3000),
            new Tuple<float, LineType>(5000, LineType.Range5000),
            new Tuple<float, LineType>(7000, LineType.RangeFar),
        };
        public static LineType RangeToLineType(float range)
        {
            LineType closesLineType = LineType.Unknown;
            float closestDistance = float.MaxValue;
            foreach (Tuple<float, LineType> rangeLine in ranges)
            {
                float currentDistance = Mathf.Abs(rangeLine.Item1 - range);
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closesLineType = rangeLine.Item2;
                }
            }

            return closesLineType;
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
