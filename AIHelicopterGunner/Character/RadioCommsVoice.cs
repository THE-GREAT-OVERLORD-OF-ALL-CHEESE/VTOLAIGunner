using CheeseMods.AIHelicopterGunner.AIHelpers;
using CheeseMods.AIHelicopterGunner.Spotting;
using CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Say(new List<LineType>() { type });
        }

        public void Say(List<LineType> lines)
        {
            Debug.Log($"{ Main.aiGunnerName}: {string.Concat(lines)}");
            CommRadioManager.instance.PlayMessageString(
                lines.Select(l => voice.GetClip(l)).ToArray(),
                true,
                false);
        }

        public void SaySystemStarting(LineType type)
        {
            Say(
                new List<LineType>
                {
                    LineType.Starting,
                    type 
                }
            );
        }

        public void SaySystemDestroyed(LineType type)
        {
            Say(
                new List<LineType>
                {
                    type,
                    LineType.Destroyed
                }
            );
        }

        public void SayReportContact(TargetMetaData targetMetaData, int count, float bearing, float range)
        {
            List<LineType> lines = new List<LineType>();

            lines.Add(LineType.Contact);

            if (count > 0)
            {
                lines.Add(LineTypeHelper.CountToLineType(count));
            }

            lines.Add(LineTypeHelper.TargetTypeToLineType(targetMetaData.TargetType));

            if (targetMetaData.SpecialFlags.HasFlag(TargetTypeSpecial.Radar)
                && targetMetaData.SpecialFlags.HasFlag(TargetTypeSpecial.Missile))
            {
                lines.Add(LineType.WithRadarMissiles);
            }
            else if (targetMetaData.SpecialFlags.HasFlag(TargetTypeSpecial.Missile))
            {
                lines.Add(LineType.WithMissiles);
            }

            lines.Add(LineTypeHelper.BearingToLineType(bearing));
            lines.Add(LineTypeHelper.RangeToLineType(range));

            Say(lines);
        }
    }
}
