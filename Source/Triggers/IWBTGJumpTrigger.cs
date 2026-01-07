using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.AletrisSandbox.Triggers
{
    [CustomEntity("AletrisSandbox/IWBTGJumpTrigger")]
    public class IWBTGJumpTrigger : Trigger
    {
        public bool enableJump;

        public IWBTGJumpTrigger(EntityData data, Vector2 offset) : base(data, offset)
        {
            enableJump = data.Bool("Enable", true);
        }

        public override void OnEnter(Player player)
        {
            base.OnEnter(player);
            AletrisSandboxModule.Session.IWBTGJumpEnabled = enableJump;
        }
    }
} 
