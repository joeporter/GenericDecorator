using CSharpDecorator.Framework;
using GME;
using GME.MGA;
using GME.MGA.Meta;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace F6Decorator
{
    class DecoratorBitmapBoxDecorator:DecoratorUiBase
    {
        Type formType;

        public DecoratorBitmapBoxDecorator(F6Decorator decorator,
            IntPtr parentHwnd, MgaFCO fco, MgaMetaPart meta, MgaMetaFCO metafco, MgaProject project,  IMgaCommonDecoratorEvents eventSink, Type formType) :
            base(parentHwnd, fco, project, true)
        {
            this.formType = formType;

            Type boxDecoratorType = Type.GetTypeFromProgID("Mga.BoxDecorator", true);
            boxDecorator = Activator.CreateInstance(boxDecoratorType);
            
            /*public void InitializeEx(MgaProject project, MgaMetaPart meta, MgaFCO obj, IMgaCommonDecoratorEvents eventSink,
                                 ulong parentWnd)*/
            boxDecoratorType.InvokeMember("InitializeEx", BindingFlags.InvokeMethod, null, boxDecorator, new object[] {project, meta, fco, eventSink,(ulong) parentHwnd });


        }

        protected override void DrawSelected(Graphics g)
        {
            return;
        }

        protected override void DrawNormal(Graphics g)
        {
            return;
        }

        object boxDecorator;
        Type boxDecoratorType;


        public override void DetailButtonLClick()
        {
            // Supported ways of showing a form:
            ShowWin32Form(formType);
            //  ShowWin32FormModal(typeof(WinformsTestWindow.WinformsTestWindow));
            //  ShowWpfFormModal(typeof(WpfTestWindows.WpfTestWindow));
            //  ShowWpfForm(typeof(WpfTestWindows.WpfTestWindow));
        }

    


        public virtual void DrawEx(uint hdc, ulong gdip)
        {
            boxDecoratorType.InvokeMember("DrawEx", BindingFlags.InvokeMethod, null, boxDecorator, new object[]{ hdc, gdip });
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
            throw new NotImplementedException();
        }

        public virtual void DropFile(ulong hDropInfo, int pointx, int pointy, ulong transformHDC)
        {
            throw new NotImplementedException();
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

        public virtual void GetLocation(out int sx, out int sy, out int ex, out int ey)
        {
            sx = DecoratorArea.Left;
            sy = DecoratorArea.Top;
            ex = DecoratorArea.Right;
            ey = DecoratorArea.Bottom;
        }

        public virtual void GetMnemonic(out string mnemonic)
        {
            throw new NotImplementedException();
        }



        public virtual void GetParam(string Name, out object value)
        {
            throw new NotImplementedException();
        }

        public virtual void GetPortLocation(GME.MGA.MgaFCO fco, out int sx, out int sy, out int ex, out int ey)
        {
            sx = sy = ex = ey = 0; // By default we don't support ports
        }

        public virtual GME.MGA.MgaFCOs GetPorts()
        {
            return null; // By default we don't support ports
        }

       



        public virtual void MenuItemSelected(uint menuItemId, uint nFlags, int pointx, int pointy, ulong transformHDC)
        {

        }

        public virtual void MouseLeftButtonDoubleClick(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {

        }

        public virtual void MouseLeftButtonDown(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {

        }

        public virtual void MouseLeftButtonUp(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {

        }

        public virtual void MouseMiddleButtonDoubleClick(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {

        }

        public virtual void MouseMiddleButtonDown(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {

        }

        public virtual void MouseMiddleButtonUp(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {

        }

        public virtual void MouseMoved(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {
            
        }

      


        public virtual void MouseRightButtonDoubleClick(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {

        }

        public virtual void MouseRightButtonDown(ulong hCtxMenu, uint nFlags, int pointx, int pointy, ulong transformHDC)
        {

        }

        public virtual void MouseRightButtonUp(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {

        }

        public virtual void MouseWheelTurned(uint nFlags, int distance, int pointx, int pointy, ulong transformHDC)
        {

        }

        public virtual void OperationCanceled()
        {
            throw new NotImplementedException();
        }

        public virtual void SaveState()
        {
            throw new NotImplementedException();
        }

        public virtual void SetActive(bool isActive)
        {
            throw new NotImplementedException();
        }

        public virtual Rectangle DecoratorArea { get; set; }

        public void SetLocation(int sx, int sy, int ex, int ey)
        {
            DecoratorArea = new Rectangle(sx, sy, ex - sx - 1, ey - sy - 1);


        }


        public virtual void SetParam(string Name, object value)
        {
            throw new NotImplementedException();
        }

        protected bool IsSelected { get; set; }
        public virtual void SetSelected(bool isSelected)
        {
            IsSelected = isSelected;

        }
    }
}
