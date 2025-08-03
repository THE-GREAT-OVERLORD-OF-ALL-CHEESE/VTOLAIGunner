using System.Collections.Generic;

namespace CheeseMods.AIHelicopterGunner.Spotting
{
    public class TargetClassification
    {
        public TargetTypes targetType;

        public TargetClassification parentClassification;

        public List<TargetClassification> childClasifications;

        public TargetClassification(TargetTypes targetType, List<TargetClassification> childClasifications)
        {
            this.targetType = targetType;
            this.childClasifications = childClasifications;

            foreach (TargetClassification classification in childClasifications)
            {
                classification.parentClassification = this;
            }
        }

        public TargetClassification(TargetTypes targetType)
        {
            this.targetType = targetType;

            foreach (TargetClassification classification in childClasifications)
            {
                classification.parentClassification = this;
            }
        }

        public static TargetClassification Classifications { get; }
            = new TargetClassification(TargetTypes.Target,
                new List<TargetClassification>
                {
                    new TargetClassification(TargetTypes.Unknown),
                    new TargetClassification(TargetTypes.Soldier,
                        new List<TargetClassification>
                        {
                            new TargetClassification(TargetTypes.RifleSoldier),
                            new TargetClassification(TargetTypes.MANPAD)
                        }
                    ),
                    new TargetClassification(TargetTypes.Vehicle,
                        new List<TargetClassification>
                        {
                            new TargetClassification(TargetTypes.Car),
                            new TargetClassification(TargetTypes.Truck),
                            new TargetClassification(TargetTypes.AAA),
                            new TargetClassification(TargetTypes.Armoured,
                                new List<TargetClassification>
                                {
                                    new TargetClassification(TargetTypes.Tank),
                                    new TargetClassification(TargetTypes.APC),
                                    new TargetClassification(TargetTypes.SPAA),
                                    new TargetClassification(TargetTypes.SPG),
                                }
                            ),
                            new TargetClassification(TargetTypes.SAM,
                                new List<TargetClassification>
                                {
                                    new TargetClassification(TargetTypes.SAMRadar),
                                    new TargetClassification(TargetTypes.SamLauncher),
                                }
                            ),
                        }
                    ),
                    new TargetClassification(TargetTypes.Boat,
                        new List<TargetClassification>
                        {
                            new TargetClassification(TargetTypes.GunBoat)
                        }
                    ),
                    new TargetClassification(TargetTypes.Ship,
                        new List<TargetClassification>
                        {
                            new TargetClassification(TargetTypes.Cruiser),
                            new TargetClassification(TargetTypes.Carrier),
                            new TargetClassification(TargetTypes.LHA)
                        }
                    ),
                    new TargetClassification(TargetTypes.Aircraft,
                        new List<TargetClassification>
                        {
                            new TargetClassification(TargetTypes.Helicopter),
                            new TargetClassification(TargetTypes.Attack),
                            new TargetClassification(TargetTypes.Fighter),
                            new TargetClassification(TargetTypes.Bomber),
                            new TargetClassification(TargetTypes.AWACS),
                            new TargetClassification(TargetTypes.RefuelPlane)
                        }
                    ),
                    new TargetClassification(TargetTypes.Structure,
                        new List<TargetClassification>
                        {
                            new TargetClassification(TargetTypes.Tent),
                            new TargetClassification(TargetTypes.Bunker)
                        }
                    ),
                }
            );
    }
}
