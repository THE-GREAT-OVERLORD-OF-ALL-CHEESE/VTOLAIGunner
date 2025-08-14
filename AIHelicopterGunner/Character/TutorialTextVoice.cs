using CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects;

namespace CheeseMods.AIHelicopterGunner.Character
{
    public class TutorialTextVoice : IVoice
    {
        public static void SayText(string text)
        {            TutorialLabel.instance.HideLabel();
            TutorialLabel.instance.DisplayLabel($"{Main.aiGunnerName}: {text}",
            null, 5f);
        }

        public void Say(string text)
        {
            SayText(text);
        }

        public void Say(LineType type)
        {
            SayText(type.ToString());
        }

        public void SaySystemStarting(LineType type)
        {
            SayText($"Starting {type}");
        }

        public void SaySystemDestroyed(LineType type)
        {
            SayText($"Destroyed {type}");
        }
    }
}
