using GME;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GenericDecorator
{
    public abstract class DecoratorBase
    {

        internal struct Defines
        {
            public const uint F_RESIZABLE = 1 << 0;
            public const uint F_MOUSEEVENTS = 1 << 1;
            public const uint F_HASLABEL = 1 << 2;
            public const uint F_HASSTATE = 1 << 3;
            public const uint F_HASPORTS = 1 << 4;
            public const uint F_ANIMATION = 1 << 5;
            public const uint F_IMGPATH = 1 << 6;
            public const uint F_RESIZEAFTERMOD = 1 << 7;
        };

        public abstract void Draw(Graphics g);
        

        public virtual void Draw(uint hdc)
        {
            // VERY important to cast to int
            IntPtr hdcptr;
            unchecked
            {
                hdcptr = (IntPtr)(int)hdc;
            }

            // Create graphics object
            Graphics g = Graphics.FromHdc(hdcptr);

            // Draw decorator
            Draw(g);

            g.Dispose();
        }


        public virtual void DrawEx(uint hdc, ulong gdip)
        {
            Draw(hdc);
        }
      
        public virtual void Destroy()
        {

        }



        public virtual void DragEnter(out uint dropEffect, ulong pCOleDataObject, uint keyState, int pointx, int pointy, ulong transformHDC)
        {
            dropEffect = 0;
        }

        public virtual void DragOver(out uint dropEffect, ulong pCOleDataObject, uint keyState, int pointx, int pointy, ulong transformHDC)
        {
            dropEffect = 0;
        }





        public virtual void Drop(ulong pCOleDataObject, uint dropEffect, int pointx, int pointy, ulong transformHDC)
        {
            // No implementation needed
        }

        public virtual void DropFile(ulong hDropInfo, int pointx, int pointy, ulong transformHDC)
        {
            // TODO: Implementation
#if DEBUG
            throw new NotImplementedException();
#endif
        }

        public virtual void GetFeatures(out uint features)
        {

            // Append other features with |
            features = Defines.F_MOUSEEVENTS;
        }

        protected Rectangle LabelLocation { get; set; }
        public virtual void GetLabelLocation(out int sx, out int sy, out int ex, out int ey)
        {
            sx = LabelLocation.Left;
            sy = LabelLocation.Top;
            ex = LabelLocation.Width;
            ey = LabelLocation.Height;
        }

        public  virtual void GetLocation(out int sx, out int sy, out int ex, out int ey)
        {
            sx = DecoratorArea.Left;
            sy = DecoratorArea.Top;
            ex = DecoratorArea.Right;
            ey = DecoratorArea.Bottom; 
        }

        public  virtual void GetMnemonic(out string mnemonic)
        {
#if DEBUG
            throw new NotImplementedException();
#else
            mnemonic = "";
#endif
        }



        public  virtual void GetParam(string Name, out object value)
        {
#if DEBUG
            throw new NotImplementedException();
#else
            value = null;
#endif
        }

        public virtual  void GetPortLocation(GME.MGA.MgaFCO fco, out int sx, out int sy, out int ex, out int ey)
        {
            sx = sy = ex = ey = 0; // By default we don't support ports
        }

        public  virtual GME.MGA.MgaFCOs GetPorts()
        {
            return null; // By default we don't support ports
        }

        public abstract void GetPreferredSize(out int sizex, out int sizey);
       


        public  virtual void MenuItemSelected(uint menuItemId, uint nFlags, int pointx, int pointy, ulong transformHDC)
        {
      
        }

        public  virtual void MouseLeftButtonDoubleClick(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {
           
        }

        public  virtual void MouseLeftButtonDown(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {
            
        }

        public  virtual void MouseLeftButtonUp(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {

        }

        public  virtual void MouseMiddleButtonDoubleClick(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {

        }

        public  virtual void MouseMiddleButtonDown(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {

        }

        public  virtual void MouseMiddleButtonUp(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {

        }

        public  virtual void MouseMoved(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {
            MousePositionChanged(new Point(pointx, pointy));
        }

        public abstract void MousePositionChanged(Point p);
        

        public virtual  void MouseRightButtonDoubleClick(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {

        }

        public  virtual void MouseRightButtonDown(ulong hCtxMenu, uint nFlags, int pointx, int pointy, ulong transformHDC)
        {

        }

        public  virtual void MouseRightButtonUp(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {
         
        }

        public  virtual void MouseWheelTurned(uint nFlags, int distance, int pointx, int pointy, ulong transformHDC)
        {
    
        }

        public  virtual void OperationCanceled()
        {
#if DEBUG
            throw new NotImplementedException();
#endif
        }

        public  virtual void SaveState()
        {
#if DEBUG
            throw new NotImplementedException();
#endif
        }

        public  virtual void SetActive(bool isActive)
        {
            // THIS IS IMPLEMENTED, please don't put the exception here. 
            return;
        }

        public  virtual Rectangle DecoratorArea { get; set; }

        public void SetLocation(int sx, int sy, int ex, int ey)
        {
            DecoratorArea = new Rectangle(sx, sy, ex - sx - 1, ey - sy - 1);

           
        }


        public virtual void SetParam(string Name, object value)
        {
            throw new NotImplementedException();
        }

        protected bool IsSelected { get; set; }
        public virtual  void SetSelected(bool isSelected)
        {
            IsSelected = isSelected;

        }
    }
}
