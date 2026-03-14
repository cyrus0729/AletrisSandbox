using System.Collections.Generic;
using Celeste.Mod.Entities;
using Celeste.Mod.Helpers.LegacyMonoMod;
using Microsoft.Xna.Framework;
using Monocle;

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
    public bool legacy;

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
        legacy = data.Bool("legacy");

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

    public void TravelPosition(Vector2 pos,float distance)
    {

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
            float plrPosDelta = Vector2.Distance(player.Center, curPos);




            // 3. Apply the Move ONCE
            MoveTo(Collider.HalfSize + new Vector2(XOffset,YOffset),player.Position);

        }
        else
        {
            MoveH(player.CenterX - CenterX);
        }
    }
}