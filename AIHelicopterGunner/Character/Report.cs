using CheeseMods.AIHelicopterGunner.Components;
using System;

namespace CheeseMods.AIHelicopterGunner.Character
{
    public class Report
    {
        public float timeout;
        public float timer;

        public Func<bool> cancelReport;
        public Func<bool> canReport;
        public Action sayReport;

        public Report(float timeout,
            Func<bool> cancelReport,
            Func<bool> canReport,
            Action sayReport)
        {
            this.timeout = timeout;
            this.cancelReport = cancelReport;
            this.canReport = canReport;
            this.sayReport = sayReport;
        }

        public void StartReport()
        {
            ReportManager.instance.StartReport(this);
        }
    }
}
