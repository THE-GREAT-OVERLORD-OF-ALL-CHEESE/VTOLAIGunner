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
            healthDamageTrackers = GetComponentsInChildren<Health>(true).Select(h => new HealthDamageTracker(h)).ToList();
        }

        private void Update()
        {
            foreach (HealthDamageTracker healthDamageTracker in healthDamageTrackers)
            {
                healthDamageTracker.CheckDamage();
            }
        }
    }

    public class HealthDamageTracker
    {
        public Health health;
        public bool reportedDamage;
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
                Debug.Log($"{health.gameObject.name} has bit the dust");
                TutorialLabel.instance.HideLabel();
                TutorialLabel.instance.DisplayLabel(
                    $"{Main.aiGunnerName}: {health.gameObject.name} is destroyed",
                    null,
                    5f);
                reportedDamage = true;
            }
        }
    }
}