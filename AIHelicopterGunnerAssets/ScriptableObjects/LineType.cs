namespace CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects
{
    public enum LineType
    {
        //PlaceHolder,

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
        //IHaveTGP,
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
        Stingers
    }

    public enum LineModifier
    {
        Normal,
        Singular,
        Plural
    }
}
