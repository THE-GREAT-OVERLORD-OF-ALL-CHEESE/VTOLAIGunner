using CheeseMods.AIHelicopterGunner.Character;
using System.Collections.Generic;
using UnityEngine;

namespace CheeseMods.AIHelicopterGunner.Components
{
    public class ReportManager : MonoBehaviour
    {
        public static ReportManager instance;

        public List<Report> reports = new List<Report>();

        private void Awake()
        {
            instance = this;
        }

        public void StartReport(Report report)
        {
            report.timer = 0;
            if (reports.Contains(report))
                return;

            reports.Add(report);
        }

        private void Update()
        {
            for (int i = 0; i < reports.Count; i++)
            {
                Report report = reports[i];

                report.timer += Time.deltaTime;
                if (report.timer > report.timeout || report.cancelReport())
                {
                    reports.Remove(report);
                    return;
                }

                if (report.canReport())
                {
                    report.sayReport();
                    reports.Remove(report);
                    return;
                }
            }
        }
    }
}
