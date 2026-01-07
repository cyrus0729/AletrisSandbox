using System;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System.Text.RegularExpressions;

// to the entirety of #code_modding: thank you so much lmao i could not have done this without you guys
// oh and to snip for dealing with all my stupid bullshit .w.

namespace Celeste.Mod.AletrisSandbox.Entities
{

    [CustomEntity("AletrisSandbox/HitboxController")]
    public class HitboxController : Entity
    {

        Collider newHitbox;
        Collider newHurtbox;

        Collider newduckHitbox;
        Collider newduckHurtbox;

        Collider newfeatherHitbox;
        Collider newfeatherHurtbox;

        public Player player;

        public bool ModifyHitbox;

        public HitboxController(EntityData data, Vector2 offset) : base(data.Position + offset)
        {

            newHitbox = hawa.ParseCollider(data.Attr("Hitbox"));
            newHurtbox = hawa.ParseCollider(data.Attr("Hurtbox"));

            newduckHitbox = hawa.ParseCollider(data.Attr("duckHitbox"));
            newduckHurtbox = hawa.ParseCollider(data.Attr("duckHurtbox"));

            newfeatherHitbox = hawa.ParseCollider(data.Attr("featherHitbox"));
            newfeatherHurtbox = hawa.ParseCollider(data.Attr("featherHurtbox"));

            ModifyHitbox = data.Bool("modifyHitbox", false);

        }

        public override void Awake(Scene scene)
        {
            base.Awake(scene);

            if (!ModifyHitbox) return;

            Player playr = Scene.Tracker.GetEntity<Player>();

            playr.normalHitbox.Width = newHitbox.Width;
            playr.normalHitbox.Height = newHitbox.Height;
            playr.normalHitbox.Position = newHitbox.Position;

            playr.duckHitbox.Width = newduckHitbox.Width;
            playr.duckHitbox.Height = newduckHitbox.Height;
            playr.duckHitbox.Position = newduckHitbox.Position;

            playr.starFlyHitbox.Width = newfeatherHitbox.Width;
            playr.starFlyHitbox.Height = newfeatherHitbox.Height;
            playr.starFlyHitbox.Position = newfeatherHitbox.Position;

            playr.normalHurtbox.Width = newHurtbox.Width;
            playr.normalHurtbox.Height = newHurtbox.Height;
            playr.normalHurtbox.Position = newHurtbox.Position;

            playr.duckHurtbox.Width = newduckHurtbox.Width;
            playr.duckHurtbox.Height = newduckHurtbox.Height;
            playr.duckHurtbox.Position = newduckHurtbox.Position;

            playr.starFlyHurtbox.Width = newfeatherHurtbox.Width;
            playr.starFlyHurtbox.Height = newfeatherHurtbox.Height;
            playr.starFlyHurtbox.Position = newfeatherHurtbox.Position;
        }

    }
}