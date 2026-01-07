using System;
using Celeste;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace AletrisSandbox
{

    public class Hole : Component
    {
        public Hole(bool active, bool visible)
            : base(active, visible)
        {
        }
        
        public bool Check(Player player)
        {
            Collider collider = base.Entity.Collider;
            bool flag = this.holeCollider != null;
            if (flag)
            {
                base.Entity.Collider = this.holeCollider;
            }
            bool flag2 = player.CollideCheck(base.Entity);
            base.Entity.Collider = collider;
            return flag2;
        }
        
        public Circle holeCollider;
    }
    
    [CustomEntity("AletrisSandbox/ModifiedBlackHole")]
    public class BlackHole : Entity
    {

        private Player player;
        private Hole hole;
        private float speedModifier;
        private float forceModifier;
        private float auraRadius;
        private float holeRadius;

        public BlackHole(EntityData data, Vector2 offset)
            : base(data.Position + offset)
        {
            this.speedModifier = data.Float("SpeedModifier", 1.02f);
            this.forceModifier = data.Float("ForceModifier", 0.8f);
            this.auraRadius = data.Float("auraRadius", 48f);
            this.holeRadius = data.Float("holeRadius", 8f);
            Sprite aura = GFX.SpriteBank.Create("BHAura");
            Sprite blackhole = GFX.SpriteBank.Create("BlackHole");
            aura.Scale = new Vector2(this.auraRadius / 48f, this.auraRadius/ 48f);
            blackhole.Scale = new Vector2(this.holeRadius / 8f, this.holeRadius/8f);
            base.Add(blackhole);
            base.Add(aura);
            aura.Origin.X = aura.Width / 2f;
            aura.Origin.Y = aura.Height / 2f;
            blackhole.Origin.X = blackhole.Height/ 2f;
            blackhole.Origin.Y = blackhole.Width / 2f;
            
            base.Collider = new Circle(this.auraRadius, 0f, 0f);
            base.Add(this.hole = new Hole(true, true));
            this.hole.holeCollider = new Circle(this.holeRadius, 0f, 0f);
        }

        public override void Update()
        {
            base.Update();
            bool flag = base.CollideCheck<Player>();
            if (flag)
            {
                this.player = base.CollideFirst<Player>();
                if (this.player == null) { return;}
                {
                    this.Drag(this.player);
                    this.HoleKill(this.player);
                }
            }
        }

        private void HoleKill(Player player)
        {
            bool flag = this.hole.Check(player);
            if (flag)
            {
                bool invincible = SaveData.Instance.Assists.Invincible;
                if (invincible)
                {
                    player.Play("event:/game/general/assist_screenbottom", null, 0f);
                    player.Bounce(base.Top);
                }
                else
                {
                    player.Die(Vector2.Zero, false, true);
                }
            }
        }

        private void Drag(Player player)
        {
            Vector2 vector = (this.Position - player.Position) * this.forceModifier;
            player.Speed *= this.speedModifier;
            player.Speed += vector;
        }

    }
}