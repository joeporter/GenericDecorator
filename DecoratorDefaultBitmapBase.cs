using CSharpDecorator.Framework;
using GME.MGA;
using GME.MGA.Meta;
using GME.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GenericDecorator
{
    enum ModifierType { NONE, INSTANCE, TYPE, SUBTYPE };

    class DefaultBitmapDecorator : DecoratorUiBase
    {

        protected readonly string defaultIconStr = string.Format("GenericDecorator.Images.{0}", "generalicon.emf");

        protected enum EmbellishType
        {
            None,
            Instance,
            Reference,
            SubType,
            Type,
            NullReference,
            NullReferenceWarning
        }

        protected Image image { get; set; }

        protected EmbellishType Embellish;

        protected string fcoName;
        protected IBackroundDraw backgrounddraw;


        Image refImage=null;


        private Type formType;


        Image LoadImage(string resourceName)
        {
            Stream imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

            if (imageStream != null)
            {
                return Image.FromStream(imageStream);
            }

            return null;
        }
        ~DefaultBitmapDecorator()
        {

        }

        public DefaultBitmapDecorator(GenericDecorator decorator,
            IntPtr parentHwnd, MgaFCO fco, MgaMetaFCO metafco, MgaProject project, IBackroundDraw backgrounddraw, Type formType = null)
            : base(parentHwnd, fco, project, formType != null)
        {
    

            string iconName;
            decorator.GetPreference(out iconName, "icon");

            this.backgrounddraw = backgrounddraw;


            this.formType = formType;


            string projectDir = Path.GetDirectoryName(project.ProjectConnStr.Substring(4));
            string paradigmDir = Path.GetDirectoryName(project.ParadigmConnStr.Substring(4));


            MgaRegistrar registrar = new MgaRegistrar();
            StringBuilder iconPath = new StringBuilder(registrar.IconPath[regaccessmode_enum.REGACCESS_BOTH]);

            iconPath.Replace("$PROJECTDIR", projectDir);
            iconPath.Replace("$PARADIGMDIR", paradigmDir);

            string[] paths = iconPath.ToString().Split(';');

            foreach (string path in paths)
            {
                string fileName = Path.Combine(path, iconName);
                if (File.Exists(fileName))
                {
                    image = Image.FromFile(fileName);
                    break;
                }
            }

            if (image == null)
            {
                image = LoadImage(defaultIconStr);
            }

            if (fco != null)
            {
                fcoName = fco.Name;
                //fcoName = String.IsNullOrEmpty(metafco.DisplayedName) ? metafco.Name : metafco.DisplayedName;

                if (fco.ObjType == objtype_enum.OBJTYPE_REFERENCE)
                {

                    MgaReference reference = fco as MgaReference;
                    if (reference.Referred == null)
                    {
                        Embellish = EmbellishType.NullReference;
                    }
                    else
                    {
                        Embellish = EmbellishType.Reference;
                    }

                } else  if (fco.IsInstance)
                {
                    // instance

                    Embellish = EmbellishType.Instance;
                }
                else if (fco.DerivedFrom != null)
                {
                    // Subtype
                    Embellish = EmbellishType.SubType;
                }
                else if (fco.DerivedObjects.Count != 0)
                {
                    Embellish = EmbellishType.Type;
                }
                else
                {
                    Embellish = EmbellishType.None;
                }
            }
            else
            {
                fcoName = String.IsNullOrEmpty(metafco.DisplayedName) ? metafco.Name : metafco.DisplayedName;
                //fcoName = metafco.Name;
            }


        }

        protected override void DrawNormal(Graphics g)
        {

            if (backgrounddraw != null)
                backgrounddraw.DrawBackground(g, new Point(DecoratorArea.X+DecoratorArea.Width/2, DecoratorArea.Y+DecoratorArea.Height/2), Active);


            g.DrawImage(image, new Rectangle(DecoratorArea.X + (DecoratorArea.Width-image.Width)/2, DecoratorArea.Y + (DecoratorArea.Height-image.Height)/2, image.Width, image.Height));

            Rectangle textArea = new Rectangle(new Point(DecoratorArea.Left, backgrounddraw == null ? DecoratorArea.Bottom : (DecoratorArea.Y + DecoratorArea.Height / 2 + backgrounddraw.Dimensions.Height / 2)), new Size(DecoratorArea.Width, 0));
            RectangleF textAreaF = textArea;

            Font font = new Font("Arial", 10);

            SizeF textSizeF = g.MeasureString(fcoName, font);
            textAreaF.Height = textSizeF.Height;
            textAreaF.X += (textAreaF.Width - textSizeF.Width) / 2;
            textAreaF.Width = textSizeF.Width;
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Near; 

            g.DrawString(fcoName, font, new SolidBrush(Color.Black), textAreaF, format);

            DrawEmbellishments(g, DecoratorArea);

        }

        protected void DrawMarker(Graphics g, string s, Point p, Color bgColor)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Font f = new Font("Century", 8, FontStyle.Bold);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            Rectangle area = new Rectangle(p.X - 13 / 2, p.Y - 13 / 2, 13, 13);
            Rectangle shadow = area;
            shadow.X -= 1;
            shadow.Y += 1;
            shadow.Width += 1;
            shadow.Height += 1;
            Brush b = new SolidBrush(Color.FromArgb(128, Color.Black));
            g.FillEllipse(b, shadow);
            b = new SolidBrush(bgColor);
            g.FillEllipse(b, area);
            g.DrawEllipse(Pens.DarkBlue, area);
            g.DrawString(s, f, new SolidBrush(Color.Black), area, format);
        }

        protected void DrawEmbellishments(Graphics g, Rectangle area)
        {
            if (refImage != null)
            {
                g.DrawImage(refImage, new Rectangle(area.X, area.Y, 13, 13));
            }

            if (Embellish != EmbellishType.None)
            {
                Rectangle iconArea = new Rectangle(area.X + area.Width - 13, area.Y - 13/2, 13, 13);

                if (Embellish != EmbellishType.Reference)
                {
                    if (Embellish != EmbellishType.NullReference)
                    {
                        string s = "";
                        switch (Embellish)
                        {
                            case EmbellishType.Instance:
                                s = "I";
                                break;
                            case EmbellishType.SubType:
                                s = "S";
                                break;
                            case EmbellishType.Type:
                                s = "T";
                                break;
                        }
                        DrawMarker(g, s, new Point((iconArea.Right + iconArea.Left) / 2, (iconArea.Bottom + iconArea.Top) / 2), Active ? Color.LightBlue : Color.LightGray);
                    }
                    else if (Embellish == EmbellishType.NullReference)
                    {
                        DrawMarker(g, "!", new Point((iconArea.Right + iconArea.Left) / 2, (iconArea.Bottom + iconArea.Top) / 2), Active ? Color.Red : Color.LightGray);
                    }
                    
                }

            }

        }

        protected override void DrawSelected(Graphics g)
        {
            DrawNormal(g);
        }

        public override void GetPreferredSize(out int sizex, out int sizey)
        {
            if (backgrounddraw != null)
            {
                sizex = backgrounddraw.Dimensions.Width;
                sizey = backgrounddraw.Dimensions.Height;
            }
            else
            {
                sizex = image.Width;
                sizey = image.Height;
            }
        }

    }
}
