using System;
using System.Collections.Generic;
using System.Linq;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using On.Celeste.Pico8;

namespace Celeste.Mod.AletrisSandbox.Entities;

public class SolidThingyNode
{
    public Vector2 Position;
    public SolidThingyNode nextNode;
    public SolidThingyNode prevNode;

    public SolidThingyNode(Vector2 position)
    {
        Position = position;
        nextNode = null;
        prevNode = null;
    }
}

[CustomEntity("AletrisSandbox/CustomCursedMovingSolidThingy")]
public class CustomCursedMovingSolidThingy : Solid
{

    public int XOffset;
    public int YOffset;
    public Color pathColor;
    public bool nonCollidable;
    public MTexture SolidSprite;
    public Image NodeSprite;
    public List<Vector2> nodes;
    public MTexture[,] NineSliceBlock;
    public bool legacy;

    SolidThingyNode currentNode;
    SolidThingyNode headNode;

    public CustomCursedMovingSolidThingy(EntityData data, Vector2 offset)
        : base(data.Position + offset, data.Width, data.Height, true)
    {

        Depth = -8500;

        XOffset = data.Int("XOffset");
        YOffset = data.Int("YOffset");
        pathColor = data.HexColor("pathColor");
        nonCollidable = data.Bool("nonCollidable");
        legacy = data.Bool("legacy");
        SolidSprite = GFX.Game[data.Attr("sprite")+"solid"];
        NodeSprite = new(GFX.Game[data.Attr("sprite") + "node"]);
        nodes = new(data.Nodes);

        SolidThingyNode head = new(data.Position + offset + Collider.HalfSize);
        currentNode = head;
        foreach (var pos in nodes)
        {
            SolidThingyNode newNode = new(pos + offset + Collider.HalfSize);
            currentNode.nextNode = newNode;
            newNode.prevNode = currentNode;
            currentNode = newNode;
        }
        currentNode = head;
        headNode = head;
        Collidable = !nonCollidable;
        NodeSprite.SetColor(nonCollidable ? new(255, 255, 255, 128) : new(255, 255, 255, 255));
        NodeSprite.JustifyOrigin(0.5f, 0.5f);

        foreach (Image i in Utils.BuildSprite(SolidSprite,this,nonCollidable ? new(255, 255, 255, 128) : new(255, 255, 255, 255))) { Add(i); }

    }

    public override void Render()
    {
        SolidThingyNode render = headNode;
        while (render.nextNode != null)
        {
            NodeSprite.Position = render.Position;
            NodeSprite.Render();
            Draw.Line(render.Position, render.nextNode.Position, pathColor);
            render = render.nextNode;
        }
        NodeSprite.Position = render.Position;
        NodeSprite.Render();
        base.Render();
    }


    private Vector2 CalculateXnaY(float targetX, Vector2 start, Vector2 end)
    {
        float dx = end.X - start.X;

        // Avoid division by zero for vertical lines
        if (Math.Abs(dx) < 0.0001f)
            return start;

        // Find the interpolation factor 't' based on X position
        float t = (targetX - start.X) / dx;

        // Use Vector2.Lerp to find the exact point on the line
        return Vector2.Lerp(start, end, t);
    }

    private bool IsBetween(float val, float a, float b)
    {
        return val >= Math.Min(a, b) && val <= Math.Max(a, b);
    }

    private Vector2 GetClosestPointOnSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        float lengthSq = ab.LengthSquared();

        if (lengthSq == 0)
            return a; // A 與 B 重合

        // 計算投影比例 t (0.0 到 1.0 之間)
        float t = MathHelper.Clamp(Vector2.Dot(p - a, ab) / lengthSq, 0f, 1f);

        return a + t * ab;
    }


    public override void Update()
    {
        base.Update();
        Player player = SceneAs<Level>().Tracker.GetEntity<Player>();
        if (player == null) return;

        if (nodes.Count == 0)
        {
            MoveH(player.CenterX - CenterX);
            return;
        }

        Vector2 playerPos = player.Center;
        Vector2 bestPos = currentNode.Position;
        float minDistanceSq = Vector2.DistanceSquared(playerPos, bestPos);

        if (currentNode.nextNode != null)
        {
            Vector2 closest = legacy
                                  ? GetClosestPointOnSegment(playerPos, currentNode.Position, currentNode.nextNode.Position)
                                  : CalculateXnaY(player.CenterX, currentNode.Position, currentNode.nextNode.Position);

            float distSq = Vector2.DistanceSquared(playerPos, closest);

            if (distSq < minDistanceSq) // closest so far
            {
                bestPos = closest;
                minDistanceSq = distSq;

                if (Vector2.DistanceSquared(playerPos, currentNode.nextNode.Position) < Vector2.DistanceSquared(playerPos, currentNode.Position)) // safeguard
                    currentNode = currentNode.nextNode;
            }

        }

        // same thing as above
        if (currentNode.prevNode != null)
        {
            Vector2 closest = legacy
                                  ? GetClosestPointOnSegment(playerPos, currentNode.Position, currentNode.prevNode.Position)
                                  : CalculateXnaY(player.CenterX, currentNode.Position, currentNode.prevNode.Position);
                float distSq = Vector2.DistanceSquared(playerPos, closest);

                if (distSq < minDistanceSq)
                {
                    bestPos = closest;

                    if (Vector2.DistanceSquared(playerPos, currentNode.prevNode.Position) < Vector2.DistanceSquared(playerPos, currentNode.Position))
                        currentNode = currentNode.prevNode;
                }
        }


        MoveTo(bestPos - Collider.HalfSize + new Vector2(XOffset, YOffset));
    }
}