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
    class ContainerBackgroundDraw : BackgroundDrawBase
    {
        private int width;
        private int height;
        public ContainerBackgroundDraw(int w, int h)
        {
            width = w;
            height = h;
        }
        public override void DrawBackground(Graphics g, Point center, bool active)
        {
            Rectangle r = new Rectangle(center.X - Dimensions.Width / 2, center.Y - Dimensions.Height / 2, Dimensions.Width, Dimensions.Height);
            Pen p = new Pen(active?Border:GrayBorder, linewidth);
            Brush b = new System.Drawing.Drawing2D.LinearGradientBrush(r, active?BlueGrad1:GrayGrad1, active?BlueGrad2:GrayGrad2, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            RoundRect(g, p, b, r);            
        }

        public override Size Dimensions
        {
            get { return new Size(width, height); }
        }
    }
       
}
