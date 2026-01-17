using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.AletrisSandbox.Entities;

[CustomEntity("AletrisSandbox/OneUseField")]
public class OneUseField : Entity
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

    public OneUseField(EntityData data, Vector2 offset) : base(data.Position + offset)
    {
        killplayer = data.Bool("kill");
        depth = data.Int("Depth", 8500);
        Depth = depth;
        color = data.HexColor("inactiveolor", Calc.HexToColor("#00FF00"));
        BorderColor = data.HexColor("inactivebordercolor", Calc.HexToColor("#008800"));
        ActiveColor = data.HexColor("activecolor", Calc.HexToColor("#FF0000"));
        ActiveBorderColor = data.HexColor("activebordercolor", Calc.HexToColor("#880000"));
        ActivatingColor = data.HexColor("activatingcolor", Calc.HexToColor("#FFFF00"));
        ActivatingBorderColor = data.HexColor("activatingbordercolor", Calc.HexToColor("#888800"));
        Collider = new Hitbox(data.Width, data.Height);
        Add(pc = new(OnCollide));
    }

    void OnCollide(Player player)
    {
        if (OnCollide != null && kill)
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

        if (hasCollided && !check) // player left
        {
            currentRectColor = ActiveColor;
            currentRectBorderColor = ActiveBorderColor;

            switch (kill)
            {
                case true:
                    kill = true;

                    break;
                case false:
                    Scene.Add(new Solid(Position, Width, Height, true));

                    break;
            }
            hasCollided = false;
        }
    }

    public override void Added(Scene scene)
    {
        base.Added(scene);
        currentRectColor = color;
        currentRectBorderColor = BorderColor;
    }

    public override void Render()
    {
        Draw.HollowRect(Collider, currentRectBorderColor);
        Draw.Rect(Collider, currentRectColor);
        base.Render();
    }
}