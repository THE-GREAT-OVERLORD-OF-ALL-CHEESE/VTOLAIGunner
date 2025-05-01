namespace AIHelicopterGunner.AIStates.Power
{
    public class State_MasterPowerOn : AITryState
    {
        private Battery battery;

        public override string Name => "Switching Master Power On";
        public override float WarmUp => 1f;
        public override float CoolDown => 1f;

        public State_MasterPowerOn(Battery battery)
        {
            this.battery = battery;
        }

        public override bool CanStart()
        {
            return !battery.connected;
        }

        public override void StartState()
        {
            battery.ToggleConnection();
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
