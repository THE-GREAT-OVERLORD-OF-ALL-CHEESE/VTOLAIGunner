namespace CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects
{
    public enum LineType
    {
        // Misc
        Roger,
        GoodJob,
        Thanks,

        // Systems
        Starting,
        Started,
        Destroyed,
        //On,
        //Off,
        APU,
        Engine1,
        Engine2,
        ThrottleOff,
        ThrottleToIdle,
        ThrottleToFull,

        // Combat
        Rifle,
        GunsGunsGuns,

        //Report
        Hit,
        Splash,
        Trashed,

        //Control
        IHaveTGP,
        IHaveWeapons,

        //Command
        Pilot,
        EngageWith,

        //Weapons
        Guns,
        Rockets,
        GuidedRockets,
        AGTMs,
        TOWs,
        Stingers,

        // Contact
        Contact,
        WithRadarMissiles,
        WithMissiles,

        // Count
        Count1,
        Count2,
        Count3,
        Count4,
        Count5,
        CountMany,

        // Clock
        Clock1,
        Clock2,
        Clock3,
        Clock4,
        Clock5,
        Clock6,
        Clock7,
        Clock8,
        Clock9,
        Clock10,
        Clock11,
        Clock12,

        // Targets
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

    public enum LineModifier
    {
        Normal,
        Singular,
        Plural
    }
}
