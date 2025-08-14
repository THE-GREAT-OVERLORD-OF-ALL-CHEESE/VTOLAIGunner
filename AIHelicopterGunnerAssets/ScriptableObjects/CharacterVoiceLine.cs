using System.Collections.Generic;
using UnityEngine;

namespace CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects
{
    public enum LineType
    {
        // Misc
        Roger,
        GoodJob,
        Thanks,

        // Systems
        Starting,
        Started,
        Destroyed,
        APU,
        Engine1,
        Engine2,
        ThrottleOff,
        ThrottleToIdle,
        ThrottleToFull,

        // Combat
        Rifle,
        GunsGunsGuns,

        //Report
        Hit,
        Splash,
        Trashed
    }

    [CreateAssetMenu(fileName = "CharacterVoiceLine", menuName = "ScriptableObjects/CharacterVoiceLine")]
    public class CharacterVoiceLine : ScriptableObject
    {
        public LineType lineType;
        public List<AudioClip> audioClips;
    }
}
