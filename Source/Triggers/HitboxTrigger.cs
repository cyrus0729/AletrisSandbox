using System;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System.Text.RegularExpressions;

// to the entirety of #code_modding: thank you so much lmao i could not have done this without you guys
// oh and to snip for dealing with all my stupid bullshit .w.

namespace Celeste.Mod.AletrisSandbox.Entities;

[CustomEntity("AletrisSandbox/HitboxTrigger")]
public class HitboxTrigger : Trigger
{
    readonly Collider newHitbox;
    readonly Collider newHurtbox;

    readonly Collider newduckHitbox;
    readonly Collider newduckHurtbox;

    readonly Collider newfeatherHitbox;
    readonly Collider newfeatherHurtbox;

    public Player player;

    public bool ModifyHitbox;

    public HitboxTrigger(EntityData data, Vector2 offset) : base(data, offset)
    {
        newHitbox = Hawa.ParseCollider(data.Attr("Hitbox"));
        newHurtbox = Hawa.ParseCollider(data.Attr("Hurtbox"));

        newduckHitbox = Hawa.ParseCollider(data.Attr("duckHitbox"));
        newduckHurtbox = Hawa.ParseCollider(data.Attr("duckHurtbox"));

        newfeatherHitbox = Hawa.ParseCollider(data.Attr("featherHitbox"));
        newfeatherHurtbox = Hawa.ParseCollider(data.Attr("featherHurtbox"));

        ModifyHitbox = data.Bool("modifyHitbox", true);
    }

    public override void OnEnter(Player player)
    {
        base.OnEnter(player);

        if (!ModifyHitbox)
            return;

        player.normalHitbox.Width = newHitbox.Width;
        player.normalHitbox.Height = newHitbox.Height;
        player.normalHitbox.Position = newHitbox.Position;

        player.duckHitbox.Width = newduckHitbox.Width;
        player.duckHitbox.Height = newduckHitbox.Height;
        player.duckHitbox.Position = newduckHitbox.Position;

        player.starFlyHitbox.Width = newfeatherHitbox.Width;
        player.starFlyHitbox.Height = newfeatherHitbox.Height;
        player.starFlyHitbox.Position = newfeatherHitbox.Position;

        player.normalHurtbox.Width = newHurtbox.Width;
        player.normalHurtbox.Height = newHurtbox.Height;
        player.normalHurtbox.Position = newHurtbox.Position;

        player.duckHurtbox.Width = newduckHurtbox.Width;
        player.duckHurtbox.Height = newduckHurtbox.Height;
        player.duckHurtbox.Position = newduckHurtbox.Position;

        player.starFlyHurtbox.Width = newfeatherHurtbox.Width;
        player.starFlyHurtbox.Height = newfeatherHurtbox.Height;
        player.starFlyHurtbox.Position = newfeatherHurtbox.Position;
    }
}