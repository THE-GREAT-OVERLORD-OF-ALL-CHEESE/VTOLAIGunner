using CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects;

namespace CheeseMods.AIHelicopterGunner.Character
{
    public static class SharedCallouts
    {
        public static Callout gunsCallout;
        public static Callout rifleCallout;

        public static void Initialise(IVoice voice)
        {
            gunsCallout = new Callout(10f, 20f, 3, () => voice.Say(LineType.GunsGunsGuns));
            rifleCallout = new Callout(5f, 10f, 2, () => voice.Say(LineType.Rifle));
        }
    }
}
