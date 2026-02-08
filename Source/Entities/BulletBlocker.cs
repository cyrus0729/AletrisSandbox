using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.AletrisSandbox.Entities;

[CustomEntity("AletrisSandbox/BulletBlocker")]
public class BulletBlocker : Entity
{

    public bool invis;

    public BulletBlocker(EntityData data, Vector2 offset) : base(data.Position + offset)
    {
        Collider = new Hitbox(data.Width, data.Height);
        invis = data.Bool("Invisible");
        BulletCollider _;
        Add(_ = new(OnCollide));
    }

    void OnCollide(IWBTGBullet bullet)
    {
        bullet.Kill();
    }

    public override void Render() // todo figure out seeker barrier renderer ol
    {
        if (invis) return;
        Draw.Rect(Collider,Color.Silver);
        base.Render();
    }
}