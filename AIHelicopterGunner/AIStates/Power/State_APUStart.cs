namespace AIHelicopterGunner.AIStates.Power
{
    public class State_APUStart : AITryState
    {
        private AuxilliaryPowerUnit apu;

        public override string Name => "Switching on APU";

        public override float WarmUp => 1f;

        public override float CoolDown => 3f;

        public State_APUStart(AuxilliaryPowerUnit apu)
        {
            this.apu = apu;
        }

        public override bool CanStart()
        {
            return !apu.powerEnabled;
        }

        public override void StartState()
        {
            apu.PowerUp();
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
