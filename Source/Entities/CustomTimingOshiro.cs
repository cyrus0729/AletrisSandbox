using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Celeste.Mod.AletrisSandbox.Entities
{
    [CustomEntity("AletrisSandbox/CustomTimingOshiro")]
    public class CustomTimingOshiro : AngryOshiro
    {
        public float[] timings;
        public CustomTimingOshiro(EntityData data, Vector2 offset)
            : base(data.Position + offset, data.Bool("fromCutscene", false))
        {
            float val;
            foreach (var t in data.Attr("timings").Split(","))
            {
                if (!float.TryParse(t, out val)) continue;
                timings.Append(val);
            }
            ChaseWaitTimes = timings;
        }
    }
}
