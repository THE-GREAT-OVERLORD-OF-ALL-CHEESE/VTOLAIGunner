using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CheeseMods.AIHelicopterGunner.Components
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
                tracker.CheckRepair();
            }
        }
    }

    public class HealthDamageTracker(Health health)
    {
        private bool reportedDamage;

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

        public void CheckDamage()
        {
            if (reportedDamage)
                return;

            if (health.isDead)
            {
                string name = health.gameObject.name;

                // Use friendly name if available
                if (friendlyNames.TryGetValue(name, out string friendlyName))
                {
                    name = friendlyName;
                }

                Debug.Log($"{name} has bit the dust");

                TutorialLabel.instance.HideLabel();
                TutorialLabel.instance.DisplayLabel(
                    $"{Main.aiGunnerName}: {name} is destroyed",
                    null,
                    5f
                );

                reportedDamage = true;
            } 
        }
        public void CheckRepair()
        {
            if (!health.isDead)
            {
                string name = health.gameObject.name;

                //use easy to read name
                if (friendlyNames.TryGetValue(name, out string friendlyName))
                {
                    name = friendlyName;
                }

                Debug.Log($"{name} has been the repaired");

                reportedDamage = false;
            }
        }
    }
}
