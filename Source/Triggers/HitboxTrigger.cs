using System;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System.Text.RegularExpressions;

// to the entirety of #code_modding: thank you so much lmao i could not have done this without you guys
// oh and to snip for dealing with all my stupid bullshit .w.

namespace Celeste.Mod.AletrisSandbox.Entities
{

    [CustomEntity("AletrisSandbox/HitboxTrigger")]
    public class HitboxTrigger : Trigger
    {

        Collider newHitbox;
        Collider newHurtbox;

        Collider newduckHitbox;
        Collider newduckHurtbox;

        Collider newfeatherHitbox;
        Collider newfeatherHurtbox;

        public Player player;

        public bool ModifyHitbox;

        public HitboxTrigger(EntityData data, Vector2 offset) : base(data, offset)
        {

            newHitbox = ParseCollider(data.Attr("Hitbox"));
            newHurtbox = ParseCollider(data.Attr("Hurtbox"));

            newduckHitbox = ParseCollider(data.Attr("duckHitbox"));
            newduckHurtbox = ParseCollider(data.Attr("duckHurtbox"));

            newfeatherHitbox = ParseCollider(data.Attr("featherHitbox"));
            newfeatherHurtbox = ParseCollider(data.Attr("featherHurtbox"));

            ModifyHitbox = data.Bool("modifyHitbox", false);

        }

        public Collider ParseCollider(string str) // in format (R/C:X,Y,oX,oY)
        {
            Regex rgc = new Regex(@"\n\((C).*?(\d),(\d),(\d)\)");
            Regex rgr = new Regex(@"\n\((R).*?(\d),(\d),(\d),(\d)\)");
            Match matchr = rgr.Match(str);
            Match matchc = rgc.Match(str);
            if (matchr.Success)
            {
                return new Hitbox(float.Parse(matchr.Groups[2].Value),
                                  float.Parse(matchr.Groups[3].Value),
                                  float.Parse(matchr.Groups[4].Value),
                                  float.Parse(matchr.Groups[5].Value));
            } else if (matchc.Success) {
                return new Circle(
                    float.Parse(matchr.Groups[2].Value),
                    float.Parse(matchr.Groups[3].Value),
                    float.Parse(matchr.Groups[4].Value));
            }
            else
            {
                Logger.Log(LogLevel.Warn, "AletrisSandbox", "Wrong syntax for string" + str + "! (R:X,Y,oX,oY)/(C:R,X,Y)");
                return new Hitbox(1f, 1f);
            }
        }

        public override void OnEnter(Player player)
        {
            base.OnEnter(player);

            if (!ModifyHitbox) return;
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
}