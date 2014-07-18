using System.Drawing;
using System.IO;
using CSharpDecorator.Framework;
using GME;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GME.MGA;
using GME.MGA.Meta;
using System.Reflection;
using System.Diagnostics;

namespace GenericDecorator
{
    
    [Guid("504827B5-B2B2-4795-B1F9-474BB8512ABD"),
     ProgId("MGA.Decorator.GenericDecorator"),
     ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class GenericDecorator : IMgaElementDecorator, IMgaDecoratorCommon
    {
        
        public  GenericDecorator(IMgaElementDecoratorEvents events)
        {

        }

        public  GenericDecorator()
        {

        }

        ~GenericDecorator()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }


        

        private MgaFCO _currentObj;
        private MgaMetaFCO _metaobj;

        private DecoratorBase decoratorBase;


        
        

        #region IMgaElementDecorator

        public void Destroy()
        {
            decoratorBase.Destroy();
        }

        public void DragEnter(out uint dropEffect, ulong pCOleDataObject, uint keyState, int pointx, int pointy,
                              ulong transformHDC)
        {
            decoratorBase.DragEnter(out dropEffect, pCOleDataObject, keyState, pointx, pointy, transformHDC);
        }

        public void DragOver(out uint dropEffect, ulong pCOleDataObject, uint keyState, int pointx, int pointy,
                             ulong transformHDC)
        {
            decoratorBase.DragOver(out dropEffect, pCOleDataObject, keyState, pointx, pointy, transformHDC);   
        }


        public void Draw(uint hdc)
        {
            throw new NotImplementedException(); // Should not be called via the new interface
           
        }

        public void DrawEx(uint hdc, ulong gdip)
        {
            decoratorBase.DrawEx(hdc, gdip);
        }

        public void Drop(ulong pCOleDataObject, uint dropEffect, int pointx, int pointy, ulong transformHDC)
        {
            decoratorBase.Drop(pCOleDataObject, dropEffect, pointx, pointy, transformHDC);
        }

        public void DropFile(ulong hDropInfo, int pointx, int pointy, ulong transformHDC)
        {
            decoratorBase.DropFile(hDropInfo, pointx, pointy, transformHDC);
        }

        public void GetFeatures(out uint features)
        {
            decoratorBase.GetFeatures(out features);
        }

        private Rectangle LabelLocation { get; set; }
        public void GetLabelLocation(out int sx, out int sy, out int ex, out int ey)
        {
            decoratorBase.GetLabelLocation(out sx, out sy, out ex, out ey);
        }

        public void GetLocation(out int sx, out int sy, out int ex, out int ey)
        {
            decoratorBase.GetLocation(out sx, out sy, out ex, out ey);
        }

        public void GetMnemonic(out string mnemonic)
        {
            mnemonic = "MGA.Decorator.GenericDecorator";
        }

        public void GetParam(string Name, out object value)
        {
            value = null;
        }

        public void GetPortLocation(MgaFCO fco, out int sx, out int sy, out int ex, out int ey)
        {
            decoratorBase.GetPortLocation(fco, out sx, out sy, out ex, out ey);
        }

        public MgaFCOs GetPorts()
        {
            return decoratorBase.GetPorts();
        }

        public void GetPreferredSize(out int sizex, out int sizey)
        {
            decoratorBase.GetPreferredSize(out sizex, out sizey);
        }



        public void Initialize(MgaProject p, MgaMetaPart meta, MgaFCO obj)
        {
            throw new NotImplementedException(); // Old decorator interface; should not be called.
        }


        private IntPtr parentHwnd;
        private MgaProject Project;
        private string MetaName;
        public void InitializeEx(MgaProject project, MgaMetaPart meta, MgaFCO obj, IMgaCommonDecoratorEvents eventSink,
                                 ulong parentWnd)
        {

            string name = meta.ParentAspect.Name;
            _currentObj = obj;
            _metaobj = meta.Role.Kind;
            Project = project;

            MetaName = _metaobj.Name;
            if (obj != null) MetaName = obj.Meta.Name;

            unchecked
            {
                parentHwnd = (IntPtr)(int)parentWnd;
            }

            IBackroundDraw bgdraw = null;

            if (_metaobj.ObjType == objtype_enum.OBJTYPE_MODEL)
            {

                bgdraw = new ContainerBackgroundDraw(100, 50);
            }
            else if (_metaobj.ObjType == objtype_enum.OBJTYPE_ATOM)
            {
                bgdraw = new AtomBackgroundDraw();
            }
            else if (_metaobj.ObjType == objtype_enum.OBJTYPE_SET)
            {
                bgdraw = new SetBackgroundDraw();
            }
            else if (_metaobj.ObjType == objtype_enum.OBJTYPE_REFERENCE)
            {
                bgdraw = new RefBackgroundDraw();
            }

            decoratorBase = new DefaultDecorator(this, parentHwnd, _currentObj, meta, _metaobj, Project, bgdraw);
            
        }


        public void MenuItemSelected(uint menuItemId, uint nFlags, int pointx, int pointy, ulong transformHDC)
        {
            decoratorBase.MenuItemSelected(menuItemId, nFlags, pointy, pointy, transformHDC);
        }

        #region Mouse events

        public void MouseLeftButtonDoubleClick(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {
            decoratorBase.MouseLeftButtonDoubleClick(nFlags, pointx, pointy, transformHDC);
        }

        
        public void MouseLeftButtonDown(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {            
                decoratorBase.MouseLeftButtonDown(nFlags, pointx, pointy, transformHDC);
        }

        public void MouseLeftButtonUp(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {
            decoratorBase.MouseLeftButtonUp(nFlags, pointx, pointy, transformHDC);
        }

        public void MouseMiddleButtonDoubleClick(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {
            decoratorBase.MouseMiddleButtonDoubleClick(nFlags, pointx, pointy, transformHDC);
        }

        public void MouseMiddleButtonDown(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {
            decoratorBase.MouseMiddleButtonDown(nFlags, pointx, pointy, transformHDC);
        }

        public void MouseMiddleButtonUp(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {
            decoratorBase.MouseMiddleButtonUp(nFlags, pointx, pointy, transformHDC);
        }

        public void MouseMoved(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {
            decoratorBase.MouseMoved(nFlags, pointx, pointy, transformHDC);            
        }

        public void MouseRightButtonDoubleClick(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {
            decoratorBase.MouseRightButtonDoubleClick(nFlags, pointx, pointy, transformHDC);
        }

        public void MouseRightButtonDown(ulong hCtxMenu, uint nFlags, int pointx, int pointy, ulong transformHDC)
        {
            decoratorBase.MouseRightButtonDown(hCtxMenu, nFlags, pointx, pointy, transformHDC);
        }

        public void MouseRightButtonUp(uint nFlags, int pointx, int pointy, ulong transformHDC)
        {
            decoratorBase.MouseRightButtonUp(nFlags, pointx, pointy, transformHDC);
        }

        public void MouseWheelTurned(uint nFlags, int distance, int pointx, int pointy, ulong transformHDC)
        {
            decoratorBase.MouseWheelTurned(nFlags, distance, pointx, pointy, transformHDC);

        }

        #endregion

        public void OperationCanceled()
        {
            decoratorBase.OperationCanceled();
        }

        public void SaveState()
        {
            decoratorBase.SaveState();
        }

        public void SetActive(bool isActive)
        {
            decoratorBase.SetActive(isActive);
        }

        
        public void SetLocation(int sx, int sy, int ex, int ey)
        {
            decoratorBase.SetLocation(sx, sy, ex, ey);
        }

        public void SetParam(string Name, object value)
        {
            decoratorBase.SetParam(Name, value);
        }

        
        public void SetSelected(bool isSelected)
        {
            decoratorBase.SetSelected(isSelected);
        }

        #endregion

        /// <summary>
        /// Get the meta object
        /// </summary>
        /// <param name="metaPart">MetaPart</param>
        /// <param name="metaFco">returns the MetaFCO</param>
        /// <returns>true if succeeded</returns>
        private bool GetMetaFCO(MgaMetaPart metaPart, out MgaMetaFCO metaFco)
        {
            metaFco = null;
            if (metaPart == null)
                return false;

            metaFco = metaPart.Role.Kind;
            return metaFco != null;
        }

        /// <summary>
        /// Return GME registry value
        /// </summary>
        /// <param name="val">returned value</param>
        /// <param name="path">path in registry</param>
        /// <returns>true if settins found and non-empty</returns>
        public bool GetPreference(out string val, string path)
        {
            val = null;
            if (_currentObj != null)
            {
                val = _currentObj.RegistryValue[path];
            }
            else if (_metaobj != null)
            {
                val = _metaobj.RegistryValue[path];              
            }
            return !string.IsNullOrEmpty(val);
        }

        // Get integer property from GME registry (if hex true, string is handled as hexa starting with 0x)
        private bool GetPreference(out int val, string path, bool hex)
        {
            val = 0;
            string strVal;
            if (GetPreference(out strVal, path))
            {
                if (hex && strVal.Length >= 2)
                {
                    return int.TryParse(strVal.Substring(2), // omit 0x
                                        System.Globalization.NumberStyles.HexNumber,
                                        System.Globalization.NumberFormatInfo.InvariantInfo,
                                        out val);
                }
                else if (!hex)
                {
                    return int.TryParse(strVal, out val);
                }
            }
            return false;
        }

        // Get color settings
        private bool GetColorPreference(out Color color, string path)
        {
            int i;
            if (GetPreference(out i, path, true))
            {
                int r = (i & 0xff0000) >> 16;
                int g = (i & 0xff00) >> 8;
                int b = i & 0xff;
                color = Color.FromArgb(255, r, g, b);
                return true;
            }
            color = Color.Black;
            return false;
        }

    }

}

