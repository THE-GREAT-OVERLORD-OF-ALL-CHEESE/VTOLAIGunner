using UnityEngine;
using System.Collections.Generic;
using VTOLVR;

namespace AIHelicopterGunner
{
    public class DamageReporter : MonoBehaviour
    {
        private enum HealthLevel
        {
            Healthy,
            Minor,
            Moderate,
            Critical,
            Destroyed
        }

        private class DamageState
        {
            public HealthLevel lastLevel = HealthLevel.Healthy;
        }

        // These are the names of the child GameObjects under "Components" that have Health scripts
        private readonly string[] partNames = new string[]
        {
            "APU",
            "MainRotorPart",
            "LeftWingPart",
            "RightWingPart",
            "TailPart",
            "TurbineEngine1",
            "TurbineEngine2"
        };

        private Dictionary<string, Health> partHealths = new();
        private Dictionary<string, DamageState> partStates = new();

        void Start()
        {
            foreach (string partName in partNames)
            {
                // Find the GameObject by name inside "Components"
                Transform partTransform = transform.Find($"Components/{partName}");
                if (partTransform)
                {
                    Health health = partTransform.GetComponent<Health>();
                    if (health)
                    {
                        partHealths[partName] = health;
                        partStates[partName] = new DamageState();
                    }
                    else
                    {
                        Debug.LogWarning($"[DamageReporter] Health script missing on {partName}");
                    }
                }
                else
                {
                    Debug.LogWarning($"[DamageReporter] Could not find Components/{partName}");
                }
            }
        }

        void Update()
        {
            foreach (var kvp in partHealths)
            {
                string partName = kvp.Key;
                Health health = kvp.Value;
                float hp = health.normalizedHealth;
                DamageState state = partStates[partName];

                HealthLevel newLevel = GetHealthLevel(hp);
                if (newLevel != state.lastLevel)
                {
                    state.lastLevel = newLevel;

                    string message = newLevel switch
                    {
                        HealthLevel.Minor => $"{Main.aiGunnerName}: {partName} has minor damage.",
                        HealthLevel.Moderate => $"{Main.aiGunnerName}: {partName} moderately damaged.",
                        HealthLevel.Critical => $"{Main.aiGunnerName}: {partName} critically damaged!",
                        HealthLevel.Destroyed => $"{Main.aiGunnerName}: {partName} is destroyed!",
                        _ => null
                    };

                    if (!string.IsNullOrEmpty(message))
                    {
                        TutorialLabel.instance.HideLabel();
                        TutorialLabel.instance.DisplayLabel(message, null, 5f);
                        Debug.Log(message);
                    }
                }
            }
        }

        private HealthLevel GetHealthLevel(float hp)
        {
            if (hp <= 0f) return HealthLevel.Destroyed;
            if (hp <= 0.25f) return HealthLevel.Critical;
            if (hp <= 0.5f) return HealthLevel.Moderate;
            if (hp <= 0.75f) return HealthLevel.Minor;
            return HealthLevel.Healthy;
        }
    }
}
