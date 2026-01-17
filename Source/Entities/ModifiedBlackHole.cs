using Celeste;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace AletrisSandbox;

public class Hole : Component
{
    public Hole(bool active, bool visible)
        : base(active, visible) { }

    public bool Check(Player player)
    {
        var collider = Entity.Collider;
        var flag = holeCollider != null;
        if (flag)
            Entity.Collider = holeCollider;
        var flag2 = player.CollideCheck(Entity);
        Entity.Collider = collider;

        return flag2;
    }

    public Circle holeCollider;
}

[CustomEntity("AletrisSandbox/ModifiedBlackHole")]
public class BlackHole : Entity
{
    Player player;
    readonly Hole hole;
    readonly float speedModifier;
    readonly float forceModifier;

    public BlackHole(EntityData data, Vector2 offset)
        : base(data.Position + offset)
    {
        speedModifier = data.Float("SpeedModifier", 1.02f);
        forceModifier = data.Float("ForceModifier", 0.8f);
        var auraRadius1 = data.Float("auraRadius", 48f);
        var holeRadius1 = data.Float("holeRadius", 8f);
        var aura = GFX.SpriteBank.Create("BHAura");
        var blackhole = GFX.SpriteBank.Create("BlackHole");
        aura.Scale = new(auraRadius1 / 48f, auraRadius1 / 48f);
        blackhole.Scale = new(holeRadius1 / 8f, holeRadius1 / 8f);
        Add(blackhole);
        Add(aura);
        aura.Origin.X = aura.Width / 2f;
        aura.Origin.Y = aura.Height / 2f;
        blackhole.Origin.X = blackhole.Height / 2f;
        blackhole.Origin.Y = blackhole.Width / 2f;

        Collider = new Circle(auraRadius1);
        Add(hole = new(true, true));
        hole.holeCollider = new(holeRadius1);
    }

    public override void Update()
    {
        base.Update();
        var flag = CollideCheck<Player>();

        if (flag)
        {
            player = CollideFirst<Player>();

            if (player == null)
                return;

            {
                Drag(player);
                HoleKill(player);
            }
        }
    }

    void HoleKill(Player plr)
    {
        var flag = hole.Check(plr);

        if (flag)
        {
            var invincible = SaveData.Instance.Assists.Invincible;

            if (invincible)
            {
                plr.Play("event:/game/general/assist_screenbottom");
                plr.Bounce(Top);
            }
            else
                plr.Die(Vector2.Zero);
        }
    }

    void Drag(Player plr)
    {
        var vector = (Position - plr.Position) * forceModifier;
        plr.Speed *= speedModifier;
        plr.Speed += vector;
    }
}