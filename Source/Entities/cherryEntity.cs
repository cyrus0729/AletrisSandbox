using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System.Runtime.CompilerServices;

namespace Celeste.Mod.AletrisSandbox.Entities
{

    [CustomEntity("AletrisSandbox/CherryEntity")]

    public class cherryEntity : Entity
    {
        public readonly bool UnforgivingHitbox;

        public readonly int AnimationRate;

        public readonly bool AnimatedHitbox;

        public readonly bool BigHitbox;

        public Vector2 velocity;

        public Color color;

        private PlayerCollider pc;

        Sprite sprite = GFX.SpriteBank.Create("CherryEntity");

        Sprite bigsprite = GFX.SpriteBank.Create("CherryEntityBig");

        [MethodImpl(MethodImplOptions.NoInlining)]

        public cherryEntity(EntityData data, Vector2 offset) : base(data.Position + offset)
        {

            Depth = -1;
            AnimationRate = data.Int("animationRate", 30);
            UnforgivingHitbox = data.Bool("unforgivingHitbox", false);
            AnimatedHitbox = data.Bool("animatedHitbox", true);
            color = data.HexColor("color", Calc.HexToColor("#FF0000"));
            BigHitbox = data.Bool("bigHitbox", false);

            velocity = data.Vector2("velocityX","velocityY", Vector2.Zero); // for koseihelper

            sprite.Color = color;
            bigsprite.Color = color;

            switch (UnforgivingHitbox)
            {
                case true:
                    Collider = new Circle(UnforgivingHitbox ? 6f : 5f);
                    break;
                case false:
                    Collider = new Circle(UnforgivingHitbox ? 4f : 3f);
                    break;
            }

            Add(BigHitbox ? bigsprite : sprite);
            Add(new LedgeBlocker());
            Add(pc = new PlayerCollider(OnCollide));

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void OnCollide(Player player)
        {
            player.Die((player.Center - Center).SafeNormalize());
        }

        public override void Render()
        {
            base.Render();

            if (velocity != Vector2.Zero)
            {
                Position += velocity; // idk if this works, ask later
            }

        }
    }
}