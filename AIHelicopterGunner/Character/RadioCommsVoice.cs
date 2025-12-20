using CheeseMods.AIHelicopterGunner.AIHelpers;
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
            LineType? special = null;
            if (targetMetaData.SpecialFlags.HasFlag(TargetTypeSpecial.Radar)
                && targetMetaData.SpecialFlags.HasFlag(TargetTypeSpecial.Missile))
            {
                special = LineType.WithRadarMissiles;
            }
            else if (targetMetaData.SpecialFlags.HasFlag(TargetTypeSpecial.Missile))
            {
                special = LineType.WithMissiles;
            }

            if (special != null)
            {
                CommRadioManager.instance.PlayMessageString(
                    new AudioClip[]
                    {
                        voice.GetClip(LineType.Contact),
                        voice.GetClip(TargetHelper.TargetTypeToLineType(targetMetaData.TargetType)),
                        voice.GetClip(special ?? LineType.Unknown),
                    },
                    true,
                    false);
            }
            else
            {
                CommRadioManager.instance.PlayMessageString(
                    new AudioClip[]
                    {
                        voice.GetClip(LineType.Contact),
                        voice.GetClip(TargetHelper.TargetTypeToLineType(targetMetaData.TargetType)),
                    },
                    true,
                    false);

            }
        }
    }
}
