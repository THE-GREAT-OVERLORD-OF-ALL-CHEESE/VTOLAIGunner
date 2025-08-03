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
        Attack,
        Fighter,
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
