using System.Collections;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.AletrisSandbox.Miscstuff;

// somebody tell me how the FUCK do i add a lib reference again

public class ButtonPress : VirtualButton.Node
{
    private bool changeState;

    private bool PreviousState;

    public bool CurrentState;

    public override bool Check => CurrentState;

    public override bool Pressed => CurrentState && !PreviousState;

    public override bool Released => !CurrentState && PreviousState;

    public override void Update()
    {
        PreviousState = changeState;
        changeState = CurrentState;
    }

    public void Press()
    {
        CurrentState = true;
    }

    public void Release()
    {
        CurrentState = default;
    }

    public IEnumerator HoldPress(float? time = null)
    {
        Press();

        yield return time;

        Release();
    }

    public void AutoHoldPress(float? time = null)
    {
        ProgrammaticNodes.HoldInput(HoldPress(time));
    }

    public static implicit operator bool(ButtonPress b)
        => b.CurrentState;
}

public class JoystickDirection : VirtualJoystick.Node
{
    public Vector2 CurrentDirection;

    public override Vector2 Value => Calc.Clamp(CurrentDirection, -1, -1, 1, 1);

    public void SetDirection(Vector2 value)
    {
        CurrentDirection = value;
    }

    public void SetNeutral()
    {
        CurrentDirection = default;
    }

    public IEnumerator HoldDirection(Vector2 value, float? time = null)
    {
        SetDirection(value);

        yield return time;

        SetNeutral();
    }

    public void AutoHoldDirection(Vector2 value, float? time = null)
    {
        ProgrammaticNodes.HoldInput(HoldDirection(value, time));
    }

    public static implicit operator Vector2(JoystickDirection j)
        => j.Value;
}

internal static class ProgrammaticNodes
{
    public static void HoldInput(IEnumerator input)
    {
        Engine.Scene.Tracker.GetEntity<Player>().Add(new Coroutine(input));
    }
}

public class AxisValue : VirtualAxis.Node
{
    public float CurrentValue;

    public override float Value => Calc.Clamp(CurrentValue, -1, 1);

    public void SetValue(float value)
    {
        CurrentValue = value;
    }

    public void SetNeutral()
    {
        CurrentValue = default;
    }

    public IEnumerator HoldValue(float value, float? time = null)
    {
        SetValue(value);

        yield return time;

        SetNeutral();
    }

    public void AutoHoldValue(float value, float? time = null)
    {
        ProgrammaticNodes.HoldInput(HoldValue(value, time));
    }

    public static implicit operator float(AxisValue a)
        => a.Value;
}

public class Binds
{
    public static readonly AxisValue MoveX = new();

    public static readonly AxisValue MoveY = new();

    public static readonly AxisValue GliderMoveY = new();

    public static readonly JoystickDirection Aim = new();

    public static readonly JoystickDirection Feather = new();

    public static readonly ButtonPress Jump = new();

    public static readonly ButtonPress Grab = new();

    public static readonly ButtonPress Dash = new();

    public static readonly ButtonPress Talk = new();

    public static readonly ButtonPress CrouchDash = new();
}