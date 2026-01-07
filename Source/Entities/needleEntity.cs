using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System.Runtime.CompilerServices;

namespace Celeste.Mod.AletrisSandbox.Entities
{
    [CustomEntity("AletrisSandbox/NeedleUp = BigUp","AletrisSandboxLite/NeedleUp = BigUp",
                "AletrisSandbox/NeedleDown = BigDown", "AletrisSandboxLite/NeedleDown = BigDown",
                "AletrisSandbox/NeedleLeft = BigLeft","AletrisSandboxLite/NeedleLeft = BigLeft",
                "AletrisSandbox/NeedleRight = BigRight","AletrisSandboxLite/NeedleRight = BigRight",
                "AletrisSandbox/MiniNeedleUp = MiniUp","AletrisSandboxLite/MiniNeedleUp = MiniUp",
                "AletrisSandbox/MiniNeedleDown = MiniDown","AletrisSandboxLite/MiniNeedleDown = MiniDown",
                "AletrisSandbox/MiniNeedleLeft = MiniLeft","AletrisSandboxLite/MiniNeedleLeft = MiniLeft",
                "AletrisSandbox/MiniNeedleRight = MiniRight","AletrisSandboxLite/MiniNeedleRight = MiniRight")]
    public class NeedleEntity : Entity
    {
        public enum HitboxType { NeedleHelper, Forgiving, Unforgiving };
        public bool Attached; //figure out how attaching works later
        private PlayerCollider pc;

        private EntityData data;
        private Vector2 offset;
        private bool isMini;
        private bool kill;

        public NeedleEntity(EntityData data, Vector2 offset, ColliderList colliderList, Sprite sprite, float rotation) : base(data.Position + offset)
        {
            isMini = data.Bool("isMini", false);
            kill = data.Bool("kill", true);
            this.data = data;
            this.offset = offset;
            this.Collider = colliderList;
            Add(pc = new PlayerCollider(OnCollide));
            Add(sprite);
            sprite.Rotation = rotation;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void OnCollide(Player player)
        {
            if (OnCollide != null && kill)
            {
                player.Die((player.Center - Center).SafeNormalize());
            }
        }

        public static Entity BigUp(Level level, LevelData leveldata, Vector2 offset, EntityData data)
        {
            HitboxType hitboxes = data.Enum("hitbox", HitboxType.NeedleHelper);
            switch (hitboxes)
            {
                default:
                    return new NeedleEntity(data, offset, new ColliderList([new Hitbox(16f, 3f, -8f, -8f), new Hitbox(4f, 2f, -2f, 2f)]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 0f);
                case HitboxType.NeedleHelper:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(16f, 3f, -8f, 5f),
                        new Hitbox(12f, 2f, -6f, 4f),
                        new Hitbox(10f, 2f, -5f, 2f),
                        new Hitbox(8f, 2f, -4f, 0f),
                        new Hitbox(6f, 2f, -3f, -2f),
                        new Hitbox(4f, 2f, -2f, -4f),
                        new Hitbox(2f, 2f, -1f, -6f),
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 0f);

                case HitboxType.Forgiving:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(14f, 2f, -7f, 6f),
                        new Hitbox(12f, 2f, -6f, 4f),
                        new Hitbox(10f, 2f, -5f, 2f),
                        new Hitbox(8f, 2f, -4f, 0f),
                        new Hitbox(6f, 2f, -3f, -2f),
                        new Hitbox(4f, 2f, -2f, -4f),
                        new Hitbox(2f, 2f, -1f, -6f),
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 0f);

                case HitboxType.Unforgiving:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(16f, 2f, -8f, 6f),
                        new Hitbox(14f, 2f, -7f, 4f),
                        new Hitbox(12f, 2f, -6f, 2f),
                        new Hitbox(10f, 2f, -5f, 0f),
                        new Hitbox(8f, 2f, -4f, -2f),
                        new Hitbox(6f, 2f, -3f, -4f),
                        new Hitbox(4f, 2f, -2f, -6f),
                        new Hitbox(2f, 2f, -1f, -8f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 0f);
            }
        }
        public static Entity BigDown(Level level, LevelData leveldata, Vector2 offset, EntityData data)
        {
            HitboxType hitboxes = data.Enum("hitbox", HitboxType.NeedleHelper);
            switch (hitboxes)
            {
                default:
                    return new NeedleEntity(data, offset, new ColliderList([new Hitbox(16f, 3f, -8f, -8f), new Hitbox(4f, 2f, -2f, 2f)]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 0f);
                case HitboxType.NeedleHelper:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(16f, 3f, -8f, -8f),
                        new Hitbox(12f, 2f, -6f, -6f),
                        new Hitbox(10f, 2f, -5f, -4f),
                        new Hitbox(8f, 2f, -4f, -2f),
                        new Hitbox(6f, 2f, -3f, -0f),
                        new Hitbox(4f, 2f, -2f, 2f),
                        new Hitbox(2f, 2f, -1f, 4f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 180f.ToRad());

                case HitboxType.Forgiving:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(14f, 2f, -7f, -8f),
                        new Hitbox(12f, 2f, -6f, -6f),
                        new Hitbox(10f, 2f, -5f, -4f),
                        new Hitbox(8f, 2f, -4f, -2f),
                        new Hitbox(6f, 2f, -3f, -0f),
                        new Hitbox(4f, 2f, -2f, 2f),
                        new Hitbox(2f, 2f, -1f, 4f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 180f.ToRad());

                case HitboxType.Unforgiving:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(16f, 2f, -8f, -8f),
                        new Hitbox(14f, 2f, -7f, -6f),
                        new Hitbox(12f, 2f, -6f, -4f),
                        new Hitbox(10f, 2f, -5f, -2f),
                        new Hitbox(8f, 2f, -4f, -0f),
                        new Hitbox(6f, 2f, -3f, 2f),
                        new Hitbox(4f, 2f, -2f, 4f),
                        new Hitbox(2f, 2f, -1f, 6f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 180f.ToRad());
            }
        }
        public static Entity BigLeft(Level level, LevelData leveldata, Vector2 offset, EntityData data)
        {
            HitboxType hitboxes = data.Enum("hitbox", HitboxType.NeedleHelper);
            switch (hitboxes)
            {
                default:
                    return new NeedleEntity(data, offset, new ColliderList([new Hitbox(16f, 3f, -8f, -8f), new Hitbox(4f, 2f, -2f, 2f)]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 0f);
                case HitboxType.NeedleHelper:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(3f, 16f, 5f, -8f),
                        new Hitbox(2f, 12f, 4f, -6f),
                        new Hitbox(2f, 10f, 2f, -5f),
                        new Hitbox(2f, 8f, 0f, -4f),
                        new Hitbox(2f, 6f, -2f, -3f),
                        new Hitbox(2f, 4f, -4f, -2f),
                        new Hitbox(2f, 2f, -6f, -1f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 270f.ToRad());

                case HitboxType.Forgiving:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(2f, 14f, 6f, -7f),
                        new Hitbox(2f, 12f, 4f, -6f),
                        new Hitbox(2f, 10f, 2f, -5f),
                        new Hitbox(2f, 8f, 0f, -4f),
                        new Hitbox(2f, 6f, -2f, -3f),
                        new Hitbox(2f, 4f, -4f, -2f),
                        new Hitbox(2f, 2f, -6f, -1f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 270f.ToRad());

                case HitboxType.Unforgiving:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(2f, 16f, 6f, -8f),
                        new Hitbox(2f, 14f, 4f, -7f),
                        new Hitbox(2f, 12f, 2f, -6f),
                        new Hitbox(2f, 10f, 0f, -5f),
                        new Hitbox(2f, 8f, -2f, -4f),
                        new Hitbox(2f, 6f, -4f, -3f),
                        new Hitbox(2f, 4f, -6f, -2f),
                        new Hitbox(2f, 2f, -8f, -1f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 270f.ToRad());
            }
        }
        public static Entity BigRight(Level level, LevelData leveldata, Vector2 offset, EntityData data)
        {
            HitboxType hitboxes = data.Enum("hitbox", HitboxType.NeedleHelper);
            switch (hitboxes)
            {
                default:
                    return new NeedleEntity(data, offset, new ColliderList([new Hitbox(16f, 3f, -8f, -8f), new Hitbox(4f, 2f, -2f, 2f)]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 0f);
                case HitboxType.NeedleHelper:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(3f, 16f, -8f, -8f),
                        new Hitbox(2f, 12f, -6f, -6f),
                        new Hitbox(2f, 10f, -4f, -5f),
                        new Hitbox(2f, 8f, -2f, -4f),
                        new Hitbox(2f, 6f, -0f, -3f),
                        new Hitbox(2f, 4f, 2f, -2f),
                        new Hitbox(2f, 2f, 4f, -1f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 90f.ToRad());

                case HitboxType.Forgiving:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(2f, 14f, -8f, -7f),
                        new Hitbox(2f, 12f, -6f, -6f),
                        new Hitbox(2f, 10f, -4f, -5f),
                        new Hitbox(2f, 8f, -2f, -4f),
                        new Hitbox(2f, 6f, -0f, -3f),
                        new Hitbox(2f, 4f, 2f, -2f),
                        new Hitbox(2f, 2f, 4f, -1f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 90f.ToRad());

                case HitboxType.Unforgiving:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(2f, 16f, -8f, -8f),
                        new Hitbox(2f, 14f, -6f, -7f),
                        new Hitbox(2f, 12f, -4f, -6f),
                        new Hitbox(2f, 10f, -2f, -5f),
                        new Hitbox(2f, 8f, -0f, -4f),
                        new Hitbox(2f, 6f, 2f, -3f),
                        new Hitbox(2f, 4f, 4f, -2f),
                        new Hitbox(2f, 2f, 6f, -1f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 90f.ToRad());
            }
        }

        public static Entity MiniUp(Level level, LevelData leveldata, Vector2 offset, EntityData data)
        {
            HitboxType hitboxes = data.Enum("hitbox", HitboxType.NeedleHelper);
            switch (hitboxes)
            {
                default:
                    return new NeedleEntity(data, offset, new ColliderList([new Hitbox(16f, 3f, -8f, -8f), new Hitbox(4f, 2f, -2f, 2f)]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 0f);
                case HitboxType.NeedleHelper:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(8f, 3f, -4f, 1f),
                        new Hitbox(4f, 2f, -2f, 0f),
                        new Hitbox(2f, 2f, -1f, -2f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "MiniNeedleEntity")), 0f);

                case HitboxType.Forgiving:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(6f, 2f, -3f, 2f),
                        new Hitbox(4f, 2f, -2f, 0f),
                        new Hitbox(2f, 2f, -1f, -2f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "MiniNeedleEntity")), 0f);

                case HitboxType.Unforgiving:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(8f, 2f, -4f, 2f),
                        new Hitbox(6f, 2f, -3f, 0f),
                        new Hitbox(4f, 2f, -2f, -2f),
                        new Hitbox(2f, 2f, -1f, -4f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "MiniNeedleEntity")), 0f);
            }
        }
        public static Entity MiniDown(Level level, LevelData leveldata, Vector2 offset, EntityData data)
        {
            HitboxType hitboxes = data.Enum("hitbox", HitboxType.NeedleHelper);
            switch (hitboxes)
            {
                default:
                    return new NeedleEntity(data, offset, new ColliderList([new Hitbox(16f, 3f, -8f, -8f), new Hitbox(4f, 2f, -2f, 2f)]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 0f);
                case HitboxType.NeedleHelper:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(8f, 2f, -4f, -4f),
                        new Hitbox(4f, 2f, -2f, -2f),
                        new Hitbox(2f, 2f, -1f, 0f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "MiniNeedleEntity")), 180f.ToRad());

                case HitboxType.Forgiving:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(6f, 2f, -3f, -4f),
                        new Hitbox(4f, 2f, -2f, -2f),
                        new Hitbox(2f, 2f, -1f, 0f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "MiniNeedleEntity")), 180f.ToRad());

                case HitboxType.Unforgiving:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(8f, 2f, -4f, -4f),
                        new Hitbox(6f, 2f, -3f, -2f),
                        new Hitbox(4f, 2f, -2f, 0f),
                        new Hitbox(2f, 2f, -1f, 2f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "MiniNeedleEntity")), 180f.ToRad());
            }
        }
        public static Entity MiniLeft(Level level, LevelData leveldata, Vector2 offset, EntityData data)
        {
            HitboxType hitboxes = data.Enum("hitbox", HitboxType.NeedleHelper);
            switch (hitboxes)
            {
                default:
                    return new NeedleEntity(data, offset, new ColliderList([new Hitbox(16f, 3f, -8f, -8f), new Hitbox(4f, 2f, -2f, 2f)]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 0f);
                case HitboxType.NeedleHelper:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(2f, 8f, 2f, -4f),
                        new Hitbox(2f, 4f, 0f, -2f),
                        new Hitbox(2f, 2f, -2f, -1f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "MiniNeedleEntity")), 270f.ToRad());

                case HitboxType.Forgiving:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(2f, 6f, 2f, -3f),
                        new Hitbox(2f, 4f, 0f, -2f),
                        new Hitbox(2f, 2f, -2f, -1f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "MiniNeedleEntity")), 270f.ToRad());

                case HitboxType.Unforgiving:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(2f, 8f, 2f, -4f),
                        new Hitbox(2f, 6f, 0f, -3f),
                        new Hitbox(2f, 4f, -2f, -2f),
                        new Hitbox(2f, 2f, -4f, -1f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "MiniNeedleEntity")), 270f.ToRad());
            }
        }
        public static Entity MiniRight(Level level, LevelData leveldata, Vector2 offset, EntityData data)
        {
            HitboxType hitboxes = data.Enum("hitbox", HitboxType.NeedleHelper);
            switch (hitboxes)
            {
                default:
                    return new NeedleEntity(data, offset, new ColliderList([new Hitbox(16f, 3f, -8f, -8f), new Hitbox(4f, 2f, -2f, 2f)]), GFX.SpriteBank.Create(data.Attr("Sprite", "NeedleEntity")), 0f);
                case HitboxType.NeedleHelper:
                    return new NeedleEntity(data, offset, new ColliderList([
                    new Hitbox(2f, 8f, -4f, -4f),
                        new Hitbox(2f, 4f, -2f, -2f),
                        new Hitbox(2f, 2f, 0f, -1f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "MiniNeedleEntity")), 90f.ToRad());

                case HitboxType.Forgiving:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(2f, 6f, -4f, -3f),
                        new Hitbox(2f, 4f, -2f, -2f),
                        new Hitbox(2f, 2f, 0f, -1f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "MiniNeedleEntity")), 90f.ToRad());

                case HitboxType.Unforgiving:
                    return new NeedleEntity(data, offset, new ColliderList([
                        new Hitbox(2f, 8f, -4f, -4f),
                        new Hitbox(2f, 6f, -2f, -3f),
                        new Hitbox(2f, 4f, 0f, -2f),
                        new Hitbox(2f, 2f, 2f, -1f)
                    ]), GFX.SpriteBank.Create(data.Attr("Sprite", "MiniNeedleEntity")), 90f.ToRad());
            }
        }

    }
}