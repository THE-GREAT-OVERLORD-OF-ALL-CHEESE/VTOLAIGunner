namespace CheeseMods.AIHelicopterGunner.Spotting
{
    public enum TargetTypes
    {
        Target,
        Unknown,

        Soldier,
        RifleSoldier,
        MANPAD,

        Vehicle,
        Car,
        Truck,
        Armoured,
        Tank,
        APC,
        IFV,
        SPAA,
        AAA,
        SPG,
        MRLS,
        SAM,
        SAMRadar,
        SamLauncher,
        Jammer,

        Boat,
        GunBoat,

        Ship,
        Cruiser,
        Carrier,
        LHA,

        Aircraft,
        Helicopter,
            AH94,
            AH6,
            AH64,
        Plane,
        Attack,
            AV42,
            GAV25,
            AV44,
            A10,
        Fighter,
            FA26B,
            F45,
            T55,
            EF24G,
            ASF30,
            ASF33,
            ASF58,
            F16,
            F18,
            F22,
        Bomber,
        AWACS,
        RefuelPlane,

        Structure,
        Tent,
        Bunker
    }

    public enum TargetTypeSpecial
    {
        None = 0,
        EWAR = 1 << 0,
        Radar = 1 << 1,
        Missile = 1 << 2
    }
}
