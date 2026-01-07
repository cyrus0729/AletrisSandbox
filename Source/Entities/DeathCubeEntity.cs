using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System.Runtime.CompilerServices;
using static Celeste.Mod.AletrisSandbox.Entities.NeedleEntity;
using Directions = Celeste.MoveBlock.Directions;

namespace Celeste.Mod.AletrisSandbox.Entities
{
    [CustomEntity("AletrisSandbox/DeathCubeUp = CubeUp",
            "AletrisSandbox/DeathCubeDown = CubeDown",
            "AletrisSandbox/DeathCubeLeft = CubeLeft",
            "AletrisSandbox/DeathCubeRight = CubeRight")]
    public class DeathCubeEntity : Entity
    {
        private PlayerCollider pc;
        private Solid solid;
        public readonly Directions direction;
        Sprite sprite = GFX.SpriteBank.Create("DeathCubeEntity");
        private EntityData data;
        private Vector2 offset;

        public DeathCubeEntity(EntityData data, Vector2 offset, Hitbox collider, Sprite sprite, float rotation) : base(data.Position + offset)
        {
            this.data = data;
            this.offset = offset;
            this.Collider = collider;
            Add(pc = new PlayerCollider(OnCollide));
            Add(sprite);
            sprite.Rotation = rotation;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void OnCollide(Player player)
        {
            if (OnCollide != null)
            {
                player.Die((player.Center - Center).SafeNormalize());
            }
        }

        public static Entity CubeUp(Level level, LevelData leveldata, Vector2 offset, EntityData data)
        {
            bool hitboxes = data.Bool("UnforgivingHitbox", false);
            switch (hitboxes)
            {
                case true:
                    return new DeathCubeEntity(
                        data, offset, new Hitbox(16, 16, -8, -8), GFX.SpriteBank.Create(data.Attr("Sprite", "DeathCubeEntity")), 0f);
                case false:
                    return new DeathCubeEntity(
                        data, offset, new Hitbox(14, 14, -7, -7), GFX.SpriteBank.Create(data.Attr("Sprite", "DeathCubeEntity")), 0f);
            }
        }

        public static Entity CubeDown(Level level, LevelData leveldata, Vector2 offset, EntityData data)
        {
            bool hitboxes = data.Bool("UnforgivingHitbox", false);
            switch (hitboxes)
            {
                case true:
                    return new DeathCubeEntity(
                        data, offset, new Hitbox(16, 16, -8, -8), GFX.SpriteBank.Create(data.Attr("Sprite", "DeathCubeEntity")), 180f.ToRad());
                case false:
                    return new DeathCubeEntity(
                        data, offset, new Hitbox(14, 14, -7, -7), GFX.SpriteBank.Create(data.Attr("Sprite", "DeathCubeEntity")), 180f.ToRad());
            }
        }

        public static Entity CubeLeft(Level level, LevelData leveldata, Vector2 offset, EntityData data)
        {
            bool hitboxes = data.Bool("UnforgivingHitbox", false);
            switch (hitboxes)
            {
                case true:
                    return new DeathCubeEntity(
                        data, offset, new Hitbox(16, 16, -8, -8), GFX.SpriteBank.Create(data.Attr("Sprite", "DeathCubeEntity")), 270f.ToRad());
                case false:
                    return new DeathCubeEntity(
                        data, offset, new Hitbox(14, 14, -7, -7), GFX.SpriteBank.Create(data.Attr("Sprite", "DeathCubeEntity")), 270f.ToRad());
            }
        }

        public static Entity CubeRight(Level level, LevelData leveldata, Vector2 offset, EntityData data)
        {
            bool hitboxes = data.Bool("UnforgivingHitbox", false);
            switch (hitboxes)
            {
                case true:
                    return new DeathCubeEntity(
                        data, offset, new Hitbox(16, 16, -8, -8), GFX.SpriteBank.Create(data.Attr("Sprite", "DeathCubeEntity")), 90f.ToRad());
                case false:
                    return new DeathCubeEntity(
                        data, offset, new Hitbox(14, 14, -7, -7), GFX.SpriteBank.Create(data.Attr("Sprite", "DeathCubeEntity")), 90f.ToRad());
            }
        }

        public override void Added(Scene scene)
        {
            base.Added(scene);
            bool hitboxes = data.Bool("UnforgivingHitbox", false);
            switch (hitboxes)
            {
                case true:
                    scene.Add(solid = new Solid(Position + new Vector2(-7, -7), base.Width - 2f, base.Height - 2f, false));
                    Add(new ClimbBlocker(edge: true));
                    break;
                case false:
                    scene.Add(solid = new Solid(Position + new Vector2(-6, -6), base.Width - 2f, base.Height - 2f, false));
                    Add(new ClimbBlocker(edge: true));
                    break;
            }
        }

    }
}
