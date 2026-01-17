using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using System;

namespace Celeste.Mod.AletrisSandbox.Entities;

[Tracked, CustomEntity("AletrisSandbox/healthDisplay")]
public class healthDisplay : Entity
{
    public int currentHP = AletrisSandboxModule.Session.HPAmount;

    public int maxHP = AletrisSandboxModule.Session.HPMax;

    public healthDisplay()
    {
        Tag = Tags.HUD | Tags.Global;
        Add(new BeforeRenderHook(DrawHP));
    }

    static void StartSpriteBatch()
    {
        Draw.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);
    }

    static void EndSpriteBatch()
    {
        Draw.SpriteBatch.End();
    }

    void DrawHP()
    {
        ActiveFont.Draw(AletrisSandboxModule.Session.HPAmount + "/" + AletrisSandboxModule.Session.HPMax, new(720f, 144f), Color.White);
    }
}