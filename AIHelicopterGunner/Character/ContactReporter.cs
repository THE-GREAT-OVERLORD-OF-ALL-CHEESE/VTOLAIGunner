using CheeseMods.AIHelicopterGunner;
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
        public Transform refTf;

        public List<TargetMetaData> unreportedContacts = new List<TargetMetaData>();
        public List<TargetMetaData> reportedContacts = new List<TargetMetaData>();

        private float waitForMoreContactsTimer;
        private float contactReportDelay = 0.5f;

        private float lastReportedTime;
        private float reportCooldown = 3f;

        public ContactReporter(GunnerTargetSpotter spotter, IVoice voice, Transform refTf)
        {
            this.spotter = spotter;
            this.voice = voice;
            this.refTf = refTf;
        }

        public void UpdateContactReporting()
        {
            UpdateUnreportedContacts();
            ReportUnreportedContacts();
        }

        private void UpdateUnreportedContacts()
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

            if (currentVisibleTarget.Count() > 0)
            {
                waitForMoreContactsTimer = 0;
            }
            unreportedContacts.AddRange(currentVisibleTarget);
        }

        private void ReportUnreportedContacts()
        {
            if (unreportedContacts.Count > 0)
            {
                waitForMoreContactsTimer += Time.deltaTime;
                if (waitForMoreContactsTimer > contactReportDelay && Time.time > lastReportedTime + reportCooldown)
                {
                    waitForMoreContactsTimer = 0;
                    lastReportedTime = Time.time;

                    TargetMetaData currentContact = unreportedContacts.First();

                    List<TargetMetaData> groupToReport = new List<TargetMetaData>();
                    groupToReport = unreportedContacts.Where(c => c.TargetType == currentContact.TargetType).ToList();

                    Vector3 averagePos = Vector3.zero;
                    foreach (TargetMetaData target in groupToReport)
                    {
                        averagePos += target.Actor.position;
                    }
                    averagePos /= (float)groupToReport.Count;

                    Vector3 offset = averagePos - refTf.position;
                    offset.y = 0;
                    float bearing = Vector3.SignedAngle(Vector3.ProjectOnPlane(refTf.forward, Vector3.up),
                        Vector3.ProjectOnPlane(offset, Vector3.up),
                        Vector3.up);

                    Debug.Log($"{Main.aiGunnerName}: Reporting: {string.Join(", ", groupToReport.Select(m => m.Actor.actorName))}");
                    voice.SayReportContact(currentContact, groupToReport.Count(), bearing, offset.magnitude);

                    foreach (TargetMetaData reported in groupToReport)
                    {
                        unreportedContacts.Remove(reported);
                        reportedContacts.Add(reported);
                    }
                }
            }
        }
    }
}
