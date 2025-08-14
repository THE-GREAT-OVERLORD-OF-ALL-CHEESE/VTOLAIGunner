using CheeseMods.AIHelicopterGunner.Spotting;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CheeseMods.AIHelicopterGunner.Components
{
    public class ReportManager : MonoBehaviour
    {
        private float timeout;
        private float timer;

        private Func<bool> canReport;
        private Action sayReport;
    }
}
