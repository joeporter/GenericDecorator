using System.Collections.Generic;
using System.Windows;
using CSharpDecorator;
using GME.MGA;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using GenericDecorator;
using System.IO;
using System.Reflection;

namespace CSharpDecorator.Framework
{
    public abstract class DecoratorUiBase:DecoratorBase
    {
        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hwnd, IntPtr lpRect, bool bErase);
        
        protected IntPtr ParentHwnd { get; set; }
        protected MgaFCO MgaFCO { get; set; }
        protected MgaProject MgaProject { get; set; }
        protected Point MousePosition { get; set; }

        protected bool Active = true;

        public override void Draw(Graphics g)
        {
            // Draw decorator
            if (IsSelected)
            {
                DrawSelected(g);

            }
            else
            {
                DrawNormal(g);
            }


            // Check if resize is needed
           // ResizeWin32Form();            
        }

        protected abstract void DrawNormal(Graphics g);
        protected abstract void DrawSelected(Graphics g);
        
        protected DecoratorUiBase(IntPtr parentHwnd, MgaFCO fco=null, MgaProject project=null, bool isDetailButtonEnabled=false)
        {
            ParentHwnd = parentHwnd;
            DecoratorArea = new Rectangle(0, 0, 0, 0);

            MgaFCO = fco;
            MgaProject = project;
        }


        protected Rect parentSize;



        public override void MouseLeftButtonDown(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {

        }

        public override void MousePositionChanged(Point p)
        {

        }

        private void InvalidateArea(Rect rec)
        {
            IntPtr recPtr = Marshal.AllocHGlobal(Marshal.SizeOf(rec));
            try
            {
                Marshal.StructureToPtr(rec, recPtr, false);
                InvalidateRect(ParentHwnd, recPtr, true);
            }
            finally
            {
                Marshal.FreeHGlobal(recPtr);
            }
        }

   
        public override void GetPreferredSize(out int sizex, out int sizey)
        {
            sizex = 200;
            sizey = 100;
        }

        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);
            Active = isActive;
        }

        public override void SetParam(string Name, object value)
        {
            
        }

       
    }
}
