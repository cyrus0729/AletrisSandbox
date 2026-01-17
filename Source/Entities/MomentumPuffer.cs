using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.AletrisSandbox.Entities;

[CustomEntity("AletrisSandbox/MomentumPuffer")]
public class MomentumPuffer : Puffer
{
    public MomentumPuffer(EntityData data, Vector2 offset)
        : base(data.Position + offset, data.Bool("right"))
    {
        Get<PlayerCollider>().OnCollide = OnPlayerHit;
    }

    public void OnPlayerHit(Player player)
    {
        if (state != States.Gone && cantExplodeTimer <= 0f)
        {
            if (cannotHitTimer <= 0f)
            {
                if (player.Bottom > lastSpeedPosition.Y + 3f)
                {
                    Explode();
                    GotoGone();
                }
                else
                {
                    player.Bounce(Top);
                    GotoHit(player.Center);
                    MoveToX(anchorPosition.X * player.Speed.X);
                    MoveToY(anchorPosition.Y * player.Speed.Y);
                    idleSine.Reset();
                    anchorPosition = lastSinePosition = Position;
                    eyeSpin = 1f;
                }
            }
            cannotHitTimer = 0.1f;
        }
    }
}