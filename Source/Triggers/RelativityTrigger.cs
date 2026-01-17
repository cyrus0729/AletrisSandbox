using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using System.Reflection;
using System;
using Monocle;

namespace Celeste.Mod.AletrisSandbox.Triggers;

[CustomEntity("AletrisSandbox/RelativityTrigger")]
public class RelativityTrigger : Trigger
{
    public bool disableOnLeave;
    public bool enable;
    public static bool hookAdded;

    public RelativityTrigger(EntityData data, Vector2 offset) : base(data, offset)
    {
        enable = data.Bool("Enable", true);
        disableOnLeave = data.Bool("DisableOnLeave");
    }

    public override void OnEnter(Player player)
    {
        base.OnEnter(player);
        AletrisSandboxModule.Session.RelativisticVelocityEnabled = enable || AletrisSandboxModule.Settings.MiscelleaneousMenu.RelativisticVelocityOverride;
    }

    public override void OnLeave(Player player)
    {
        base.OnLeave(player);

        if (!disableOnLeave)
            return;

        AletrisSandboxModule.Session.RelativisticVelocityEnabled = false || AletrisSandboxModule.Settings.MiscelleaneousMenu.RelativisticVelocityOverride;
    }
}