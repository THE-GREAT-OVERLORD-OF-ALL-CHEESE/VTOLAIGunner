namespace AIHelicopterGunner.AIStates.Power
{
    public class State_MasterArmOn : AITryState
    {
        private WeaponManager wm;

        public override string Name => "Switching Master Arm On";
        public override float WarmUp => 1f;
        public override float CoolDown => 0.5f;

        public State_MasterArmOn(WeaponManager wm)
        {
            this.wm = wm;
        }

        public override bool CanStart()
        {
            return !wm.isMasterArmed;
        }

        public override void StartState()
        {
            wm.ToggleMasterArmed();
        }

        public override void UpdateState()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsOver()
        {
            return true;
        }

        public override void EndState()
        {
            //Do Nothing
        }
    }
}
