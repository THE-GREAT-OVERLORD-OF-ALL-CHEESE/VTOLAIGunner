using System;

namespace AIHelicopterGunner.AIStates.MFDStates
{
    public class State_MFDSwitchTPGPage : AITryState
    {
        private MFD mfd;

        private TargetingMFDPage tgpMfd;


        public override string Name => "Switching To TGP Page";
        public override float WarmUp => 1f;
        public override float CoolDown => 0.5f;

        public State_MFDSwitchTPGPage(MFD mfd, TargetingMFDPage tgpMfd)
        {
            this.mfd = mfd;
            this.tgpMfd = tgpMfd;
        }

        public override bool CanStart()
        {
            return mfd.activePage != tgpMfd.mfdPage;
        }

        public override void StartState()
        {
            mfd.OpenPage(tgpMfd.mfdPage.pageName);
            tgpMfd.mfdPage.ToggleInput();
        }

        public override void UpdateState()
        {
            throw new NotImplementedException();
        }

        public override bool IsOver()
        {
            return true;
        }

        public override void EndState()
        {

        }
    }
}
