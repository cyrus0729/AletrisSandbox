namespace Celeste.Mod.AletrisSandbox;

using Microsoft.Xna.Framework;
using Monocle;
using System.Text.RegularExpressions;


public class hawa
{
    public static Collider ParseCollider(string str) // in format (R/C:X,Y,oX,oY)
    {
        Logger.Log(LogLevel.Info, "AletrisSandbox", str);
        Regex rgc = new Regex(@"^C:(-?\d+),(-?\d+),(-?\d+)$");
        Regex rgr = new Regex(@"^R:(-?\d+),(-?\d+),(-?\d+),(-?\d+)$");
        Match matchr = rgr.Match(str);
        Match matchc = rgc.Match(str);

        if (matchr.Success)
        {
            return new Hitbox(
                float.Parse(matchr.Groups[1].Value),
                float.Parse(matchr.Groups[2].Value),
                float.Parse(matchr.Groups[3].Value),
                float.Parse(matchr.Groups[4].Value));
        }
        else if (matchc.Success)
        {
            return new Circle(
                float.Parse(matchc.Groups[1].Value),
                float.Parse(matchc.Groups[2].Value),
                float.Parse(matchc.Groups[3].Value));
        }
        else
        {
            Logger.Log(LogLevel.Warn, "AletrisSandbox", "Wrong syntax for string " + str + "! (R:X,Y,oX,oY)/(C:R,X,Y)");

            return new Hitbox(8f, 8f, -4f, -8f);
        }
    }
}