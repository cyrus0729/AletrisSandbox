using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.AletrisSandbox.Triggers;

[CustomEntity("AletrisSandbox/ToggleHealthTrigger")]
public class ToggleHealthTrigger : Trigger
{
    public bool enableHealth;
    public int defaultHealth;

    public ToggleHealthTrigger(EntityData data, Vector2 offset) : base(data, offset)
    {
        enableHealth = data.Bool("Enable", true);
        defaultHealth = data.Int("DefaultHealth", 1000);
    }

    public override void OnEnter(Player player) // start
    {
        base.OnEnter(player);
        AletrisSandboxModule.Session.HPSystemEnabled = enableHealth;
        AletrisSandboxModule.Session.HPAmount = defaultHealth;
        AletrisSandboxModule.Session.HPMax = defaultHealth;
    }
}