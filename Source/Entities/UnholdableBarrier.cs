using Celeste;
using Celeste.Mod.AletrisSandbox;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System.Collections.Generic;

// THANK YOU *SAMAH*, USSRNAME, MADDIE, AON, DZHAKE

namespace AletrisSandbox.Entities;

[Tracked, CustomEntity("AletrisSandbox/UnholdableBarrier")]
public class UnholdableBarrier : Solid
{
    public float Flash;
    public float Solidify;
    public bool Flashing;
    public string colore;
    float solidifyDelay;
    protected List<Vector2> particles = new();
    List<UnholdableBarrier> adjacent = new();
    readonly float[] speeds = { 12f, 20f, 40f };

    public UnholdableBarrier(EntityData data, Vector2 offset)
        : base(data.Position + offset, data.Width, data.Height, true)
    {
        colore = data.Attr("color", "a4911e");
        Collidable = false;
        for (var i = 0; i < Width * Height / 16f; i++)
            particles.Add(new(Calc.Random.NextFloat(Width - 1f), Calc.Random.NextFloat(Height - 1f)));
        Collidable = false;
        Add(new AletrisSandboxModule.OnlyBlocksPlayer());
    }

    public override void Render()
    {
        if (Flashing)
        {
            Flash = Calc.Approach(Flash, 0f, Engine.DeltaTime * 4f);
            if (Flash <= 0f)
                Flashing = false;
        }
        else if (solidifyDelay > 0f)
            solidifyDelay -= Engine.DeltaTime;
        else if (Solidify > 0f)
            Solidify = Calc.Approach(Solidify, 0f, Engine.DeltaTime);
        var num = speeds.Length;
        var i = 0;

        for (var count = particles.Count; i < count; i++)
        {
            var value = particles[i] + new Vector2(0f, 1f) * speeds[i % num] * Engine.DeltaTime;
            value.Y = mod(value.Y, Height - 1f);
            value.X = mod(value.X, Width - 1f);
            particles[i] = value;
        }
        Draw.Rect(Collider, Calc.HexToColor(colore));
        base.Render();
    }

    protected float mod(float x, float m)
        => (x % m + m) % m;
}