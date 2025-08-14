using CheeseMods.AIHelicopterGunner.Character;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CheeseMods.AIHelicopterGunner.Components
{
    public class DamageReporter : MonoBehaviour
    {
        public IVoice voice; 
        private List<HealthDamageTracker> healthDamageTrackers = new List<HealthDamageTracker>();

        private void Start()
        {
            healthDamageTrackers = GetComponentsInChildren<Health>(true)
                .Select(h => new HealthDamageTracker(h, voice)).ToList();
        }

        private void Update()
        {
            foreach (HealthDamageTracker tracker in healthDamageTrackers)
            {
                tracker.CheckDamage();
            }
        }
    }

    public class HealthDamageTracker
    {
        public Health health;
        public IVoice voice;
        public bool reportedDamage;

        private static readonly Dictionary<string, string> friendlyNames = new()
        {
            { "TurbineEngine 1", "Engine 1" },
            { "TurbineEngine 2", "Engine 2" },
            { "apu", "APU" },
            { "RightWingPart", "Right Wing" },
            { "LeftWingPart", "Left Wing" },
            { "TailRotorPart", "Tail Rotor" },
            { "TailPart", "Tail" },
            { "MainRotorPart", "Main Rotor" }
        };

        public HealthDamageTracker(Health health, IVoice voice)
        {
            this.health = health;
            this.voice = voice;
        }

        public void CheckDamage()
        {
            if (reportedDamage != health.isDead)
            {
                reportedDamage = health.isDead;

                if (health.isDead)
                {
                    string name = health.gameObject.name;

                    // Use friendly name if available
                    if (friendlyNames.TryGetValue(name, out string friendlyName))
                    {
                        name = friendlyName;
                    }

                    switch (name)
                    {
                        case "Engine 1":
                            voice.SaySystemDestroyed(AIHelicopterGunnerAssets.ScriptableObjects.LineType.Engine1);
                            break;
                        case "Engine 2":
                            voice.SaySystemDestroyed(AIHelicopterGunnerAssets.ScriptableObjects.LineType.Engine2);
                            break;
                        case "APU":
                            voice.SaySystemDestroyed(AIHelicopterGunnerAssets.ScriptableObjects.LineType.APU);
                            break;
                        default:
                            voice.Say($"{name} is destroyed");
                            break;
                    }
                }
            }
        }
    }
}
