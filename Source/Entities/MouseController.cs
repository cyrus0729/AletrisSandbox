using System;
using System.Reflection.Metadata.Ecma335;
using Celeste.Mod.AletrisSandbox.Miscstuff;
using Celeste.Mod.Entities;
using Celeste.Pico8;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;

namespace Celeste.Mod.AletrisSandbox.Entities;

[CustomEntity("AletrisSandbox/MouseController")]
[Tracked]
public class MouseController : Entity
{

	public bool enabled;
    public bool forced;
    public bool visible;

    public MouseController(EntityData data, Vector2 offset)
        : base(data.Position + offset)
    {
        enabled = data.Bool("Enabled");
        forced = data.Bool("Forced");
        visible = data.Bool("Visible");

        if (visible) { Add(GFX.SpriteBank.Create("MouseController")); }
    }

    public void Unload(Level level, LevelData next, Vector2 direction)
    {
        Binds.MoveX.SetValue(0);
        Binds.MoveY.SetValue(0);
        Binds.Aim.SetDirection(Vector2.Zero);
        Binds.Feather.SetDirection(Vector2.Zero);
    }

    public override void Update()
    {
        base.Update();

        AletrisSandboxModule.Session.mouseControlsState[1] = forced;

        if (AletrisSandboxModule.Session.mouseControlsState[1])
        {
            MInput.Keyboard.CurrentState = new();
        }

        // not forced and enabled and isn't paused
        if (!AletrisSandboxModule.Session.mouseControlsState[1] && enabled && !AletrisSandboxModule.Session.mouseControlsState[0])
        {
            Binds.MoveX.SetValue(0);
            Binds.MoveY.SetValue(0);
            Binds.Aim.SetDirection(Vector2.Zero);
            Binds.Feather.SetDirection(Vector2.Zero);
            return;
        }

        var State = Mouse.GetState();
        var MouseCursorPos = Vector2.Transform(new(State.X, State.Y), Matrix.Invert(Engine.ScreenMatrix));
        var player = SceneAs<Level>().Tracker.GetEntity<Player>();
        var toCursor = AletrisSandboxModule.ToCursor(player, MouseCursorPos, false);
        var toCursorNormal = toCursor.SafeNormalize();

        if (SceneAs<Level>().InCutscene || player == null)
            return;

        // Logger.Log(LogLevel.Info, nameof(AletrisSandboxModule), MouseCursorPos.ToString()+toCursorNormal.ToString());
        if (Math.Abs(toCursor.X) > 4)
        {
            if (player.dashAttackTimer <= 0) {
                Binds.MoveX.SetValue(toCursorNormal.X);
                Binds.MoveY.SetValue(toCursorNormal.Y);
                Binds.Aim.SetDirection(toCursorNormal);
                Binds.Feather.SetDirection(toCursorNormal);
            }
        }
        else
        {
            Binds.MoveX.SetValue(0);
            Binds.MoveY.SetValue(toCursorNormal.Y);
        }

        if (MInput.Mouse.CheckLeftButton) // jump
        {
            Binds.Jump.Press();
        }
        else
        {
            Binds.Jump.Release();
        }

        if (MInput.Mouse.CheckMiddleButton) // climb
        {
            Binds.Grab.Press();
        }
        else
        {
            Binds.Grab.Release();
        }

        if (MInput.Mouse.PressedRightButton) // dash
        {
            Binds.MoveX.SetValue(toCursorNormal.X);
            Binds.MoveY.SetValue(toCursorNormal.Y);
            Binds.Aim.SetDirection(toCursorNormal);
            Binds.Feather.SetDirection(toCursorNormal);
            Binds.Dash.Press();
        }
        else
        {
            Binds.Dash.Release();
        }

        Everest.Events.Level.OnTransitionTo += Unload;

    }
}