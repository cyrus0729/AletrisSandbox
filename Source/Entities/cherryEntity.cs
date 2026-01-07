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

        public Color color;

        private PlayerCollider pc;

        Sprite sprite = GFX.SpriteBank.Create("CherryEntity");

        Sprite bigsprite = GFX.SpriteBank.Create("CherryEntityBig");

        [MethodImpl(MethodImplOptions.NoInlining)]

        public cherryEntity(EntityData data, Vector2 offset) : base(data.Position + offset)
        {

            base.Depth = -1;
            AnimationRate = data.Int("animationRate", 30);
            UnforgivingHitbox = data.Bool("unforgivingHitbox", false);
            AnimatedHitbox = data.Bool("animatedHitbox", true);
            color = data.HexColor("color", Calc.HexToColor("#FF0000"));
            BigHitbox = data.Bool("bigHitbox", false);

            sprite.Color = color;
            bigsprite.Color = color;

            switch (UnforgivingHitbox)
            {
                case true:
                    base.Collider = new Circle(UnforgivingHitbox ? 6f : 5f);
                    break;
                case false:
                    base.Collider = new Circle(UnforgivingHitbox ? 4f : 3f);
                    break;
            }

            Add(BigHitbox ? bigsprite : sprite);
            Add(new LedgeBlocker());
            Add(pc = new PlayerCollider(OnCollide));

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void OnCollide(Player player)
        {
            if (OnCollide != null)
            {
                player.Die((player.Center - base.Center).SafeNormalize());
            }
        }

        public override void Update()
        {

        }

    }
}