namespace Celeste.Mod.AletrisSandbox
{
    public class AletrisSandboxModuleSession : EverestModuleSession
    {
        public bool oldEnabledConfig { get; set; } = false;
        public bool IWBTGGunEnabled { get; set; } = false;
        public bool IWBTGGunMouseAimEnabled { get; set; } = false;
        public bool IWBTGGunAutofireEnabled { get; set; } = false;
        public bool IWBTGGunDestroysStuff { get; set; } = false;
        public bool IWBTGGunHitsStuff { get; set; } = false;
        public bool HPSystemEnabled { get; set; } = false;
        public bool IWBTGJumpEnabled { get; set; } = false;
        public bool RelativisticVelocityEnabled { get; set; } = false;
        public int HPAmount { get; set; } = 1000;
        public int HPMax { get; set; } = 1000;
        public int Maxbullets { get; set; } = 4;
    }
}
