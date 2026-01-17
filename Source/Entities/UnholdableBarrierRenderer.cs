using Celeste;
using AletrisSandbox.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections.Generic;

namespace AletrisSandbox.Entities;

//Stole the code from SeekerBarrierRenderer verbatim, changed the values I needed to to search for UnholdableBarrier, and it just worked lmao
[Tracked()]
public class UnholdableBarrierRenderer : Entity
{
    class Edge
    {
        public readonly UnholdableBarrier Parent;

        public bool Visible;

        public readonly Vector2 A;

        public readonly Vector2 B;

        public readonly Vector2 Min;

        public readonly Vector2 Max;

        public readonly Vector2 Normal;

        public readonly Vector2 Perpendicular;

        public float[] Wave;
        public readonly float Length;

        public Edge(UnholdableBarrier parent, Vector2 a, Vector2 b)
        {
            Parent = parent;
            Visible = true;
            A = a;
            B = b;
            Min = new(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));
            Max = new(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
            Normal = (b - a).SafeNormalize();
            Perpendicular = -Normal.Perpendicular();
            Length = (a - b).Length();
        }

        public void UpdateWave(float time)
        {
            if (Wave == null || Wave.Length <= Length)
                Wave = new float[(int)Length + 2];

            for (var i = 0; i <= Length; i++)
                Wave[i] = GetWaveAt(time, i, Length);
        }

        float GetWaveAt(float offset, float along, float length)
        {
            if (along <= 1f || along >= length - 1f)
                return 0f;

            if (Parent.Solidify >= 1f)
                return 0f;

            var num = offset + along * 0.25f;
            var num2 = (float)(Math.Sin(num) * 2.0 + Math.Sin(num * 0.25f));

            return (1f + num2 * Ease.SineInOut(Calc.YoYo(along / length))) * (1f - Parent.Solidify);
        }

        public bool InView(ref Rectangle view)
        {
            if (view.Left < Parent.X + Max.X && view.Right > Parent.X + Min.X && view.Top < Parent.Y + Max.Y)
                return view.Bottom > Parent.Y + Min.Y;

            return false;
        }
    }

    readonly List<UnholdableBarrier> list = new();

    readonly List<Edge> edges = new();

    VirtualMap<bool> tiles;

    Rectangle levelTileBounds;

    bool dirty;

    public UnholdableBarrierRenderer()
    {
        Tag = (int)Tags.Global | (int)Tags.TransitionUpdate;
        Depth = 0;
        Add(new CustomBloom(OnRenderBloom));
    }

    public override void Awake(Scene scene)
    {
        base.Awake(scene);
    }

    public void Track(UnholdableBarrier block)
    {
        list.Add(block);

        if (tiles == null)
        {
            levelTileBounds = (Scene as Level).TileBounds;
            tiles = new(levelTileBounds.Width, levelTileBounds.Height, emptyValue: false);
        }

        for (var i = (int)block.X / 8; i < block.Right / 8f; i++)
        {
            for (var j = (int)block.Y / 8; j < block.Bottom / 8f; j++)
                tiles[i - levelTileBounds.X, j - levelTileBounds.Y] = true;
        }
        dirty = true;
    }

    public void Untrack(UnholdableBarrier block)
    {
        list.Remove(block);

        if (list.Count <= 0)
            tiles = null;
        else
        {
            for (var i = (int)block.X / 8; i < block.Right / 8f; i++)
            {
                for (var j = (int)block.Y / 8; j < block.Bottom / 8f; j++)
                    tiles[i - levelTileBounds.X, j - levelTileBounds.Y] = false;
            }
        }
        dirty = true;
    }

    public override void Update()
    {
        if (dirty)
            RebuildEdges();
        UpdateEdges();
    }

    public void UpdateEdges()
    {
        var camera = (Scene as Level).Camera;
        var view = new Rectangle((int)camera.Left - 4, (int)camera.Top - 4, (int)(camera.Right - camera.Left) + 8, (int)(camera.Bottom - camera.Top) + 8);

        for (var i = 0; i < edges.Count; i++)
        {
            if (edges[i].Visible)
            {
                if (Scene.OnInterval(0.25f, i * 0.01f) && !edges[i].InView(ref view))
                    edges[i].Visible = false;
            }
            else if (Scene.OnInterval(0.05f, i * 0.01f) && edges[i].InView(ref view))
                edges[i].Visible = true;

            if (edges[i].Visible && (Scene.OnInterval(0.05f, i * 0.01f) || edges[i].Wave == null))
                edges[i].UpdateWave(Scene.TimeActive * 3f);
        }
    }

    void RebuildEdges()
    {
        dirty = false;
        edges.Clear();

        if (list.Count > 0)
        {
            var obj = Scene as Level;
            _ = obj.TileBounds.Left;
            _ = obj.TileBounds.Top;
            _ = obj.TileBounds.Right;
            _ = obj.TileBounds.Bottom;
            var array = new Point[4]
            {
                new(0, -1),
                new(0, 1),
                new(-1, 0),
                new(1, 0)
            };

            foreach (var item in list)
            {
                for (var i = (int)item.X / 8; i < item.Right / 8f; i++)
                {
                    for (var j = (int)item.Y / 8; j < item.Bottom / 8f; j++)
                    {
                        var array2 = array;

                        for (var k = 0; k < array2.Length; k++)
                        {
                            var point = array2[k];
                            var point2 = new Point(-point.Y, point.X);

                            if (!Inside(i + point.X, j + point.Y) && (!Inside(i - point2.X, j - point2.Y) || Inside(i + point.X - point2.X, j + point.Y - point2.Y)))
                            {
                                var point3 = new Point(i, j);
                                var point4 = new Point(i + point2.X, j + point2.Y);
                                var value = new Vector2(4f) + new Vector2(point.X - point2.X, point.Y - point2.Y) * 4f;

                                while (Inside(point4.X, point4.Y) && !Inside(point4.X + point.X, point4.Y + point.Y))
                                {
                                    point4.X += point2.X;
                                    point4.Y += point2.Y;
                                }
                                var a = new Vector2(point3.X, point3.Y) * 8f + value - item.Position;
                                var b = new Vector2(point4.X, point4.Y) * 8f + value - item.Position;
                                edges.Add(new(item, a, b));
                            }
                        }
                    }
                }
            }
        }
    }

    bool Inside(int tx, int ty)
        => tiles[tx - levelTileBounds.X, ty - levelTileBounds.Y];

    void OnRenderBloom()
    {
        var camera = (Scene as Level).Camera;

        foreach (var item in list)
        {
            if (item.Visible)
                Draw.Rect(item.X, item.Y, item.Width, item.Height, Color.White);
        }

        foreach (var edge in edges)
        {
            if (edge.Visible)
            {
                var value = edge.Parent.Position + edge.A;
                _ = edge.Parent.Position + edge.B;

                for (var i = 0; i <= edge.Length; i++)
                {
                    var vector = value + edge.Normal * i;
                    Draw.Line(vector, vector + edge.Perpendicular * edge.Wave[i], Color.White);
                }
            }
        }
    }

    public override void Render()
    {
        if (list.Count > 0)
        {
            foreach (var item in list)
            {
                if (item.Visible)
                    Draw.Rect(item.Collider, Color.SaddleBrown * 0.15f);
            }

            if (edges.Count > 0)
            {
                foreach (var edge in edges)
                {
                    if (edge.Visible)
                    {
                        var value2 = edge.Parent.Position + edge.A;
                        _ = edge.Parent.Position + edge.B;

                        for (var i = 0; i <= edge.Length; i++)
                        {
                            var vector = value2 + edge.Normal * i;
                            Draw.Line(vector, vector + edge.Perpendicular * edge.Wave[i], Color.SaddleBrown * 0.25f);
                        }
                    }
                }
            }
        }
    }
}