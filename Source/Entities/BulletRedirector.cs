using System;
using Celeste.Mod.Entities;
using Monocle;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Celeste.Mod.AletrisSandbox.Entities;

[CustomEntity("AletrisSandbox/BulletRedirector")]
public class BulletRedirector : Entity
{

    public bool kill;
    public int rotation;
    PlayerCollider pc;
    BulletCollider bc;

    int cooldown;

    Sprite s = GFX.SpriteBank.Create("BulletRedirector");

    public BulletRedirector(EntityData data, Vector2 offset)
        : base(data.Position + offset)
    {
        // TODO: read properties from data
        Collider = new Hitbox(16, 16, -8, -8);
        kill = data.Bool("Deadly");
        rotation = data.Int("Rotation");
        Add(pc = new(OnCollide));
        Add(bc = new(BulletCollide));
        s.Rotation = Calc.ToRad((float)rotation);
        Add(s);
    }

    public override void Update()
    {
        base.Update();
        if (cooldown > 0)
            cooldown--;
    }

    public void OnCollide(Player player)
    {
        if (kill) player.Die((player.Center - Center).SafeNormalize());
    }

    public void BulletCollide(IWBTGBullet bullet)
    {
        if (cooldown > 0) { return; }
        bullet.Position = Position;
        bullet.velocity = new Vector2((float)Math.Cos(rotation * Math.PI / 180f), (float)Math.Sin(rotation * Math.PI / 180f)) * bullet.velocity.Length();
        s.Play("active");
        cooldown = 5;
    }
}