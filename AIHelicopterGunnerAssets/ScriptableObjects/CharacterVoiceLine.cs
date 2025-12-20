using System.Collections.Generic;
using UnityEngine;

namespace CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CharacterVoiceLine", menuName = "ScriptableObjects/CharacterVoiceLine")]
    public class CharacterVoiceLine : ScriptableObject
    {
        public LineType lineType;
        public LineModifier modifier;
        public List<AudioClip> audioClips;
    }
}
