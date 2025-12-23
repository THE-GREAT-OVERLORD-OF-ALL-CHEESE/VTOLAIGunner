using System.Collections.Generic;

namespace CheeseMods.AIHelicopterGunner.Spotting
{
    public enum ClassificationType
    {
        Synonymous,
        NotSynonymous,
        SpecificName
    }

    public class TargetClassification
    {
        public TargetTypes targetType;
        public ClassificationType classificationType;

        public TargetClassification parentClassification;

        public List<TargetClassification> childClasifications;

        public TargetClassification(TargetTypes targetType, ClassificationType classificationType, List<TargetClassification> childClasifications)
        {
            this.targetType = targetType;
            this.classificationType = classificationType;
            this.childClasifications = childClasifications;

            foreach (TargetClassification classification in childClasifications)
            {
                classification.parentClassification = this;
            }
        }

        public TargetClassification(TargetTypes targetType, ClassificationType classificationType)
        {
            this.targetType = targetType;
            this.classificationType = classificationType;
        }

        public static TargetClassification Classifications { get; }
            = new TargetClassification(TargetTypes.Target,
                ClassificationType.Synonymous,
                new List<TargetClassification>
                {
                    new TargetClassification(TargetTypes.Unknown,
                        ClassificationType.Synonymous),
                    new TargetClassification(TargetTypes.Soldier,
                        ClassificationType.Synonymous,
                        new List<TargetClassification>
                        {
                            new TargetClassification(TargetTypes.RifleSoldier,
                                ClassificationType.Synonymous),
                            new TargetClassification(TargetTypes.MANPAD,
                                ClassificationType.Synonymous)
                        }
                    ),
                    new TargetClassification(TargetTypes.Vehicle,
                        ClassificationType.Synonymous,
                        new List<TargetClassification>
                        {
                            new TargetClassification(TargetTypes.Car,
                                ClassificationType.Synonymous),
                            new TargetClassification(TargetTypes.Truck,
                                ClassificationType.Synonymous),
                            new TargetClassification(TargetTypes.AAA,
                                ClassificationType.Synonymous),
                            new TargetClassification(TargetTypes.Armoured,
                                ClassificationType.Synonymous,
                                new List<TargetClassification>
                                {
                                    new TargetClassification(TargetTypes.Tank,
                                ClassificationType.Synonymous),
                                    new TargetClassification(TargetTypes.APC,
                                ClassificationType.Synonymous),
                                    new TargetClassification(TargetTypes.SPAA,
                                ClassificationType.Synonymous),
                                    new TargetClassification(TargetTypes.SPG,
                                ClassificationType.Synonymous),
                                }
                            ),
                            new TargetClassification(TargetTypes.SAM,
                                ClassificationType.Synonymous,
                                new List<TargetClassification>
                                {
                                    new TargetClassification(TargetTypes.SAMRadar,
                                        ClassificationType.Synonymous),
                                    new TargetClassification(TargetTypes.SamLauncher,
                                        ClassificationType.Synonymous),
                                }
                            ),
                        }
                    ),
                    new TargetClassification(TargetTypes.Boat,
                                ClassificationType.Synonymous,
                        new List<TargetClassification>
                        {
                            new TargetClassification(TargetTypes.GunBoat,
                                ClassificationType.Synonymous)
                        }
                    ),
                    new TargetClassification(TargetTypes.Ship,
                        ClassificationType.Synonymous,
                        new List<TargetClassification>
                        {
                            new TargetClassification(TargetTypes.Cruiser,
                                ClassificationType.Synonymous),
                            new TargetClassification(TargetTypes.Carrier,
                                ClassificationType.Synonymous),
                            new TargetClassification(TargetTypes.LHA,
                                ClassificationType.Synonymous)
                        }
                    ),
                    new TargetClassification(TargetTypes.Aircraft,
                        ClassificationType.Synonymous,
                        new List<TargetClassification>
                        {
                            new TargetClassification(TargetTypes.Helicopter,
                                ClassificationType.Synonymous,
                                new List<TargetClassification>
                                {
                                    new TargetClassification(TargetTypes.AH64,
                                        ClassificationType.SpecificName),
                                    new TargetClassification(TargetTypes.AH6,
                                        ClassificationType.SpecificName),
                                    new TargetClassification(TargetTypes.AH64,
                                        ClassificationType.SpecificName)
                                }
                            ),
                            new TargetClassification(TargetTypes.Plane,
                                ClassificationType.Synonymous,
                                new List<TargetClassification>
                                {
                                    new TargetClassification(TargetTypes.Attack,
                                        ClassificationType.Synonymous,
                                        new List<TargetClassification>
                                        {
                                            new TargetClassification(TargetTypes.AV42,
                                                ClassificationType.SpecificName),
                                            new TargetClassification(TargetTypes.GAV25,
                                                ClassificationType.SpecificName),
                                            new TargetClassification(TargetTypes.AV44,
                                                ClassificationType.SpecificName),
                                            new TargetClassification(TargetTypes.A10,
                                                ClassificationType.SpecificName)
                                        }
                                    ),
                                    new TargetClassification(TargetTypes.Fighter,
                                        ClassificationType.Synonymous,
                                        new List<TargetClassification>
                                        {
                                            new TargetClassification(TargetTypes.FA26B,
                                                ClassificationType.SpecificName),
                                            new TargetClassification(TargetTypes.F45,
                                                ClassificationType.SpecificName),
                                            new TargetClassification(TargetTypes.T55,
                                                ClassificationType.SpecificName),
                                            new TargetClassification(TargetTypes.EF24G,
                                                ClassificationType.SpecificName),
                                            new TargetClassification(TargetTypes.ASF30,
                                                ClassificationType.SpecificName),
                                            new TargetClassification(TargetTypes.ASF33,
                                                ClassificationType.SpecificName),
                                            new TargetClassification(TargetTypes.ASF58,
                                                ClassificationType.SpecificName),
                                            new TargetClassification(TargetTypes.F16,
                                                ClassificationType.SpecificName),
                                            new TargetClassification(TargetTypes.F18,
                                                ClassificationType.SpecificName),
                                            new TargetClassification(TargetTypes.F22,
                                                ClassificationType.SpecificName)
                                        }
                                    ),
                                    new TargetClassification(TargetTypes.Bomber,
                                        ClassificationType.Synonymous),
                                    new TargetClassification(TargetTypes.AWACS,
                                        ClassificationType.Synonymous),
                                    new TargetClassification(TargetTypes.RefuelPlane,
                                        ClassificationType.Synonymous)
                                }
                            )
                        }
                    ),
                    new TargetClassification(TargetTypes.Structure,
                        ClassificationType.Synonymous,
                        new List<TargetClassification>
                        {
                            new TargetClassification(TargetTypes.Tent,
                                ClassificationType.Synonymous),
                            new TargetClassification(TargetTypes.Bunker,
                                ClassificationType.Synonymous)
                        }
                    ),
                }
            );
    }
}
