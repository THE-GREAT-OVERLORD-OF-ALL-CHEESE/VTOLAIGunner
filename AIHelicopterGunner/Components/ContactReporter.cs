using CheeseMods.AIHelicopterGunner.Spotting;
using System.Collections.Generic;
using UnityEngine;

namespace CheeseMods.AIHelicopterGunner.Components
{
    public class ContactReporter : MonoBehaviour
    {
        public List<TargetMetaData> unreportedContacts = new List<TargetMetaData>();
        public List<TargetMetaData> reportedContacts = new List<TargetMetaData>();

        private float waitForMoreContactsTimer;
    }
}
