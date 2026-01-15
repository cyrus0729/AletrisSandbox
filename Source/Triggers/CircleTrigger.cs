using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.AletrisSandbox.Triggers
{
    [CustomEntity("AletrisSandbox/CircleTrigger")]
    public class CircleTrigger : Trigger
    {
        public bool drawCircle;
        public bool DisableOnLeave;
        public float circleRadius;

        public CircleTrigger(EntityData data, Vector2 offset) : base(data, offset)
        {
            drawCircle = data.Bool("drawCircle", true);
            DisableOnLeave = data.Bool("DisableOnLeave", true);
            circleRadius = data.Float("circleRadius", 6f);
        }

        public override void OnEnter(Player player) // start
        {
            base.OnEnter(player);
            AletrisSandboxModule.Session.CircleMadelineEnabled = drawCircle;
            AletrisSandboxModule.Session.CircleMadelineRadius = circleRadius;
        }

        public override void OnLeave(Player player)
        {
            base.OnLeave(player);
            if (!DisableOnLeave) { return; }
            AletrisSandboxModule.Session.CircleMadelineEnabled = false;
            AletrisSandboxModule.Session.CircleMadelineRadius = 0;
        }

        public void Unload()
        {
            if (!DisableOnLeave) { return; }
            AletrisSandboxModule.Session.CircleMadelineEnabled = false;
            AletrisSandboxModule.Session.CircleMadelineRadius = 0;
        }

    }
}