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

    public override void Update()
    {
        base.Update();

        Player player = SceneAs<Level>().Tracker.GetEntity<Player>();

        if (player == null)
            return;

        if (nodes.Count > 1)
        {
            Vector2 curPos = currentNode.Position;

            Vector2 bestSnappedPos = curPos;
            float bestDistanceSq = float.MaxValue;
            SolidThingyNode nextBestNode = null;

            // check segment to next node
            if (currentNode.nextNode != null)
            {
                Vector2 path = currentNode.nextNode.Position - curPos;
                float len = path.Length();

                if (len > 0)
                {
                    Vector2 unit = path / len;
                    float proj = MathHelper.Clamp(Vector2.Dot(player.Position - curPos, unit), 0, len);
                    Vector2 snap = curPos + (unit * proj);

                    float distSq = Vector2.DistanceSquared(player.Position, snap);
                    bestSnappedPos = snap;
                    bestDistanceSq = distSq;

                    if (proj > len - 0.001f)
                        nextBestNode = currentNode.nextNode;
                }
            }

            // ditto (only if closer)
            if (currentNode.prevNode != null)
            {
                Vector2 path = currentNode.prevNode.Position - curPos;
                float len = path.Length();

                if (len > 0)
                {
                    Vector2 unit = path / len;
                    float proj = MathHelper.Clamp(Vector2.Dot(player.Position - curPos, unit), 0, len);
                    Vector2 snap = curPos + (unit * proj);

                    float distSq = Vector2.DistanceSquared(player.Position, snap);

                    if (distSq < bestDistanceSq) // if it's actually closer
                    {
                        bestSnappedPos = snap;

                        if (proj > len - 0.001f)
                            nextBestNode = currentNode.prevNode;
                    }
                }
            }

            // 3. Apply the Move ONCE
            MoveTo(bestSnappedPos - Collider.HalfSize + new Vector2(XOffset,YOffset), bestSnappedPos - player.Position);

            // 4. Switch Node if needed
            if (nextBestNode != null)
            {
                currentNode = nextBestNode;
            }
        }
        else
        {
            MoveH(player.CenterX - CenterX);
        }
    }
}