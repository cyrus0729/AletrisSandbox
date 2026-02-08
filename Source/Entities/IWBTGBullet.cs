using System;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

// thank you code_modding and especially kalobi and snip for dealing with me being dumb at code modding
// without them this would not have worked

namespace Celeste.Mod.AletrisSandbox.Entities;

[CustomEntity("AletrisSandbox/IWBTGBullet"), Tracked]
public class IWBTGBullet : Actor
{
    public Vector2 velocity;
    public readonly Player owner;
    bool dead;

    readonly Collision onCollideH;
    readonly Collision onCollideV;

    int lifetime;

    public IWBTGBullet(Vector2 position, Vector2 velocity, Player owner) : base(position)
    {
        Position = position;
        this.velocity = velocity;
        this.owner = owner;

        Depth = 100;
        Collider = new Hitbox(4f, 4f);

        onCollideH += OnCollideH;
        onCollideV += OnCollideV;

        lifetime = 600;

    #pragma warning disable CL013
        owner.SceneAs<Level>().Add(this);
    #pragma warning restore CL013
        Add(GFX.SpriteBank.Create("IWBTBullet"));
    }

    void OnCollideH(CollisionData data)
    {
        foreach (var v in data.GetType().GetProperties())
        {
            Logger.Log(LogLevel.Info, nameof(AletrisSandboxModule), v.ToString());
        }
        if (data.Hit.OnDashCollide != null) { data.Hit.OnDashCollide.Invoke(owner, velocity); }
        Kill();
    }

    void OnCollideV(CollisionData data)
    {
        if (data.Hit.OnDashCollide != null) { data.Hit.OnDashCollide.Invoke(owner, velocity); }
        Kill();
    }

    public override void Update()
    {
        base.Update();

        MoveH(velocity.X, onCollideH);
        MoveV(velocity.Y, onCollideV);

        if (--lifetime <= 0)
            Kill();

        /*Camera camera = (Scene as Level).Camera;
        if (Position.X < camera.X || Position.X > camera.X + 320f ||
            Position.Y < camera.Y || Position.Y > camera.Y + 180f)
        {
            Kill();
        */

        var level = SceneAs<Level>();
        if (Position.X <= level.Bounds.Left ||
            Position.X >= level.Bounds.Right ||
            Position.Y >= level.Bounds.Bottom ||
            Position.Y <= level.Bounds.Top)
            Kill();

        foreach (var component in Scene.Tracker.GetComponents<BulletCollider>())
        {
            var co = (BulletCollider)component;

            if (co == null || !co.Check(this))
                continue;

            if (dead)
                return;

            co.OnCollide(this);
        }
    }

    public override void Render()
    {
        if (owner.Scene != null)
        {
            //(owner.Scene as Level).Particles.Emit(ParticleTypes.SparkyDust, Position, Color.Yellow);
        }
        base.Render();
    }

    public void Kill()
    {
        dead = true;
        RemoveSelf();
    }
}

[Tracked]
public class BulletCollider : Component
{
    readonly Collider collider;
    public Action<IWBTGBullet> OnCollide;

    public BulletCollider(Action<IWBTGBullet> onCollide, Collider collider = null)
        : base(active: false, visible: false)
    {
        this.collider = collider;
        OnCollide = onCollide;
    }

    public bool Check(IWBTGBullet bullet)
    {
        var collide = Entity.Collider;

        if (collider != null)
            Entity.Collider = collider;
        var result = bullet.CollideCheck(Entity);
        Entity.Collider = collide;

        return result;
    }
}