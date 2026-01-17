using System;
using System.Collections.Generic;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System.Text.RegularExpressions;
using Mono.Cecil.Cil;
using MonoMod.Cil;

// to the entirety of #code_modding: thank you so much lmao i could not have done this without you guys
// oh and to snip for dealing with all my stupid bullshit .w.

namespace Celeste.Mod.AletrisSandbox.Entities;

[CustomEntity("AletrisSandbox/HitboxController")]
public class HitboxController : Entity
{
    readonly Collider newHitbox;
    readonly Collider newHurtbox;

    readonly Collider newduckHitbox;
    readonly Collider newduckHurtbox;

    readonly Collider newfeatherHitbox;
    readonly Collider newfeatherHurtbox;

    public Player player;

    public bool ModifyHitbox;

    public HitboxController(EntityData data, Vector2 offset) : base(data.Position + offset)
    {
        newHitbox = Hawa.ParseCollider(data.Attr("Hitbox"));
        newHurtbox = Hawa.ParseCollider(data.Attr("Hurtbox"));

        newduckHitbox = Hawa.ParseCollider(data.Attr("duckHitbox"));
        newduckHurtbox = Hawa.ParseCollider(data.Attr("duckHurtbox"));

        newfeatherHitbox = Hawa.ParseCollider(data.Attr("featherHitbox"));
        newfeatherHurtbox = Hawa.ParseCollider(data.Attr("featherHurtbox"));

        ModifyHitbox = data.Bool("modifyHitbox");
    }

    public class HitboxHurtboxData
    {
        public Collider NormalHitbox { get; set; }
        public Collider DuckHitbox { get; set; }
        public Collider StarFlyHitbox { get; set; }
        public Collider NormalHurtbox { get; set; }
        public Collider DuckHurtbox { get; set; }
        public Collider StarFlyHurtbox { get; set; }

        public HitboxHurtboxData()
        {
            NormalHitbox = new Hitbox(8f, 11f, -4f, -11f);
            DuckHitbox = new Hitbox(8f, 6f, -4f, -6f);
            NormalHurtbox = new Hitbox(8f, 9f, -4f, -11f);
            DuckHurtbox = new Hitbox(8f, 4f, -4f, -6f);
            StarFlyHitbox = new Hitbox(8f, 8f, -4f, -10f);
            StarFlyHurtbox = new Hitbox(6f, 6f, -3f, -9f);
        }
    }

    void UpdatePlayerHitboxes(Player playr, HitboxHurtboxData data)
    {
        playr.normalHitbox.Width = data.NormalHitbox.Width;
        playr.normalHitbox.Height = data.NormalHitbox.Height;
        playr.normalHitbox.Position = data.NormalHitbox.Position;

        playr.duckHitbox.Width = data.DuckHitbox.Width;
        playr.duckHitbox.Height = data.DuckHitbox.Height;
        playr.duckHitbox.Position = data.DuckHitbox.Position;

        playr.starFlyHitbox.Width = data.StarFlyHitbox.Width;
        playr.starFlyHitbox.Height = data.StarFlyHitbox.Height;
        playr.starFlyHitbox.Position = data.StarFlyHitbox.Position;

        playr.normalHurtbox.Width = data.NormalHurtbox.Width;
        playr.normalHurtbox.Height = data.NormalHurtbox.Height;
        playr.normalHurtbox.Position = data.NormalHurtbox.Position;

        playr.duckHurtbox.Width = data.DuckHurtbox.Width;
        playr.duckHurtbox.Height = data.DuckHurtbox.Height;
        playr.duckHurtbox.Position = data.DuckHurtbox.Position;

        playr.starFlyHurtbox.Width = data.StarFlyHurtbox.Width;
        playr.starFlyHurtbox.Height = data.StarFlyHurtbox.Height;
        playr.starFlyHurtbox.Position = data.StarFlyHurtbox.Position;
    }

    public void Load(Scene scene)
    {
        if (!ModifyHitbox)
            return;

        var playr = Scene.Tracker.GetEntity<Player>();

        HitboxHurtboxData h = new();
        h.NormalHitbox = newHitbox;
        h.DuckHitbox = newduckHitbox;
        h.StarFlyHitbox = newfeatherHitbox;
        h.NormalHurtbox = newHurtbox;
        h.DuckHurtbox = newduckHurtbox;
        h.StarFlyHurtbox = newfeatherHurtbox;

        UpdatePlayerHitboxes(playr, h);
    }

    public void Unload(Scene scene)
    {

        if (!ModifyHitbox)
            return;

        var playr = Scene.Tracker.GetEntity<Player>();

        UpdatePlayerHitboxes(playr, new());
    }
}