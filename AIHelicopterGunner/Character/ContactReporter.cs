using CheeseMods.AIHelicopterGunner.Character;
using CheeseMods.AIHelicopterGunner.Spotting;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIHelicopterGunner.Character
{
    public class ContactReporter
    {
        public GunnerTargetSpotter spotter;
        public IVoice voice;

        public List<TargetMetaData> unreportedContacts = new List<TargetMetaData>();
        public List<TargetMetaData> reportedContacts = new List<TargetMetaData>();

        private float waitForMoreContactsTimer;
        private float contactReportDelay = 0.5f;

        public ContactReporter(GunnerTargetSpotter spotter, IVoice voice)
        {
            this.spotter = spotter;
            this.voice = voice;
        }

        public void UpdateContactReporting()
        {
            List<TargetMetaData> currentVisibleTarget = spotter.visibleTargets.ToList();

            for (int i = 0; i < currentVisibleTarget.Count; i++)
            {
                TargetMetaData current = currentVisibleTarget[i];
                if (reportedContacts.Contains(current) || unreportedContacts.Contains(current))
                {
                    currentVisibleTarget.RemoveAt(i);
                    i--;
                }
            }

            unreportedContacts.AddRange(currentVisibleTarget);

            if (unreportedContacts.Count > 0)
            {
                waitForMoreContactsTimer += Time.deltaTime;
                if (waitForMoreContactsTimer > contactReportDelay)
                {
                    waitForMoreContactsTimer = 0;

                    Debug.Log($"Reporting something");
                    TargetMetaData currentContact = unreportedContacts.First();
                    voice.SayReportContact(currentContact);
                    unreportedContacts.Remove(currentContact);
                    reportedContacts.Add(currentContact);
                }
            }
        }
    }
}
