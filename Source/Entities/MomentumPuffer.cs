using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.AletrisSandbox.Entities
{
    [CustomEntity("AletrisSandbox/MomentumPuffer")]
    public class MomentumPuffer : Puffer
    {
        public MomentumPuffer(EntityData data, Vector2 offset)
            : base(data.Position + offset, data.Bool("right", false))
        {
            Get<PlayerCollider>().OnCollide = OnPlayerHit;
        }

        public void OnPlayerHit(Player player)
        {
            if (this.state != Puffer.States.Gone && this.cantExplodeTimer <= 0f)
            {
                if (this.cannotHitTimer <= 0f)
                {
                    if (player.Bottom > this.lastSpeedPosition.Y + 3f)
                    {
                        this.Explode();
                        this.GotoGone();
                    }
                    else
                    {
                        player.Bounce(base.Top);
                        this.GotoHit(player.Center);
                        base.MoveToX(this.anchorPosition.X * player.Speed.X, null);
                        base.MoveToY(this.anchorPosition.Y * player.Speed.Y, null);
                        this.idleSine.Reset();
                        this.anchorPosition = (this.lastSinePosition = this.Position);
                        this.eyeSpin = 1f;
                    }
                }
                this.cannotHitTimer = 0.1f;
            }
        }

    }
}
