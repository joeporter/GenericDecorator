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
    interface IBackroundDraw
    {
        void DrawBackground(Graphics g, Point center, bool active);
        Size Dimensions
        {
            get;
        }
    }
}
