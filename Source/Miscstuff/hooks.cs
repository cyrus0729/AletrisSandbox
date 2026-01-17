using Celeste.Mod.AletrisSandbox.Entities;
using Microsoft.Xna.Framework;
using Monocle;
// ReSharper disable All

// THANK YOU EVERYBODY IN #CODE_MODDING

namespace Celeste.Mod.AletrisSandbox.GunSupport
{
    internal static class VanillaHooks
    {
        public static void Load()
        {
            On.Celeste.CrystalStaticSpinner.ctor_Vector2_bool_CrystalColor += CrystalSpinnerHook;
            On.Celeste.CrushBlock.ctor_Vector2_float_float_Axes_bool += KevinHook;
            On.Celeste.FlyFeather.ctor_Vector2_bool_bool += FeatherHook;
            On.Celeste.Bumper.ctor_Vector2_Nullable1 += BumperHook;
            On.Celeste.Seeker.ctor_EntityData_Vector2 += SeekerHook;
            On.Celeste.DashBlock.ctor_EntityData_Vector2_EntityID += DashBlockHook;
            On.Celeste.TouchSwitch.ctor_EntityData_Vector2 += TouchSwitchHook;

        }
        public static void Unload()
        {
            On.Celeste.CrystalStaticSpinner.ctor_Vector2_bool_CrystalColor -= CrystalSpinnerHook;
            On.Celeste.CrushBlock.ctor_Vector2_float_float_Axes_bool -= KevinHook;
            On.Celeste.FlyFeather.ctor_Vector2_bool_bool -= FeatherHook;
            On.Celeste.Bumper.ctor_Vector2_Nullable1 -= BumperHook;
            On.Celeste.Seeker.ctor_EntityData_Vector2 -= SeekerHook;
            On.Celeste.DashBlock.ctor_EntityData_Vector2_EntityID -= DashBlockHook;
            On.Celeste.TouchSwitch.ctor_EntityData_Vector2 -= TouchSwitchHook;
        }

        private static void CrystalSpinnerHook(On.Celeste.CrystalStaticSpinner.orig_ctor_Vector2_bool_CrystalColor orig, CrystalStaticSpinner self, Vector2 position, bool attachToSolid, CrystalColor color)
        {
            void CollisionHandler(IWBTGBullet bullet)
            {
                if (!(AletrisSandboxModule.Settings.IWBTOptions.IWBTGGunHitsStuffOverride || AletrisSandboxModule.Session.IWBTGGunHitsStuff)) { return; }
                bullet.Kill();
                if (!(AletrisSandboxModule.Settings.IWBTOptions.IWBTGGunDestroyStuffOverride || AletrisSandboxModule.Session.IWBTGGunDestroysStuff)) { return; }
                self.Destroy();
            }

            orig(self, position, attachToSolid, color);
            self.Add(new BulletCollider(CollisionHandler, self.Collider));
        }

        private static void KevinHook(On.Celeste.CrushBlock.orig_ctor_Vector2_float_float_Axes_bool orig, CrushBlock self, Vector2 position, float width, float height, CrushBlock.Axes axes, bool chillOut)
        {
            void CollisionHandler(IWBTGBullet bullet)
            {
                if (!(AletrisSandboxModule.Settings.IWBTOptions.IWBTGGunHitsStuffOverride || AletrisSandboxModule.Session.IWBTGGunHitsStuff)) { return; }
                self.Attack(-bullet.velocity.SafeNormalize());
                bullet.Kill();
            }

            orig(self, position, width, height, axes, chillOut);

            Collider collidere = new Hitbox(self.Width + 4f, self.Height + 4f, self.Collider.Left - 2f, self.Collider.Top - 2f);
            self.Add(new BulletCollider(CollisionHandler, collidere));
        }

        private static void FeatherHook(On.Celeste.FlyFeather.orig_ctor_Vector2_bool_bool orig, FlyFeather self, Vector2 position, bool shielded, bool singleUse)
        {
            void CollisionHandler(IWBTGBullet bullet)
            {
                if (!(AletrisSandboxModule.Settings.IWBTOptions.IWBTGGunHitsStuffOverride || AletrisSandboxModule.Session.IWBTGGunHitsStuff)) { return; }
                if (!(self.shielded))
                {
                    self.OnPlayer(bullet.owner);
                    bullet.Kill();
                }
                else
                {
                    self.moveWiggle.Start();
                    self.shieldRadiusWiggle.Start();
                    self.moveWiggleDir = (self.Center - bullet.Position).SafeNormalize(Vector2.UnitY);
                    Audio.Play("event:/game/06_reflection/feather_bubble_bounce", bullet.Position);
                    Input.Rumble(RumbleStrength.Medium, RumbleLength.Medium);
                    bullet.velocity = -bullet.velocity;
                }
            }
            orig(self, position, shielded, singleUse);
            self.Add(new BulletCollider(CollisionHandler, self.Collider));
        }

        private static void BumperHook(On.Celeste.Bumper.orig_ctor_Vector2_Nullable1 orig, Bumper self, Vector2 position, Vector2? node)
        {
            void CollisionHandler(IWBTGBullet bullet)
            {
                if (!(AletrisSandboxModule.Settings.IWBTOptions.IWBTGGunHitsStuffOverride || AletrisSandboxModule.Session.IWBTGGunHitsStuff)) { return; }
                if (self.fireMode)
                {
                    Vector2 vector = (bullet.Position - self.Center).SafeNormalize();
                    self.hitDir = -vector;
                    self.hitWiggler.Start();
                    Audio.Play("event:/game/09_core/hotpinball_activate", self.Position);
                    bullet.Kill();
                    self.SceneAs<Level>().Particles.Emit(Bumper.P_FireHit, 12, self.Center + vector * 12f, Vector2.One * 3f, vector.Angle());
                }
                else if (self.respawnTimer <= 0f)
                {
                    Audio.Play("event:/game/06_reflection/pinballbumper_hit", bullet.Position);
                    self.respawnTimer = 0.6f;
                    Vector2 vector = (self.Center - bullet.Position).SafeNormalize(-Vector2.UnitY);
                    self.sprite.Play("hit", restart: true);
                    self.spriteEvil.Play("hit", restart: true);
                    self.light.Visible = false;
                    self.bloom.Visible = false;
                    self.SceneAs<Level>().DirectionalShake(vector, 0.15f);
                    self.SceneAs<Level>().Displacement.AddBurst(self.Center, 0.3f, 8f, 32f, 0.8f);
                    self.SceneAs<Level>().Particles.Emit(Bumper.P_Launch, 12, self.Center + vector * 12f, Vector2.One * 3f, vector.Angle());
                    bullet.velocity = -vector * bullet.velocity.Length();
                }

            }

            orig(self, position, node);
            self.Add(new BulletCollider(CollisionHandler, self.Collider));

        }

        private static void SeekerHook(On.Celeste.Seeker.orig_ctor_EntityData_Vector2 orig, Seeker self, EntityData data, Vector2 offset)
        {
            void CollisionHandler(IWBTGBullet bullet)
            {
                if (!(AletrisSandboxModule.Settings.IWBTOptions.IWBTGGunHitsStuffOverride || AletrisSandboxModule.Session.IWBTGGunHitsStuff)) { return; }
                self.Speed = (self.Center - bullet.Center).SafeNormalize(2000f);
                self.State.State = 6;
                self.sprite.Scale = new Vector2(1.4f, 0.6f);
                self.SceneAs<Level>().Particles.Emit(Seeker.P_Stomp, 8, self.Center - Vector2.UnitY * 5f, new Vector2(6f, 3f));
                bullet.Kill();
            }

            orig(self, data, offset);
            self.Add(new BulletCollider(CollisionHandler, self.Collider));
        }

        private static void DashBlockHook(On.Celeste.DashBlock.orig_ctor_EntityData_Vector2_EntityID orig, DashBlock self, EntityData data, Vector2 offset, EntityID id)
        {

            void CollisionHandler(IWBTGBullet bullet)
            {
                Logger.Log(LogLevel.Info,nameof(AletrisSandboxModule),"aaa");
                bullet.Kill();
                if (!(AletrisSandboxModule.Settings.IWBTOptions.IWBTGGunHitsStuffOverride || AletrisSandboxModule.Session.IWBTGGunHitsStuff)) { return; }
                self.Break(bullet.Center, bullet.velocity, true, true);
            }
            orig(self, data, offset, id);
            self.Add(new BulletCollider(CollisionHandler, self.Collider));
        }

        private static void TouchSwitchHook(On.Celeste.TouchSwitch.orig_ctor_EntityData_Vector2 orig, TouchSwitch self, EntityData data, Vector2 offset)
        {
            void CollisionHandler(IWBTGBullet bullet)
            {
                Logger.Log(LogLevel.Info, nameof(AletrisSandboxModule), "aaa");
                bullet.Kill();

                if (!(AletrisSandboxModule.Settings.IWBTOptions.IWBTGGunHitsStuffOverride || AletrisSandboxModule.Session.IWBTGGunHitsStuff)) { return; }

            }

            orig(self, data, offset);
            self.Add(new BulletCollider(CollisionHandler, self.Collider));
        }

    }
}