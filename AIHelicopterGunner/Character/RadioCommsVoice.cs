using CheeseMods.AIHelicopterGunner.AIHelpers;
using CheeseMods.AIHelicopterGunner.Spotting;
using CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects;
using System.Collections.Generic;
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

        public void SayReportContact(TargetMetaData targetMetaData, int count)
        {
            List<AudioClip> clips = new List<AudioClip>();

            clips.Add(voice.GetClip(LineType.Contact));

            if (count > 0)
            {
                clips.Add(voice.GetClip(LineTypeHelper.CountToLineType(count)));
            }

            clips.Add(voice.GetClip(LineTypeHelper.TargetTypeToLineType(targetMetaData.TargetType)));

            if (targetMetaData.SpecialFlags.HasFlag(TargetTypeSpecial.Radar)
                && targetMetaData.SpecialFlags.HasFlag(TargetTypeSpecial.Missile))
            {
                clips.Add(voice.GetClip(LineType.WithRadarMissiles));
            }
            else if (targetMetaData.SpecialFlags.HasFlag(TargetTypeSpecial.Missile))
            {
                clips.Add(voice.GetClip(LineType.WithMissiles));
            }

            CommRadioManager.instance.PlayMessageString(
                    clips.ToArray(),
                    true,
                    false);
        }
    }
}
