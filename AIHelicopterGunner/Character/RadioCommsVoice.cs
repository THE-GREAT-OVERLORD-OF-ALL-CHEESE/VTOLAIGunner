using CheeseMods.AIHelicopterGunner.Spotting;
using CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects;
using UnityEngine;

namespace CheeseMods.AIHelicopterGunner.Character
{
    public class RadioCommsVoice : IVoice
    {
        private CharacterVoice voice;

        public RadioCommsVoice(CharacterVoice voice)
        {
            this.voice = voice;
        }

        public void Say(string text)
        {
            TutorialTextVoice.SayText(text);
        }

        public void Say(LineType type)
        {
            CommRadioManager.instance.PlayMessage(
                voice.GetClip(type),
                true,
                false);
        }

        public void SaySystemStarting(LineType type)
        {
            CommRadioManager.instance.PlayMessageString(
                new AudioClip[]
                {
                    voice.GetClip(LineType.Starting),
                    voice.GetClip(type)
                },
                true,
                false);
        }

        public void SaySystemDestroyed(LineType type)
        {
            CommRadioManager.instance.PlayMessageString(
                new AudioClip[]
                {
                    voice.GetClip(type),
                    voice.GetClip(LineType.Destroyed)
                },
                true,
                false);
        }

        public void SayReportContact(TargetMetaData targetMetaData)
        {
            string thingToSay = $"Contact {targetMetaData.TargetType}";
            if (targetMetaData.SpecialFlags.HasFlag(TargetTypeSpecial.Radar)
                && targetMetaData.SpecialFlags.HasFlag(TargetTypeSpecial.Missile))
            {
                thingToSay += $" with radar missiles";
            }
            else if (targetMetaData.SpecialFlags.HasFlag(TargetTypeSpecial.Missile))
            {
                thingToSay += $" with missiles";
            }
            Say(thingToSay);
        }
    }
}
