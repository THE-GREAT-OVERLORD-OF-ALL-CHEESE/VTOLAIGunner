namespace CheeseMods.AIHelicopterGunner.Character
{
    public class TutorialTextVoice : IVoice
    {
        public void Say(string text)
        {            TutorialLabel.instance.HideLabel();
            TutorialLabel.instance.DisplayLabel($"{Main.aiGunnerName}: {text}",
            null, 5f);
        }
    }
}
