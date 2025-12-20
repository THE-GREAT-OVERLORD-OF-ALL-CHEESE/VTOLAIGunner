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
            CharacterVoiceLine line = voiceLines.FirstOrDefault(l => l.lineType == lineType);

            if (line == null || line.audioClips.Count == 0)
            {
                Debug.LogError($"Error: missing lines for line type: {lineType}");
            }

            return line.audioClips[Random.Range(0, line.audioClips.Count)];
        }
    }
}