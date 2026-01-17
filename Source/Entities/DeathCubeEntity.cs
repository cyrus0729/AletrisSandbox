using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System.Runtime.CompilerServices;
using static Celeste.Mod.AletrisSandbox.Entities.NeedleEntity;
using Directions = Celeste.MoveBlock.Directions;

namespace Celeste.Mod.AletrisSandbox.Entities;

[CustomEntity(
    "AletrisSandbox/DeathCubeUp = CubeUp",
    "AletrisSandbox/DeathCubeDown = CubeDown",
    "AletrisSandbox/DeathCubeLeft = CubeLeft",
    "AletrisSandbox/DeathCubeRight = CubeRight")]
public class DeathCubeEntity : Entity
{
    PlayerCollider pc;
    Solid solid;
    public readonly Directions direction;
    Sprite sprite = GFX.SpriteBank.Create("DeathCubeEntity");
    readonly EntityData data;
    Vector2 offset;

    public DeathCubeEntity(EntityData data, Vector2 offset, Hitbox collider, Sprite sprite, float rotation) : base(data.Position + offset)
    {
        this.data = data;
        this.offset = offset;
        Collider = collider;
        Add(pc = new(OnCollide));
        Add(sprite);
        sprite.Rotation = rotation;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    void OnCollide(Player player)
    {
        if (OnCollide != null)
            player.Die((player.Center - Center).SafeNormalize());
    }

    public static Entity CubeUp(Level level, LevelData leveldata, Vector2 offset, EntityData data)
    {
        var hitboxes = data.Bool("UnforgivingHitbox");

        switch (hitboxes)
        {
            case true:
                return new DeathCubeEntity(data, offset, new(16, 16, -8, -8), GFX.SpriteBank.Create(data.Attr("Sprite", "DeathCubeEntity")), 0f);
            case false:
                return new DeathCubeEntity(data, offset, new(14, 14, -7, -7), GFX.SpriteBank.Create(data.Attr("Sprite", "DeathCubeEntity")), 0f);
        }
    }

    public static Entity CubeDown(Level level, LevelData leveldata, Vector2 offset, EntityData data)
    {
        var hitboxes = data.Bool("UnforgivingHitbox");

        switch (hitboxes)
        {
            case true:
                return new DeathCubeEntity(data, offset, new(16, 16, -8, -8), GFX.SpriteBank.Create(data.Attr("Sprite", "DeathCubeEntity")), 180f.ToRad());
            case false:
                return new DeathCubeEntity(data, offset, new(14, 14, -7, -7), GFX.SpriteBank.Create(data.Attr("Sprite", "DeathCubeEntity")), 180f.ToRad());
        }
    }

    public static Entity CubeLeft(Level level, LevelData leveldata, Vector2 offset, EntityData data)
    {
        var hitboxes = data.Bool("UnforgivingHitbox");

        switch (hitboxes)
        {
            case true:
                return new DeathCubeEntity(data, offset, new(16, 16, -8, -8), GFX.SpriteBank.Create(data.Attr("Sprite", "DeathCubeEntity")), 270f.ToRad());
            case false:
                return new DeathCubeEntity(data, offset, new(14, 14, -7, -7), GFX.SpriteBank.Create(data.Attr("Sprite", "DeathCubeEntity")), 270f.ToRad());
        }
    }

    public static Entity CubeRight(Level level, LevelData leveldata, Vector2 offset, EntityData data)
    {
        var hitboxes = data.Bool("UnforgivingHitbox");

        switch (hitboxes)
        {
            case true:
                return new DeathCubeEntity(data, offset, new(16, 16, -8, -8), GFX.SpriteBank.Create(data.Attr("Sprite", "DeathCubeEntity")), 90f.ToRad());
            case false:
                return new DeathCubeEntity(data, offset, new(14, 14, -7, -7), GFX.SpriteBank.Create(data.Attr("Sprite", "DeathCubeEntity")), 90f.ToRad());
        }
    }

    public override void Added(Scene scene)
    {
        base.Added(scene);
        var hitboxes = data.Bool("UnforgivingHitbox");

        switch (hitboxes)
        {
            case true:
                scene.Add(solid = new(Position + new Vector2(-7, -7), Width - 2f, Height - 2f, false));
                Add(new ClimbBlocker(edge: true));

                break;
            case false:
                scene.Add(solid = new(Position + new Vector2(-6, -6), Width - 2f, Height - 2f, false));
                Add(new ClimbBlocker(edge: true));

                break;
        }
    }
}