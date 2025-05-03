using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIHelicopterGunner.Components
{
    public class DamageReporter : MonoBehaviour
    {
        private List<HealthDamageTracker> healthDamageTrackers = new List<HealthDamageTracker>();

        private void Awake()
        {
            healthDamageTrackers = GetComponentsInChildren<Health>(true)
                .Select(h => new HealthDamageTracker(h)).ToList();
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

        public HealthDamageTracker(Health health)
        {
            this.health = health;
        }

        public void CheckDamage()
        {
            if (reportedDamage)
                return;

            if (health.isDead)
            {
                string name = health.gameObject.name;

                // Use friendly name if available
                if (friendlyNames.TryGetValue(name, out var friendlyName))
                {
                    name = friendlyName;
                }

                Debug.Log($"{name} has bit the dust uwu");

                TutorialLabel.instance.HideLabel();
                TutorialLabel.instance.DisplayLabel(
                    $"{Main.aiGunnerName}: {name} is destroyed",
                    null,
                    5f
                );

                reportedDamage = true;
            }
        }
    }
}
