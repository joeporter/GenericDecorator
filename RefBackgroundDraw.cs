using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CSharpDecorator.Framework;
using GME.MGA;
using GME.MGA.Meta;
using GME.Util;

namespace GenericDecorator
{
    class RefBackgroundDraw : BackgroundDrawBase
    {
        public override void DrawBackground(Graphics g, Point center, bool active)
        {
            Rectangle r = new Rectangle(center.X - Dimensions.Width / 2, center.Y - Dimensions.Height / 2, Dimensions.Width, Dimensions.Height);
            Brush b = new System.Drawing.Drawing2D.LinearGradientBrush(r, active?BlueGrad1:GrayGrad1, active?BlueGrad2:GrayGrad2, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            Pen p = new Pen(active?(RefBorder):GrayBorder, linewidth);
            RoundRect(g, p, b, r);
        }

        public override System.Drawing.Size Dimensions
        {
            get { return new System.Drawing.Size(50, 50); }
        }
    }
}
