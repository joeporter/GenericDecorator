using GME.MGA;
using GME.MGA.Meta;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GenericDecorator
{

    // FCO must have a IsWellFormed Boolean attribute
    class ReadOnlyDecorator: DefaultBitmapDecorator
    {

        public ReadOnlyDecorator(GenericDecorator decorator, 
            IntPtr parentHwnd, MgaFCO fco, MgaMetaFCO metafco, MgaProject project, Type formType, IBackroundDraw bgdraw)
            : base(decorator, parentHwnd, fco, metafco, project, bgdraw, formType)
        {
            
        }

        protected override void DrawNormal(Graphics g)
        {

            base.DrawNormal(g);            

            
        }

    }
}
