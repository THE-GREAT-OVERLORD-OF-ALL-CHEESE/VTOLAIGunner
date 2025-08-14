using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CharacterVoice", menuName = "ScriptableObjects/CharacterVoice")]
    public class CharacterVoice : ScriptableObject
    {
        public string characterName;

        public List<CharacterVoiceLine> voiceLines;

        public AudioClip GetClip(LineType lineType)
        {
            return voiceLines.FirstOrDefault(l => l.lineType == lineType).audioClips.FirstOrDefault();
        }
    }
}