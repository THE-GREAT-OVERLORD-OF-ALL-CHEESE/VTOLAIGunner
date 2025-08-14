using CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects;

namespace CheeseMods.AIHelicopterGunner.Character
{
    public interface IVoice
    {
        public void Say(string text);
        public void Say(LineType type);
        public void SaySystemStarting(LineType type);
        public void SaySystemDestroyed(LineType type);
    }
}
