using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.AletrisSandbox.Entities;

[CustomEntity("AletrisSandbox/OneUseField")]
public class OneUseField : Solid
{
    public Color color;
    public Color BorderColor;
    public Color ActiveColor;
    public Color ActiveBorderColor;
    public Color ActivatingColor;
    public Color ActivatingBorderColor;

    public Color currentRectColor;
    public Color currentRectBorderColor;

    public new int depth;

    public Solid solid;
    bool killplayer;
    bool kill;
    public PlayerCollider pc;
    bool hasCollided;
    bool active;

    public OneUseField(EntityData data, Vector2 offset) : base(data.Position + offset, data.Width, data.Height, true)
    {
        Collidable = false;
        killplayer = data.Bool("kill");
        depth = data.Int("Depth", 8500);
        Depth = depth;
        color = data.HexColor("InactiveColor", Calc.HexToColor("#00FF00"));
        BorderColor = data.HexColor("InactiveBorderColor", Calc.HexToColor("#008800"));
        ActiveColor = data.HexColor("ActiveColor", Calc.HexToColor("#FF0000"));
        ActiveBorderColor = data.HexColor("ActiveBorderColor", Calc.HexToColor("#880000"));
        ActivatingColor = data.HexColor("ActivatingColor", Calc.HexToColor("#FFFF00"));
        ActivatingBorderColor = data.HexColor("ActivatingBorderColor", Calc.HexToColor("#888800"));
        Collider = new Hitbox(data.Width, data.Height);
        Add(pc = new(OnCollide));
    }

    internal new void OnCollide(Player player)
    {
        if (kill)
            player.Die((player.Center - Center).SafeNormalize());
    }

    public override void Update()
    {
        if (Scene.Tracker.GetEntity<Player>() is not { } player)
            return;

        var check = CollideCheck<Player>();

        if (!hasCollided && check) // player entered
        {
            hasCollided = true;
            currentRectColor = ActivatingColor;
            currentRectBorderColor = ActivatingBorderColor;
        }

        if (!active && hasCollided && !check) // player left
        {
            active = true;
            currentRectColor = ActiveColor;
            currentRectBorderColor = ActiveBorderColor;

            switch (kill)
            {
                case true:
                    kill = true; break;
                case false:
                    Collidable = true; break;
            }
            hasCollided = false;
        }

        foreach (StaticMover staticMover in staticMovers)
        {
            staticMover.Entity.Depth = depth - 1;

            Spikes spikes = staticMover.Entity as Spikes;
            if (spikes != null)
            {
                spikes.EnabledColor = currentRectBorderColor;
                spikes.DisabledColor = currentRectBorderColor;
                spikes.VisibleWhenDisabled = true;
                spikes.SetSpikeColor(currentRectBorderColor);
            }

            Spring spring = staticMover.Entity as Spring;
            if (spring != null)
            {
                spring.DisabledColor = currentRectBorderColor;
                spring.VisibleWhenDisabled = true;
            }
        }

        if (Collidable) { EnableStaticMovers(); }
        else { DisableStaticMovers(); }

    }

    public override void Added(Scene scene)
    {
        base.Added(scene);
        currentRectColor = color;
        currentRectBorderColor = BorderColor;
    }

    public override void Render()
    {
        Draw.Rect(Collider, currentRectColor);
        Draw.HollowRect(Collider, currentRectBorderColor);
        base.Render();
    }
}