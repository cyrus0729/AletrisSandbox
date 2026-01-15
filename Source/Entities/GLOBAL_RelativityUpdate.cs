using System;
using Celeste;
using Celeste.Mod;
using Celeste.Mod.AletrisSandbox;
using Celeste.Mod.Entities;
using Monocle;

namespace Celeste.Mod.AletrisSandbox.Entities
{
    [CustomEntity("AletrisSandbox/GLOBAL_RelativityUpdate")]
    public class GLOBAL_RelativityUpdate : Entity
    {
        public GLOBAL_RelativityUpdate()
        {
            AddTag(Tags.Global);
        }

        public void Update(Scene scene)
        {
            bool enableRelativisticVel = AletrisSandboxModule.Session.RelativisticVelocityEnabled ||
                                         AletrisSandboxModule.Settings.MiscelleaneousMenu.RelativisticVelocityOverride;

            //float dt = Engine.DeltaTime;
            //PropertyInfo engineDeltaTimeProp = typeof(Engine).GetProperty("DeltaTime");
            if (enableRelativisticVel)
            {

                Player playr = Scene.Tracker.GetEntity<Player>();

                float speedcap = 1000f;

                if (Math.Abs(playr.Speed.X) > 0)
                {
                    Logger.Log(LogLevel.Debug, "AletrisSandbox", "Clamped Speed:" + speedcap / Math.Abs(playr.Speed.X));
                    Logger.Log(LogLevel.Debug, "AletrisSandbox", "Relativistic TimeRate:" + Math.Clamp(speedcap / Math.Abs(playr.Speed.X), 0f, 1.0f));
                    Add(new TimeRateModifier(Math.Clamp(speedcap / Math.Abs(playr.Speed.X), 0f, 1.0f)));
                    //(n)x of default speed
                }
                else
                {
                    Add(new TimeRateModifier(1.0f));
                }
            }
            base.Awake(scene);
        }
    }
}