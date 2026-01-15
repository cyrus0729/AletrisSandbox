using System;
using System.Collections.Generic;
using System.Linq;
using AsmResolver.PE.DotNet.StrongName;
using Celeste.Mod.AletrisSandbox.Triggers;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.AletrisSandbox.Entities
{
    [CustomEntity("AletrisSandbox/EntityHPController")]
    public class EntityHPController : Hawa.Wrapper
    {

        public int HP;
        public int dmg;
        public bool IsBoss;
        public string OnlyType;
        public string flagOnKillAll;
        public string HitSound;
        public string DeathSound;
        public int IFrames;

        public int maxHP;

        Dictionary<Entity, stupidFUcker> D = new();

        public class stupidFUcker
        {
            public BulletCollider bulletCollider { get; set; }
            public bool IsDead { get; set; }
            public int HP { get; set; }
            public int MaxHP { get; set; }
            public int Dmg { get; set; }
            public int IFrames { get; set; }
            public int IFrameCount { get; set; }
        }

        public EntityHPController(EntityData data, Vector2 offset)
            : base(data.Position + offset)
        {
            HP = data.Int("HP",1); // total hp, floats are unnessceary just use bigger integers
            dmg = data.Int("DamageIncrement", 1); // how much damage to deal
            IsBoss = data.Bool("Boss"); // whether to use the bigass healthbar
            flagOnKillAll = data.Attr("flagOnKillAll"); // flag to set on every entity being dead
            OnlyType = data.Attr("OnlyType");
            HitSound = data.Attr("HitSound");
            DeathSound = data.Attr("DeathSound");
            IFrames = data.Int("IFrames", 0);

            // maybe add seperate healthbar colors and node bullshit so its like destination???
        }

        public override void Render()
        {
            base.Render();

            foreach (Entity e in D.Keys)
            {

                if (D.TryGetValue(e, out stupidFUcker data) && data != null && !data.IsDead)
                {
                    data.IFrames -= 1;
                    ActiveFont.DrawOutline(data.HP + "/" + data.MaxHP, e.Position, new Vector2(0.5f, 2f), new Vector2(0.25f, 0.25f), Color.White, 2f, Color.Black);
                }
            }
        }

        public override void Awake(Scene scene)
        {
            base.Awake(scene); ;
            List<Entity> entities = FindTargets(Position, new Vector2[0], Vector2.Zero, false, OnlyType);

            Logger.Log(LogLevel.Info,"ALetrisSandbox", string.Join("\t", entities));

            foreach (Entity e in entities)
            {
                // Capture the current entity in a local variable to avoid closure issues
                Entity currentEntity = e;

                void collisionHandler(IWBTGBullet bullet)
                {
                    if (currentEntity == null)
                    {
                        return;
                    }

                    bullet.Kill();

                    if (D.TryGetValue(currentEntity, out stupidFUcker data))
                    {
                        if (data.IFrames > 0)  return;

                        if (data.IFrameCount > 0) data.IFrames = data.IFrameCount;

                        data.HP -= data.Dmg;
                        Audio.Play(HitSound);

                        if (data.HP <= 0)
                        {
                            D[currentEntity].IsDead = true;
                            D[currentEntity] = null;

                            if (D.All(kv => kv.Value == null))
                            {
                                SceneAs<Level>().Session.SetFlag(flagOnKillAll);
                            }
                            Audio.Play(DeathSound);
                            Logger.Log(LogLevel.Info, "AletrisSandbox", $"deleted {currentEntity}");
                            currentEntity.RemoveSelf();
                        }

                    } else {
                        Logger.Log(LogLevel.Warn, "AletrisSandbox", $"data for id {currentEntity.SourceId} is null");
                    }


                }

                BulletCollider h = new BulletCollider(collisionHandler, e.Collider);
                D[e] = new() { bulletCollider = h, IsDead = false, HP = HP, MaxHP = HP, Dmg = dmg, IFrameCount = IFrames, IFrames = 0 };
                e.Add(h);
                Logger.Log(LogLevel.Info, "ALetrisSandbox", string.Join("\t", D));
            }
        }
    }
}