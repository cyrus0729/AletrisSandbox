using System.Security.Cryptography.X509Certificates;
using Celeste.Mod.AletrisSandbox.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using YamlDotNet.Serialization;

// ReSharper disable All

// THANK YOU EVERYBODY IN #CODE_MODDING

namespace Celeste.Mod.AletrisSandbox.GunSupport
{
    internal static class VanillaHooks
    {

        public static bool CanDoShit(Actor owner)
        {
            return owner != null && owner.Scene != null && owner.Scene.Tracker != null;
        }

        public static void Load()
        {
            On.Celeste.CrystalStaticSpinner.ctor_Vector2_bool_CrystalColor += CrystalSpinnerHook;
            On.Celeste.CrushBlock.ctor_Vector2_float_float_Axes_bool += KevinHook;
            On.Celeste.FlyFeather.ctor_Vector2_bool_bool += FeatherHook;
            On.Celeste.Bumper.ctor_Vector2_Nullable1 += BumperHook;
            On.Celeste.Seeker.ctor_EntityData_Vector2 += SeekerHook;
            On.Celeste.TouchSwitch.ctor_EntityData_Vector2 += TouchSwitchHook;
            On.Celeste.Refill.ctor_EntityData_Vector2 += RefillHook;
            On.Celeste.Glider.ctor_EntityData_Vector2 += JellyHook;
            On.Celeste.HeartGem.ctor_EntityData_Vector2 += HeartHook;
        }

        public static void Unload()
        {
            On.Celeste.CrystalStaticSpinner.ctor_Vector2_bool_CrystalColor -= CrystalSpinnerHook;
            On.Celeste.CrushBlock.ctor_Vector2_float_float_Axes_bool -= KevinHook;
            On.Celeste.FlyFeather.ctor_Vector2_bool_bool -= FeatherHook;
            On.Celeste.Bumper.ctor_Vector2_Nullable1 -= BumperHook;
            On.Celeste.Seeker.ctor_EntityData_Vector2 -= SeekerHook;
            On.Celeste.TouchSwitch.ctor_EntityData_Vector2 -= TouchSwitchHook;
            On.Celeste.Refill.ctor_EntityData_Vector2 -= RefillHook;
            On.Celeste.Glider.ctor_EntityData_Vector2 -= JellyHook;
            On.Celeste.HeartGem.ctor_EntityData_Vector2 -= HeartHook;
        }

        private static void CrystalSpinnerHook(On.Celeste.CrystalStaticSpinner.orig_ctor_Vector2_bool_CrystalColor orig,
                                               CrystalStaticSpinner self,
                                               Vector2 position,
                                               bool attachToSolid,
                                               CrystalColor color)
        {
            void CollisionHandler(IWBTGBullet bullet)
            {
                if (!CanDoShit(bullet.owner)) { return; }

                if (!(AletrisSandboxModule.Settings.IWBTOptions.IWBTGGunHitsStuffOverride || AletrisSandboxModule.Session.IWBTGGunHitsStuff)) { return; }
                bullet.Kill();

                if (!(AletrisSandboxModule.Settings.IWBTOptions.IWBTGGunDestroyStuffOverride || AletrisSandboxModule.Session.IWBTGGunDestroysStuff)) { return; }
                self.Destroy();
            }

            orig(self, position, attachToSolid, color);
            self.Add(new BulletCollider(CollisionHandler, self.Collider));
        }

        private static void KevinHook(On.Celeste.CrushBlock.orig_ctor_Vector2_float_float_Axes_bool orig,
                                      CrushBlock self,
                                      Vector2 position,
                                      float width,
                                      float height,
                                      CrushBlock.Axes axes,
                                      bool chillOut)
        {
            void CollisionHandler(IWBTGBullet bullet)
            {
                if (!CanDoShit(bullet.owner)) { return; }

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
                if (!CanDoShit(bullet.owner)) { return; }

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
                if (!CanDoShit(bullet.owner)) { return; }

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
            self.Add(new BulletCollider(CollisionHandler));

        }

        private static void SeekerHook(On.Celeste.Seeker.orig_ctor_EntityData_Vector2 orig, Seeker self, EntityData data, Vector2 offset)
        {

            void CollisionHandler(IWBTGBullet bullet)
            {
                if (!CanDoShit(bullet.owner)) { return; }

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

        private static void TouchSwitchHook(On.Celeste.TouchSwitch.orig_ctor_EntityData_Vector2 orig, TouchSwitch self, EntityData data, Vector2 offset)
        {
            void CollisionHandler(IWBTGBullet bullet)
            {
                if (!CanDoShit(bullet.owner)) { return; }

                if (!(AletrisSandboxModule.Settings.IWBTOptions.IWBTGGunHitsStuffOverride || AletrisSandboxModule.Session.IWBTGGunHitsStuff)) { return; }
                self.TurnOn();
            }

            orig(self, data, offset);
            self.Add(new BulletCollider(CollisionHandler, self.Collider));
        }

        private static void RefillHook(On.Celeste.Refill.orig_ctor_EntityData_Vector2 orig, Refill self, EntityData data, Vector2 offset)
        {
            void CollisionHandler(IWBTGBullet bullet)
            {
                if (!CanDoShit(bullet.owner)) { return; }

                if (bullet.owner == null)
                    return;
                if (!bullet.owner.UseRefill(self.twoDashes))
                    return;

                if (!(AletrisSandboxModule.Settings.IWBTOptions.IWBTGGunHitsStuffOverride || AletrisSandboxModule.Session.IWBTGGunHitsStuff)) { return; }

                Audio.Play(self.twoDashes ? "event:/new_content/game/10_farewell/pinkdiamond_touch" : "event:/game/general/diamond_touch", self.Position);
                Input.Rumble(RumbleStrength.Medium, RumbleLength.Medium);
                self.Collidable = false;
                Celeste.Freeze(0.05f);
                self.level.Shake();
                self.sprite.Visible = self.flash.Visible = false;
                if (!self.oneUse)
                    self.outline.Visible = true;
                self.Depth = 8999;

                if (bullet.owner == null)
                    return;

                float direction = bullet.owner.Speed.Angle();
                self.level.ParticlesFG.Emit(self.p_shatter, 5, self.Position, Vector2.One * 4f, direction - 1.5707964f);
                self.level.ParticlesFG.Emit(self.p_shatter, 5, self.Position, Vector2.One * 4f, direction + 1.5707964f);
                SlashFx.Burst(self.Position, direction);
                if (self.oneUse)
                    self.Remove();
                self.respawnTimer = 2.5f;

            }

            orig(self, data, offset);
            self.Add(new BulletCollider(CollisionHandler, self.Collider));
        }

        private static void JellyHook(On.Celeste.Glider.orig_ctor_EntityData_Vector2 orig, Glider self, EntityData data, Vector2 offset)
        {
            void CollisionHandler(IWBTGBullet bullet)
            {
                if (!CanDoShit(bullet.owner)) { return; }

                if (!(AletrisSandboxModule.Settings.IWBTOptions.IWBTGGunHitsStuffOverride || AletrisSandboxModule.Session.IWBTGGunHitsStuff)) { return; }

                if (bullet.owner == null)
                    return;

                // trigger pickup
                if (self.bubble)
                {
                    for (int num = 0; num < 24; ++num)
                        self.level.Particles.Emit(Glider.P_Platform, self.Position + self.PlatformAdd(num), self.PlatformColor(num));
                }
                self.AllowPushing = false;
                self.Speed = Vector2.Zero;
                self.AddTag((int)Tags.Persistent);
                self.highFrictionTimer = 0.5f;
                self.bubble = false;
                self.wiggler.Start();
                self.tutorial = false;

                self.MoveTowardsY(bullet.CenterY - 5f, 4f);
                self.Speed.X = 160f * Vector2.Normalize(bullet.velocity).X;
                self.Speed.Y = -80f * -Vector2.Normalize(bullet.velocity).Y - 10f;
                self.noGravityTimer = 0.1f;
                self.wiggler.Start();

                bullet.Kill();
            }

            orig(self, data, offset);
            self.Add(new BulletCollider(CollisionHandler, self.Hold.PickupCollider));

            // launch jelly foward
        }

        private static void HeartHook(On.Celeste.HeartGem.orig_ctor_EntityData_Vector2 orig, HeartGem self, EntityData data, Vector2 offset)
        {

            void CollisionHandler(IWBTGBullet bullet)
            {
                if (!CanDoShit(bullet.owner)) { return; }

                if (!(AletrisSandboxModule.Settings.IWBTOptions.IWBTGGunHitsStuffOverride || AletrisSandboxModule.Session.IWBTGGunHitsStuff)) { return; }

                self.Collect(bullet.owner);
                bullet.Kill();
            }

            orig(self, data, offset);
            self.Add(new BulletCollider(CollisionHandler, self.Collider));

        }

    }
}