using GME.MGA;
using GME.MGA.Meta;
using GME.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace GenericDecorator
{
    class Port
    {
        public MgaFCO portFCO;
        public string portName;
        public int x, y; // Port coordinates when they are displayed in the model on a sheet
        public Image image;
        public Size imageSize;
        public Size labelSize;
        
    }

    public class FilterCondition
    {
       public string aspectName;
       public string roleName;
       public string parentKind;
       public MgaFCO portFCO;
       public MgaFCO part;
    }

    class DefaultDecorator : DefaultBitmapDecorator
    {


        public readonly string defaultPortIconStr = string.Format("GenericDecorator.Images.{0}", "generalport.emf");

        public DefaultDecorator(GenericDecorator decorator,
            IntPtr parentHwnd, MgaFCO fco, MgaMetaPart metaPart, MgaMetaFCO metafco, MgaProject project, IBackroundDraw bgdraw, Type formType = null) :
            base(decorator, parentHwnd, fco, metafco, project, bgdraw, formType)
        {
            this.metaPart = metaPart;
            this.decorator = decorator;
      

            LoadPorts();

            if (portList != null && portList.Count != 0)
            {
                LoadlPortIcons();
                OrderPorts();
                ComputePortLayout();
            }
        }


        GenericDecorator decorator;

        Font portLabelFont = new Font("Arial", 8, FontStyle.Bold);

        MgaMetaPart metaPart;

        List<Port> portList;
        Dictionary<MgaFCO, Rectangle> portLayout;

        List<Port> leftPorts;
        List<Port> rightPorts;
        Size preferredSize;
        Rectangle imageLayout;

        Image defPortImage = null;

        const int yspacing = 1;
        const int xspacing = 5;

        int maxPortWidth;

        private void ComputePortLayout()
        {
            portLayout = new Dictionary<MgaFCO, Rectangle>();
        
            int maxLeftWidth = 0;
            int maxLeftLabelWidth = 0;

            int leftHeight = 0;
            int rightHeight = 0;
           
            foreach (Port port in leftPorts)
            {
                maxLeftWidth = Math.Max(port.imageSize.Width, maxLeftWidth);
                maxLeftLabelWidth = Math.Max(port.labelSize.Width, maxLeftLabelWidth);
                leftHeight += yspacing + port.imageSize.Height;
            }

            int maxRightWidth = 0;
            int maxRightLabelWidth = 0;

            foreach (Port port in rightPorts)
            {
                maxRightWidth = Math.Max(port.imageSize.Width, maxRightWidth);
                maxRightLabelWidth = Math.Max(port.labelSize.Width, maxRightLabelWidth);
                rightHeight += yspacing + port.imageSize.Height;
            }

            maxLeftLabelWidth = maxRightLabelWidth = Math.Max(maxRightLabelWidth, maxLeftLabelWidth); 
            maxPortWidth = maxLeftWidth = maxRightWidth = Math.Max(maxRightWidth, maxLeftWidth);
            int maxHeight = Math.Max(leftHeight, rightHeight);
            maxHeight = Math.Max(image.Height, maxHeight); // Ports might exceed the height of the image

           


            preferredSize = new Size(maxLeftLabelWidth + maxLeftWidth + image.Width + maxRightLabelWidth + maxRightWidth + 6 * xspacing, maxHeight + 8*yspacing + BackgroundDrawBase.roundsize * 2);
            imageLayout = new Rectangle(maxLeftLabelWidth + maxLeftWidth + 3 * xspacing, yspacing * 4 + (maxHeight - image.Height) / 2 + BackgroundDrawBase.roundsize, image.Width, image.Height);

            int ystart = yspacing + BackgroundDrawBase.roundsize;

            foreach (Port port in leftPorts)
            {

                portLayout.Add(port.portFCO, new Rectangle(xspacing, ystart, port.imageSize.Width, port.imageSize.Height));
                ystart += yspacing + port.imageSize.Height;
            }

            ystart = yspacing + BackgroundDrawBase.roundsize;
            foreach (Port port in rightPorts)
            {
                portLayout.Add(port.portFCO, new Rectangle(preferredSize.Width - xspacing - port.imageSize.Width, ystart, port.imageSize.Width, port.imageSize.Height));
                ystart += yspacing + port.imageSize.Height;
            }


        }

        private static int ComparePorts(Port x, Port y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're 
                    // equal.  
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y 
                    // is greater.  
                    return -1;
                }
            }
            else
            {
                // If x is not null... 
                // 
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the  
                    // lengths of the two strings. 
                    // 

                    if (x.y < y.y)
                    {
                        return -1;
                    }
                    else if (x.y > y.y)
                    {
                        return 1;

                    }
                    else
                    {
                        return 0;
                    }

                }
            }
        }

        private void OrderPorts()
        {            

            // Getting the horizontal span
            int xIconMin = int.MaxValue;
            int xIconMax = 0;
        
        

            foreach (Port port in portList)
            {
                xIconMin = Math.Min(port.x, xIconMin);
                xIconMax = Math.Max(port.x, xIconMax);                
            }

            // Separating ports vertically
            const int WIDTH_MODELSIDE = 100;
            leftPorts = new List<Port>();
            rightPorts = new List<Port>();

            foreach (Port port in portList)
            {
                if (port.x <= WIDTH_MODELSIDE || port.x < (xIconMax - xIconMin) / 2)
                {
                    leftPorts.Add(port);
                }
                else
                {
                    rightPorts.Add(port);
                }
            }

          

            // Ordering ports
            leftPorts.Sort(ComparePorts);
            rightPorts.Sort(ComparePorts);

          // leftPorts.OrderBy(p=>p.y);
           // rightPorts.OrderBy(p => p.y);

             
        }


        private void LoadlPortIcons()
        {

            foreach (Port port in portList)
            {
                string portIconName = port.portFCO.RegistryValue["porticon"];
                string iconName = port.portFCO.RegistryValue["icon"];


                string projectDir = Path.GetDirectoryName(MgaProject.ProjectConnStr.Substring(4));
                string paradigmDir = Path.GetDirectoryName(MgaProject.ParadigmConnStr.Substring(4));


                MgaRegistrar registrar = new MgaRegistrar();
                StringBuilder portIconPath = new StringBuilder(registrar.IconPath[regaccessmode_enum.REGACCESS_BOTH]);


                portIconPath.Replace("$PROJECTDIR", projectDir);
                portIconPath.Replace("$PARADIGMDIR", paradigmDir);

                string[] paths = portIconPath.ToString().Split(';');

                Image image = null;
                if (portIconName != null)
                {
                    foreach (string path in paths)
                    {
                        string fileName = Path.Combine(path, portIconName);
                        if (File.Exists(fileName))
                        {
                            image = Image.FromFile(fileName);
                            break;
                        }
                    }
                }

                if (image == null && iconName != null) // Using the shrunk version of the port's icon
                {
                    foreach (string path in paths)
                    {
                        string fileName = Path.Combine(path, iconName);
                        if (File.Exists(fileName))
                        {
                            image = Image.FromFile(fileName);
                            break;
                        }
                    }
                }
                
                if (image == null) // Check FCO port's image specified in its PortIcon attribute
                {
                    if (defPortImage == null)
                    {
                        Stream defaultPortIconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(defaultPortIconStr);

                        if (defaultPortIconStream != null)
                        {
                            defPortImage = image = Image.FromStream(defaultPortIconStream);
                        }
                    }
                    else
                    {
                        image = defPortImage;
                    }
                }

                port.image = image;
                
                // TODO: Uncomment it
                // port.imageSize = new Size(image.Width, image.Height);
                port.imageSize = new Size(iconSizeX, iconSizeY);
               

            }

        }

        public override GME.MGA.MgaFCOs GetPorts()
        {
            if (portList == null || portList.Count == 0)
            {
                return null;
            }

            //MgaFCOs ports = new MgaFCOs();
            MgaFCOs ports = (MgaFCOs)Activator.CreateInstance(Type.GetTypeFromProgID("Mga.MgaFCOs"));

            foreach (Port port in portList)
            {
                ports.Append(port.portFCO);
            }

            return ports;

        }

        [DllImport("user32")]
        public static extern System.IntPtr GetDC(IntPtr hwnd);

        public void LoadPorts()
        {
            MgaModel model = MgaFCO as MgaModel;


            if (model == null)
            {
                MgaReference reference = MgaFCO as MgaReference;

                if (reference != null)
                {
                    model = reference.Referred as MgaModel;
                }

                if (model == null)
                {
                    return;
                }
            }

            MgaMetaAspect parentAspect = metaPart.ParentAspect;
            MgaMetaModel metaModel = model.Meta as MgaMetaModel;

            string aspectName = metaPart.KindAspect;
            if (String.IsNullOrEmpty(aspectName))
            {
                aspectName = parentAspect.Name;
            }

            MgaMetaAspect aspect = null;

            try
            {
                aspect = metaModel.AspectByName[aspectName];

            }
            catch (COMException ex)
            {
                if (ex.ErrorCode != -2139942905)
                {
                    throw;
                }
            }


            // This makes the whole thing very lenient, but that is the code in the original decorator
            if (aspect == null)
            {              
                try
                {
                    aspect = metaModel.Aspects[1];
                }
                catch(COMException)
                {
                }

            }

            if (aspect == null)
            {
                return;
            }

         
            portList = new List<Port>();
            portLayout = new Dictionary<MgaFCO, Rectangle>();

            foreach (MgaFCO childFCO in model.ChildFCOs)
            {

                MgaPart part = null; ;
 
                try
                {
                   part = childFCO.get_Part(aspect);
                }
                catch (COMException)
                {
                }

                if (part == null)
                {
                    continue;
                }

              
                MgaMetaPart partMeta = part.Meta;

                if (partMeta.IsLinked)
                {                
                    if (!CheckPortRule(new FilterCondition { aspectName = aspectName, parentKind = model.ParentModel==null?null: model.ParentModel.Meta.Name, roleName = part.MetaRole.Name, portFCO = childFCO}))
                        {
                            continue; // Skipping this port
                        }
                    

                    int x = 0, y = 0;
                    string icon; // Not used
                    part.GetGmeAttrs(out icon, out x, out y);

                    Size labelSize;
                    using (Graphics g = Graphics.FromHdc(GetDC(ParentHwnd)))
                    {
                        labelSize = Size.Ceiling( g.MeasureString(childFCO.Name, portLabelFont));
                    }


                        
                    portList.Add(new Port{portFCO = childFCO, x = x, y=y, labelSize = labelSize, portName = childFCO.Name });
                }

            }


        }

        protected virtual bool CheckPortRule(FilterCondition fc)
        {
            /* Here we can override whether or not ports should be displayed 
            if (MgaFCO != null && MgaFCO.MetaRole.Name == "some MetaGME class name")
            {
                return false;
            } */
            return true;
        }

        public override void GetPortLocation(MgaFCO fco, out int sx, out int sy, out int ex, out int ey)
        {

            Rectangle layout = portLayout[fco];

            sx = layout.X;
            sy = layout.Y;
            ex = layout.Right;
            ey = layout.Bottom;
        }






        // Ugly hack.
        // TODO: Remove this
        const int iconSizeX = 10;
        const int iconSizeY = 11;



        public override void Draw(Graphics g)
        {
            if (portList == null || portList.Count == 0)
            {
                base.Draw(g);
                return;
            }


            (new ContainerBackgroundDraw(DecoratorArea.Width,DecoratorArea.Height)).DrawBackground(g, new Point(DecoratorArea.X+DecoratorArea.Width/2, DecoratorArea.Y+DecoratorArea.Height/2), Active);

            //g.DrawImage(defShadowImage, DecoratorArea);
            // Drawing the image
            Rectangle imageRectangle = new Rectangle(DecoratorArea.X+ imageLayout.X, DecoratorArea.Y+imageLayout.Y, imageLayout.Width,imageLayout.Height);

            g.DrawImage(image, imageRectangle);

            // Drawing label
            Rectangle textArea = new Rectangle(new Point(imageRectangle.Left, DecoratorArea.Bottom), new Size(imageRectangle.Width, 0));
            RectangleF textAreaF = textArea;

            Font font = new Font("Arial", 10);

            SizeF textSizeF = g.MeasureString(fcoName, font);
            textAreaF.Height = textSizeF.Height;
            textAreaF.X += (textAreaF.Width - textSizeF.Width) / 2;
            textAreaF.Width = textSizeF.Width;
            StringFormat format = new StringFormat();           
            format.Alignment = StringAlignment.Center;

            SolidBrush brush = new SolidBrush(Color.Black);
            //SolidBrush brush = new SolidBrush(Color.White);

            g.DrawString(fcoName, font, brush, textAreaF, format);

            brush.Color = Color.White;

            // Drawing ports
            format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            foreach (Port port in leftPorts)
            {
                Rectangle layout = portLayout[port.portFCO];

                Rectangle portRectangle = new Rectangle(DecoratorArea.X+layout.X, DecoratorArea.Y + layout.Y, layout.Width, layout.Height);
                g.DrawImage(port.image, portRectangle);

                Rectangle portLabelRectangle = new Rectangle(portRectangle.Right + xspacing, portRectangle.Y,  port.labelSize.Width, portRectangle.Height);
            
                g.DrawString(port.portName, portLabelFont, brush, portLabelRectangle,format);

            }

            foreach (Port port in rightPorts)
            {
                Rectangle layout = portLayout[port.portFCO];

                Rectangle portRectangle = new Rectangle(DecoratorArea.X + layout.X, DecoratorArea.Y + layout.Y, layout.Width, layout.Height);
                g.DrawImage(port.image, portRectangle);

                Rectangle portLabelRectangle = new Rectangle(portRectangle.Left - xspacing - port.labelSize.Width, portRectangle.Y, port.labelSize.Width, portRectangle.Height);

                g.DrawString(port.portName, portLabelFont, brush, portLabelRectangle, format);
            }


            DrawEmbellishments(g, DecoratorArea);
        }


        public override void GetPreferredSize(out int sizex, out int sizey)
        {
            if (portList == null || portList.Count == 0)
            {
                base.GetPreferredSize(out sizex, out sizey);
            }
            else
            {
                sizex = preferredSize.Width;
                sizey = preferredSize.Height;
            }
        }
    }

}
