using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using CSharpDecorator.Framework;
using GME.MGA;
using GME.MGA.Meta;
using GME.Util;

namespace GenericDecorator
{
    abstract class BackgroundDrawBase : IBackroundDraw
    {

        public static int roundsize = 8;
        public static int roundres = 5;
        public static float linewidth = 2;
        protected static readonly Color BlueGrad1 = Color.FromArgb(255, 33, 177, 254);
        protected static readonly Color BlueGrad2 = Color.FromArgb(255, 3, 124, 188);
        protected static readonly Color GrayGrad1 = Color.FromArgb(255, 200, 200, 200);
        protected static readonly Color GrayGrad2 = Color.FromArgb(255, 100, 100, 100);
        public static readonly Color Border = Color.FromArgb(255, 29, 127, 181);
        protected static readonly Color GrayBorder = Color.FromArgb(255, 150, 150, 150);
        protected static readonly Color RefBorder = Color.Cyan;
        protected static readonly Color NullRefBorder = Color.Red;

        protected void RoundRect(Graphics g, Pen p, Brush b, Rectangle r)
        {
            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.HighQuality;
            int strokeOffset = Convert.ToInt32(Math.Ceiling(p.Width));
            Rectangle Bounds = Rectangle.Inflate(r, -strokeOffset, -strokeOffset);

            p.EndCap = p.StartCap = LineCap.Round;

            GraphicsPath gfxPath = new GraphicsPath();
            gfxPath.AddArc(Bounds.X, Bounds.Y, roundsize, roundsize, 180, 90);
            gfxPath.AddArc(Bounds.X + Bounds.Width - roundsize, Bounds.Y, roundsize, roundsize, 270, 90);
            gfxPath.AddArc(Bounds.X + Bounds.Width - roundsize, Bounds.Y + Bounds.Height - roundsize, roundsize, roundsize, 0, 90);
            gfxPath.AddArc(Bounds.X, Bounds.Y + Bounds.Height - roundsize, roundsize, roundsize, 90, 90);
            gfxPath.CloseAllFigures();

            g.FillPath(b, gfxPath);
            g.DrawPath(p, gfxPath);
            g.SmoothingMode = sm;
        }


        public abstract void DrawBackground(Graphics g, Point center, bool active);

        public abstract Size Dimensions
        {
            get;
        }
    }
}
