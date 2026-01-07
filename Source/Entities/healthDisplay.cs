using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using System;

namespace Celeste.Mod.AletrisSandbox.Entities
{
    [Tracked]
    [CustomEntity("AletrisSandbox/healthDisplay")]
    public class healthDisplay : Entity
    {

        public int currentHP = AletrisSandboxModule.Session.HPAmount;

        public int maxHP = AletrisSandboxModule.Session.HPMax;

        public healthDisplay()
        {
            Tag = Tags.HUD | Tags.Global;
            Add(new BeforeRenderHook(new Action(DrawHP)));
        }

        private static void StartSpriteBatch()
        {
            Draw.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);
        }
        private static void EndSpriteBatch()
        {
            Draw.SpriteBatch.End();
        }

        private void DrawHP()
        {
            ActiveFont.Draw(AletrisSandboxModule.Session.HPAmount.ToString() + "/" + AletrisSandboxModule.Session.HPMax.ToString(), new Vector2(720f, 144f), Color.White);
        }

    }
}
