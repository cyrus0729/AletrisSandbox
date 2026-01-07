using AsmResolver.IO;
using Celeste.Mod.AletrisSandbox.Entities;
using Celeste.Mod.AletrisSandbox.GunSupport;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monocle;
using MonoMod.ModInterop;
using On.Celeste;
using System;
using System.Reflection;

namespace Celeste.Mod.AletrisSandbox
{
    public class AletrisSandboxModule : EverestModule
    {
        public static AletrisSandboxModule Instance { get; private set; }

        public override Type SettingsType => typeof(AletrisSandboxModuleSettings);
        public static AletrisSandboxModuleSettings Settings => (AletrisSandboxModuleSettings)Instance._Settings;

        public override Type SessionType => typeof(AletrisSandboxModuleSession);
        public static AletrisSandboxModuleSession Session => (AletrisSandboxModuleSession)Instance._Session;

        public AletrisSandboxModule()
        {
            Instance = this;
#if DEBUG
            // debug builds use verbose logging
            Logger.SetLogLevel(nameof(AletrisSandboxModule), LogLevel.Verbose);
#else
            // release builds use info logging to reduce spam in log files
            Logger.SetLogLevel(nameof(AletrisSandboxModule), LogLevel.Info);
#endif
        }

        public MTexture gunTexture;
        private Texture2D crossTexture;

        private VirtualJoystick joystickAim;
        private Vector2 oldJoystickAim;
        private Vector2 oldMouseCursorPos = Vector2.Zero;
        private Vector2 CursorPos = Vector2.Zero;
        private bool usingJoystickAim = false;

        private static MouseState State => Mouse.GetState();
        private static Vector2 MouseCursorPos => Vector2.Transform(new Vector2(State.X, State.Y), Matrix.Invert(Engine.ScreenMatrix));
        public override void LoadContent(bool firstLoad)
        {
            gunTexture = GFX.Game["AletrisSandbox/gun/gun"];
            crossTexture = GFX.Game["AletrisSandbox/gun/crosshair"].Texture.Texture;
        }
        private void EverestExitMethod(Level level, LevelExit exit, LevelExit.Mode mode, Session session, HiresSnow snow)
        {
            Settings.IWBTOptions.IWBTGGunEnableOverride = Session.oldEnabledConfig;
            Settings.HealthOptions.HPSystemEnableOverride = Session.oldEnabledConfig;
        }
        private void EverestEnterMethod(Session session, bool fromSaveData)
        {
            Session.oldEnabledConfig = Settings.IWBTOptions.IWBTGGunEnableOverride;
            Session.oldEnabledConfig = Settings.HealthOptions.HPSystemEnableOverride;
        }

        private static Vector2 PlayerPosScreenSpace(Actor self)
        {
            return self.Center - (self.Scene as Level).Camera.Position;
        }

        private static Vector2 ToCursor(Actor player, Vector2 cursorPos)
        {
            return Vector2.Normalize(cursorPos / 6f - PlayerPosScreenSpace(player));
        }

        private static float ToRotation(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        private void GunPlayerRender(On.Celeste.Player.orig_Render orig, Player self)
        {
            orig(self);

            if (!(Settings.IWBTOptions.IWBTGGunEnableOverride || Session.IWBTGGunEnabled)) return;

            SpriteEffects flip = SpriteEffects.None;
            Vector2 offset;

            Vector2 gunVector = ToCursor(self, CursorPos);

            float gunRotation = Math.Min(Math.Max(ToRotation(gunVector), -1.2f), 1.2f);

            if (Settings.IWBTOptions.IWBTGGunAimOverride || Session.IWBTGGunMouseAimEnabled)
            {

                self.Facing = (Facings)Math.Sign(ToCursor(self, CursorPos).X);
                if (self.Facing == 0) self.Facing = Facings.Right;
                gunRotation = ToRotation(gunVector);

                if (self.Facing == Facings.Left)
                {
                    flip = SpriteEffects.FlipVertically;
                }

                offset.X = 0.3f;
                offset.Y = 0.5f;

            }
            else
            {

                gunRotation = 0f;

                if (self.Facing == Facings.Left)
                {
                    flip = SpriteEffects.FlipHorizontally;
                    offset.X = 0.9f;
                    offset.Y = 0.5f;
                }
                else
                {
                    flip = SpriteEffects.None;
                    offset.X = 0.1f;
                    offset.Y = 0.5f;
                }

            }

            gunTexture.DrawJustified(
            self.Center,
            offset,
            Color.White,
            1f,
            gunRotation,
            flip
            );

        }

        private void GunLevelRender(On.Celeste.Level.orig_Render orig, Level self)
        {
            orig(self);
            if (!(Settings.IWBTOptions.IWBTGGunEnableOverride || Session.IWBTGGunEnabled)) return;
            if (!(Settings.IWBTOptions.IWBTGGunAimOverride || Session.IWBTGGunMouseAimEnabled)) return;

            Draw.SpriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Engine.ScreenMatrix);

            Color aimColor = Color.White;

            if (self.Tracker.CountEntities<IWBTGBullet>() >= Session.Maxbullets)
            {
                aimColor = Color.Red;
            }
            else
            {
                aimColor = Color.White;
            }

            Draw.SpriteBatch.Draw(crossTexture, CursorPos, null, aimColor, 0f, new Vector2(crossTexture.Width / 2f, crossTexture.Height / 2f), 4f, 0, 0f);
            Draw.SpriteBatch.End();
        }

        private void GunPlayerUpdate(On.Celeste.Player.orig_Update orig, Player self)
        {

            orig(self);

            bool boolToCheck;

            if (!(Settings.IWBTOptions.IWBTGGunEnableOverride || Session.IWBTGGunEnabled)) return;

            var components = self.SceneAs<Level>().Tracker.GetComponents<OnlyBlocksPlayer>();
            foreach (OnlyBlocksPlayer component in components)
            {
                component.Entity.Collidable = true;
            }
            foreach (OnlyBlocksPlayer component in components)
            {
                component.Entity.Collidable = false;
            }

            // cursor pos update
            if (joystickAim.Value.LengthSquared() > 0.04f)
            {
                usingJoystickAim = true;
            }
            else if (MouseCursorPos != oldMouseCursorPos)
            {
                usingJoystickAim = false;
            }

            if (usingJoystickAim && self.Scene != null)
            {
                CursorPos = (PlayerPosScreenSpace(self) + oldJoystickAim * 70f) * 6f;
                if (joystickAim.Value.LengthSquared() > 0.04f)
                {
                    oldJoystickAim = joystickAim.Value;
                }
            }
            else
            {
                CursorPos = MouseCursorPos;
            }
            oldMouseCursorPos = MouseCursorPos;


            if (self.Scene == null || self.Scene.TimeActive <= 0f || (TalkComponent.PlayerOver != null && Input.Talk.Pressed)) return;

            float turnOffset = (self.Facing == Facings.Left) ? -20f : 0f;
            Vector2 mouseposition = new Vector2(self.Center.X, self.Center.Y - 4.5f);
            Vector2 position = new Vector2(self.Center.X + 7f + turnOffset, self.Center.Y - 4.5f);

            bool isAimOverride = Settings.IWBTOptions.IWBTGGunAimOverride || Session.IWBTGGunMouseAimEnabled;
            bool isAutoFireOverride = Settings.IWBTOptions.IWBTGGunAutoFireOverride || Session.IWBTGGunAutofireEnabled;

            boolToCheck = isAimOverride
                ? (Settings.IWBTOptions.ShootBullet.Pressed || MInput.Mouse.PressedLeftButton)
                : Settings.IWBTOptions.ShootBullet.Pressed;

            if (isAutoFireOverride)
            {
                boolToCheck = isAimOverride
                    ? Settings.IWBTOptions.ShootBullet.Check || MInput.Mouse.CheckLeftButton
                    : Settings.IWBTOptions.ShootBullet.Check;
            }

            if (!boolToCheck) return;
            if (self.Scene.Tracker.CountEntities<IWBTGBullet>() >= Session.Maxbullets) return;

            if (Settings.IWBTOptions.IWBTGGunAimOverride || Session.IWBTGGunMouseAimEnabled)
            {
                new IWBTGBullet(mouseposition, ToCursor(self, CursorPos) * 5f, self);
            }
            else
            {
                new IWBTGBullet(position, (self.Facing == Facings.Left ? new Vector2(-1, 0) : new Vector2(1, 0)) * 5f, self);
            }
            Audio.Play("event:/AletrisSandbox_stuff/fire" + Settings.IWBTOptions.GunSound.ToString(), "fade", 0.5f);

        }

        private void HitboxPlayerUpdate(On.Celeste.Player.orig_Update orig, Player self)
        {

            orig(self);
            if (!Settings.HitboxOptions.PlayerHitboxEnableOverride) return;

            // god is real and i killed them

            try
            {
                self.normalHitbox.Width = (float)Settings.HitboxSizeMenu.NormalHitboxSizeX;
                self.normalHitbox.Height = (float)Settings.HitboxSizeMenu.NormalHitboxSizeY;
                self.duckHitbox.Width = (float)Settings.HitboxSizeMenu.DuckHitboxSizeX;
                self.duckHitbox.Height = (float)Settings.HitboxSizeMenu.DuckHitboxSizeY;
                self.starFlyHitbox.Width = (float)Settings.HitboxSizeMenu.FeatherHitboxSizeX;
                self.starFlyHitbox.Height = (float)Settings.HitboxSizeMenu.FeatherHitboxSizeY;
                self.normalHurtbox.Width = (float)Settings.HitboxSizeMenu.NormalHurtboxSizeX;
                self.normalHurtbox.Height = (float)Settings.HitboxSizeMenu.NormalHurtboxSizeY;
                self.duckHurtbox.Width = (float)Settings.HitboxSizeMenu.DuckHurtboxSizeX;
                self.duckHurtbox.Height = (float)Settings.HitboxSizeMenu.DuckHurtboxSizeY;
                self.starFlyHurtbox.Width = (float)Settings.HitboxSizeMenu.FeatherHurtboxSizeX;
                self.starFlyHurtbox.Height = (float)Settings.HitboxSizeMenu.FeatherHurtboxSizeY;

                self.normalHitbox.Position.X = (float)Settings.HitboxOffsetMenu.NormalHitboxOffsetX;
                self.normalHitbox.Position.Y = (float)Settings.HitboxOffsetMenu.NormalHitboxOffsetY;
                self.duckHitbox.Position.X = (float)Settings.HitboxOffsetMenu.DuckHitboxOffsetX;
                self.duckHitbox.Position.Y = (float)Settings.HitboxOffsetMenu.DuckHitboxOffsetY;
                self.starFlyHitbox.Position.X = (float)Settings.HitboxOffsetMenu.FeatherHitboxOffsetX;
                self.starFlyHitbox.Position.Y = (float)Settings.HitboxOffsetMenu.FeatherHitboxOffsetY;
                self.normalHurtbox.Position.X = (float)Settings.HitboxOffsetMenu.NormalHurtboxOffsetX;
                self.normalHurtbox.Position.Y = (float)Settings.HitboxOffsetMenu.NormalHurtboxOffsetY;
                self.duckHurtbox.Position.X = (float)Settings.HitboxOffsetMenu.DuckHurtboxOffsetX;
                self.duckHurtbox.Position.Y = (float)Settings.HitboxOffsetMenu.DuckHurtboxOffsetY;
                self.starFlyHurtbox.Position.X = (float)Settings.HitboxOffsetMenu.FeatherHurtboxOffsetX;
                self.starFlyHurtbox.Position.Y = (float)Settings.HitboxOffsetMenu.FeatherHurtboxOffsetY;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, "AletrisSandbox", "Error assigning hitbox or hurtbox sizes/offsets!");
                Logger.Log(LogLevel.Error, "AletrisSandbox", e.Message);
                self.normalHitbox.Width = 8f;
                self.normalHitbox.Height = 11f;
                self.duckHitbox.Width = 8f;
                self.duckHitbox.Height = 9f;
                self.starFlyHitbox.Width = 8f;
                self.starFlyHitbox.Height = 8f;
                self.normalHurtbox.Width = 8f;
                self.normalHurtbox.Height = 6f;
                self.duckHurtbox.Width = 8f;
                self.duckHurtbox.Height = 4f;
                self.starFlyHurtbox.Width = 6f;
                self.starFlyHurtbox.Height = 6f;

                self.normalHitbox.Position.X = -4f;
                self.normalHitbox.Position.Y = -11f;
                self.duckHitbox.Position.X = -4f;
                self.duckHitbox.Position.Y = -11f;
                self.starFlyHitbox.Position.X = -4f;
                self.starFlyHitbox.Position.Y = -10f;
                self.normalHurtbox.Position.X = -4f;
                self.normalHurtbox.Position.Y = -6f;
                self.duckHurtbox.Position.X = -4f;
                self.duckHurtbox.Position.Y = -6f;
                self.starFlyHurtbox.Position.X = -3f;
                self.starFlyHurtbox.Position.Y = -9f;
            }

        }

        private void HitboxPlayerRender(On.Celeste.Player.orig_Render orig, Player self)
        {
            orig(self);
            if (!Settings.CircleMadelineOverride) { return; }
            Draw.Circle(self.Position, 5, Color.Red, 50);
        }

        private void HPLevelUpdate(On.Celeste.Level.orig_Update orig, Level self)
        {
            orig(self);
            if (!(Settings.HealthOptions.HPSystemEnableOverride || Session.HPSystemEnabled)) { return; }
            if (self.Tracker.CountEntities<healthDisplay>() >= 1) { return; }
            self.Add(new healthDisplay());
        }

        // thanks whoever made the static int part of the code
        private static int Player_IWBTJumpUpdate(On.Celeste.Player.orig_NormalUpdate orig, Player self)
        {
            if (!(Settings.IWBTOptions.IWBTGJumpEnableOverride || Session.IWBTGJumpEnabled)) return orig(self);
            if (self.varJumpTimer > 0)
            {
                if (!self.AutoJump && !Input.Jump.Check)
                {
                    self.Speed.Y -= Math.Min(self.Speed.Y, self.varJumpSpeed) * 0.35f;
                }
            }
            return orig(self);
        }


        [Tracked]
        public class BulletCollider : Component
        {

            private Collider collider;
            public Action<IWBTGBullet> OnCollide;

            public BulletCollider(Action<IWBTGBullet> onCollide, Collider collider = null)
            : base(active: false, visible: false)
            {
                this.collider = collider;
                OnCollide = onCollide;
            }

            public bool Check(IWBTGBullet bullet)
            {
                Collider collider = Entity.Collider;
                if (this.collider != null)
                {
                    Entity.Collider = this.collider;
                }
                bool result = bullet.CollideCheck(Entity);
                Entity.Collider = collider;
                return result;
            }
        }

        [Tracked]
        public class OnlyBlocksPlayer : Component
        {
            public OnlyBlocksPlayer() : base(false, false) { }

            public override void Added(Entity entity)
            {
                base.Added(entity);
                entity.Collidable = false;
            }
        }

        public static void  UnholdableBarrier_Player_Update(On.Celeste.Player.orig_Update orig, Player self)
        {
            var components = self.SceneAs<Level>().Tracker.GetComponents<OnlyBlocksPlayer>();
            foreach (OnlyBlocksPlayer component in components)
            {
                component.Entity.Collidable = true;
            }
            orig(self);
            foreach (OnlyBlocksPlayer component in components)
            {
                component.Entity.Collidable = false;
            }
        }

        public static void RelativisticVelocity_PlayerUpdate(On.Celeste.Player.orig_Update orig, Player self)
        {
            bool enableRelativisticVel = AletrisSandboxModule.Session.RelativisticVelocityEnabled || AletrisSandboxModule.Settings.MiscelleaneousMenu.RelativisticVelocityOverride;
            //float dt = Engine.DeltaTime;
            //PropertyInfo engineDeltaTimeProp = typeof(Engine).GetProperty("DeltaTime");
            if (enableRelativisticVel)
            {
                Logger.Log("aletrissandboxmodule", "relativistic velocity: " + enableRelativisticVel.ToString());
                float speedcap = 1000f;
                if (Math.Abs(self.Speed.X) > 0)
                {
                    Logger.Log("aletrissandboxmodule", "Clamped Speed:" + (speedcap / Math.Abs(self.Speed.X)).ToString());
                    Logger.Log("aletrissandboxmodule","Relativistic TimeRate:" + Math.Clamp(speedcap / Math.Abs(self.Speed.X), 0f, 1.0f).ToString());
                    Engine.TimeRate = Math.Clamp(speedcap / Math.Abs(self.Speed.X), 0f, 1.0f);
                    //(n)x of default speed
                }
                else
                {
                    Engine.TimeRate = 1.0f;
                }
            }
            orig(self);
            //engineDeltaTimeProp.SetValue(null, dt, null);
        }



        //public override void OnInputInitialize()
        //{
        //    base.OnInputInitialize();
        //    joystickAim = new VirtualJoystick(true, new VirtualJoystick.PadRightStick(Input.Gamepad, 0.1f));
        //}

        //public override void OnInputDeregister()
        //{
        //    base.OnInputDeregister();
        //    joystickAim?.Deregister();
        //}

        public override void Load()
        {
            VanillaHooks.Load();
            On.Celeste.Player.Render += GunPlayerRender;
            On.Celeste.Player.Update += GunPlayerUpdate;
            On.Celeste.Level.Render += GunLevelRender;
            On.Celeste.Player.Render += HitboxPlayerRender;
            On.Celeste.Player.Update += HitboxPlayerUpdate;
            On.Celeste.Level.Update += HPLevelUpdate;
            On.Celeste.Player.NormalUpdate += Player_IWBTJumpUpdate;
            On.Celeste.Player.Update += UnholdableBarrier_Player_Update;
            On.Celeste.Player.Update += RelativisticVelocity_PlayerUpdate;
            // TODO: apply any hooks that should always be active
        }

        public override void Unload()
        {
            VanillaHooks.Unload();
            On.Celeste.Player.Render -= GunPlayerRender;
            On.Celeste.Player.Update -= GunPlayerUpdate;
            On.Celeste.Level.Render -= GunLevelRender;
            On.Celeste.Player.Render -= HitboxPlayerRender;
            On.Celeste.Player.Update -= HitboxPlayerUpdate;
            On.Celeste.Level.Update -= HPLevelUpdate;
            On.Celeste.Player.NormalUpdate -= Player_IWBTJumpUpdate;
            On.Celeste.Player.Update -= UnholdableBarrier_Player_Update;
            On.Celeste.Player.Update -= RelativisticVelocity_PlayerUpdate;
            // TODO: unapply any hooks applied in Load()
        }
    }
}