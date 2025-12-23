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
        ThrottleOff,
        ThrottleToIdle,
        ThrottleToFull,

        // Parts
        APU,
        Engine1,
        Engine2,
        Rotor,
        TailRotor,
        LeftWing,
        RightWing,
        Tail,

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

        // Range
        RangeClose,
        Range100,
        Range200,
        Range300,
        Range400,
        Range500,
        Range750,
        Range1000,
        Range2000,
        Range3000,
        Range5000,
        RangeFar,

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
        Bunker,

        // Missile Warning
        IRLaunch,
        RADARLaunch,
        TowLaunch,
        IRMissle,
        RadarMissile,
        TowMissile,
        Missile,
    }

    public enum LineModifier
    {
        Normal,
        Singular,
        Plural
    }
}
